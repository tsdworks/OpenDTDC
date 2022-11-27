using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenDTDC.HMI
{
    public class HMIInstance
    {
        private byte Model;
        private byte Version;

        private readonly SerialPort SerialPort = new SerialPort();

        // 暂时不需要
        // private readonly byte[] ReceiveBuffer = new byte[Define.Communication.COMM_RECV_BUFFER_SIZE];
        // private readonly byte[] SendBuffer = new byte[Define.Communication.COMM_SEND_BUFFER_SIZE];

        // private int receivedSize = 0;

        private readonly object TaskSigintLockObject = new object();

        private bool TaskSigint = false;
        private bool ClosedByThread = false;

        private Task ReceiveHandler = null;
        private Task SendHandler = null;

        private DeviceData DeviceData = new DeviceData();

        // 连接事件
        public delegate void DeviceConnectedHandler(string portName);
        public delegate void DeviceDisconnectedHandler();

        public event DeviceConnectedHandler DeviceConnected;
        public event DeviceDisconnectedHandler DeviceDisconnected;

        // 数据事件
        // public delegate void DeviceReceivedDataUpdatedHandler(List<Tuple<string, int>> dataList);
        public delegate void DeviceSendDataUpdatedHandler(List<Tuple<string, Tuple<Define.HALPorts.ValueMode, object>>> dataList);

        // public event DeviceReceivedDataUpdatedHandler DeviceReceivedDataUpdated;
        public event DeviceSendDataUpdatedHandler DeviceSendDataUpdated;

        public bool Connect(string serialPortName = "")
        {
            bool retValue = false;

            try
            {
                if (SerialPort.IsOpen)
                {
                    try
                    {
                        SerialPort.Close();
                    }
                    catch (Exception) { };
                }

                if (!SerialPort.IsOpen)
                {
                    // 自动连接
                    if (serialPortName == string.Empty)
                    {
                        foreach (string serialPort in SerialPort.GetPortNames())
                        {
                            try
                            {
                                SerialPort.Close();
                            }
                            catch (Exception) { };

                            SerialPort.BaudRate = Define.Communication.COMM_BAUDRATE;
                            SerialPort.PortName = serialPort;
                            SerialPort.ReadTimeout = Define.Communication.COMM_TIMEOUT;
                            SerialPort.WriteTimeout = Define.Communication.COMM_TIMEOUT;

                            SerialPort.ReadBufferSize = Define.Communication.COMM_RECV_BUFFER_SIZE;
                            SerialPort.WriteBufferSize = Define.Communication.COMM_SEND_BUFFER_SIZE;

                            try
                            {
                                SerialPort.Open();

                                Thread.Sleep(100);

                                SerialPort.Write(Define.Communication.COMM_CONNECT_REQUEST);
                                SerialPort.Write(Define.Communication.COMM_TAIL, 0, Define.Communication.COMM_TAIL.Length);

                                Thread.Sleep(100);

                                string response = SerialPort.ReadExisting();

                                if (response.IndexOf(Define.Communication.COMM_CONNECT_RESPONSE) != -1)
                                {
                                    SerialPort.Write(Define.Communication.COMM_START_TRANS_REQUEST);
                                    SerialPort.Write(Define.Communication.COMM_TAIL, 0, Define.Communication.COMM_TAIL.Length);

                                    serialPortName = serialPort;

                                    break;
                                }
                            }
                            catch (Exception) { };
                        }
                    }
                    // 手动连接
                    else
                    {
                        SerialPort.BaudRate = Define.Communication.COMM_BAUDRATE;
                        SerialPort.PortName = serialPortName;
                        SerialPort.ReadTimeout = Define.Communication.COMM_TIMEOUT;
                        SerialPort.WriteTimeout = Define.Communication.COMM_TIMEOUT;

                        SerialPort.ReadBufferSize = Define.Communication.COMM_RECV_BUFFER_SIZE;
                        SerialPort.WriteBufferSize = Define.Communication.COMM_SEND_BUFFER_SIZE;

                        SerialPort.Open();

                        Thread.Sleep(100);

                        SerialPort.Write(Define.Communication.COMM_CONNECT_REQUEST);
                        SerialPort.Write(Define.Communication.COMM_TAIL, 0, Define.Communication.COMM_TAIL.Length);

                        Thread.Sleep(100);

                        string response = SerialPort.ReadExisting();

                        if (response.IndexOf(Define.Communication.COMM_CONNECT_RESPONSE) != -1)
                        {
                            SerialPort.Write(Define.Communication.COMM_START_TRANS_REQUEST);
                            SerialPort.Write(Define.Communication.COMM_TAIL, 0, Define.Communication.COMM_TAIL.Length);
                        }
                        else
                        {
                            SerialPort.Close();
                        }
                    }

                    if (SerialPort.IsOpen && serialPortName != string.Empty)
                    {
                        DeviceData = new DeviceData();

                        // receivedSize = 0;

                        lock (TaskSigintLockObject)
                        {
                            TaskSigint = false;
                            ClosedByThread = false;
                        }

                        ReceiveHandler = new Task(() =>
                        {
                            Receive();
                        });

                        SendHandler = new Task(() =>
                        {
                            Send();
                        });

                        SerialPort.DiscardInBuffer();
                        SerialPort.DiscardOutBuffer();

                        ReceiveHandler.Start();
                        SendHandler.Start();

                        // 消息回调
                        _ = (DeviceConnected?.BeginInvoke(serialPortName, null, null));

                        retValue = true;
                    }
                }
            }
            catch (Exception) { };

            return retValue;
        }

        public bool Disconnect()
        {
            bool retValue = false;

            try
            {
                if (IsConnected())
                {
                    lock (TaskSigintLockObject)
                    {
                        TaskSigint = true;
                    }

                    if (ReceiveHandler != null && SendHandler != null)
                    {
                        _ = ReceiveHandler.Wait(1000);
                        _ = SendHandler.Wait(1000);

                        SendHandler = null;
                        ReceiveHandler = null;
                    }

                    try
                    {
                        SerialPort.DiscardInBuffer();
                        SerialPort.DiscardOutBuffer();
                    }
                    catch (Exception) { };

                    try
                    {
                        SerialPort.Close();
                    }
                    catch (Exception) { };

                    // receivedSize = 0;

                    // 消息回调
                    if (!ClosedByThread)
                    {
                        _ = (DeviceDisconnected?.BeginInvoke(null, null));
                    }

                    retValue = true;
                }
            }
            catch (Exception) { };

            return retValue;
        }

        public bool IsConnected()
        {
            bool retValue = SerialPort.IsOpen;
            return retValue;
        }

        public int GetModel()
        {
            int retValue = 0x00;

            if (IsConnected())
            {
                retValue = Model;
            }

            return retValue;
        }

        public int GetVersion()
        {
            int retValue = 0x00;

            if (IsConnected())
            {
                retValue = Version;
            }

            return retValue;
        }

        public List<string> GetValueNames()
        {
            List<string> retValue = new List<string>();

            if (IsConnected())
            {
                for (int i = 0; i < Define.HALPorts.ValueDefineList.Count; i++)
                {
                    retValue.Add(((Define.HALPorts.ValueEnum)i).ToString());
                }
            }

            return retValue;
        }

        public List<string> GetValueNames(Define.HALPorts.ValueMode typeFilter)
        {
            List<string> retValue = new List<string>();

            if (IsConnected())
            {
                for (int i = 0; i < Define.HALPorts.ValueDefineList.Count; i++)
                {
                    if (Define.HALPorts.ValueDefineList[i].Item2 == typeFilter)
                    {
                        retValue.Add(((Define.HALPorts.ValueMode)i).ToString());
                    }
                }
            }

            return retValue;
        }

        public List<Tuple<string, Tuple<Define.HALPorts.ValueMode, object>>> GetValueTuples()
        {
            List<Tuple<string, Tuple<Define.HALPorts.ValueMode, object>>> retValue = new List<Tuple<string, Tuple<Define.HALPorts.ValueMode, object>>>();

            if (IsConnected())
            {
                retValue = DeviceData.GetValueTuples();
            }

            return retValue;
        }

        public Tuple<Define.HALPorts.ValueMode, object> GetValue(Define.HALPorts.ValueEnum io)
        {
            Tuple<Define.HALPorts.ValueMode, object> retValue = new Tuple<Define.HALPorts.ValueMode, object>(Define.HALPorts.ValueMode.DATA_MODE_UNDEFINED, new object());

            if (IsConnected())
            {
                retValue = DeviceData.Read(io);
            }

            return retValue;
        }

        public Tuple<Define.HALPorts.ValueMode, object> GetValue(string ioName)
        {
            Tuple<Define.HALPorts.ValueMode, object> retValue = new Tuple<Define.HALPorts.ValueMode, object>(Define.HALPorts.ValueMode.DATA_MODE_UNDEFINED, new object());

            try
            {
                retValue = GetValue(
                    (Define.HALPorts.ValueEnum)Enum.Parse(typeof(Define.HALPorts.ValueEnum), ioName));
            }
            catch (Exception) { };

            return retValue;
        }

        public bool SetValue(Define.HALPorts.ValueEnum io, object value, bool stringValue = true)
        {
            bool retValue = false;

            if (IsConnected())
            {
                retValue = DeviceData.Update(io, value, stringValue);
            }

            return retValue;
        }

        public bool SetValue(string ioName, object value, bool stringValue = true)
        {
            bool retValue = false;

            try
            {
                retValue = SetValue(
                    (Define.HALPorts.ValueEnum)Enum.Parse(typeof(Define.HALPorts.ValueEnum), ioName), value, stringValue);
            }
            catch (Exception) { };

            return retValue;
        }

        private void Receive()
        {
            while (true)
            {
                if (!IsConnected())
                {
                    lock (TaskSigintLockObject)
                    {
                        TaskSigint = true;

                        if (!ClosedByThread)
                        {
                            ClosedByThread = true;

                            _ = (DeviceDisconnected?.BeginInvoke(null, null));
                        }
                    }

                    break;
                }

                try
                {
                    Model = Define.DeviceInfo.Model;
                    Version = Define.DeviceInfo.Version;

                    // 暂时无可实现内容，若需要使用 HMI 对系统进行控制，在此实现数据获取线程。
                }
                catch (Exception) { };

                lock (TaskSigintLockObject)
                {
                    if (TaskSigint)
                    {
                        break;
                    }
                }

                Thread.Sleep((int)(1000.0 / Define.Communication.COMM_RECV_FREQ));
            }
        }

        private void Send()
        {
            while (true)
            {
                if (!IsConnected())
                {
                    lock (TaskSigintLockObject)
                    {
                        TaskSigint = true;

                        if (!ClosedByThread)
                        {
                            ClosedByThread = true;

                            _ = (DeviceDisconnected?.BeginInvoke(null, null));
                        }
                    }

                    break;
                }

                try
                {
                    List<Tuple<string, Tuple<Define.HALPorts.ValueMode, object>>> dataList = DeviceData.GetValueTuples();

                    for (int i = 0; i < dataList.Count; i++)
                    {
                        // Prefix 部分
                        StringBuilder commandToSend = new StringBuilder(Define.HALPorts.ValueDefineList[i].Item1);

                        // 数据部分
                        switch (dataList[i].Item2.Item1)
                        {
                            case Define.HALPorts.ValueMode.DATA_MODE_STRING:
                                {
                                    _ = commandToSend.Append('\"');
                                    _ = commandToSend.Append((string)dataList[i].Item2.Item2);
                                    _ = commandToSend.Append('\"');

                                    break;
                                }
                            default:
                                {
                                    _ = commandToSend.Append(dataList[i].Item2.Item2.ToString());

                                    break;
                                }
                        }

                        // 发送指令
                        byte[] dataToSend = Encoding.UTF8.GetBytes(commandToSend.ToString());

                        SerialPort.Write(dataToSend, 0, dataToSend.Length);

                        // 发送结束符
                        SerialPort.Write(Define.Communication.COMM_TAIL, 0, Define.Communication.COMM_TAIL.Length);

                        Thread.Sleep(Define.Communication.COMM_DELAY_PER_SEND);
                    }

                    _ = (DeviceSendDataUpdated?.BeginInvoke(dataList, null, null));
                }
                catch (Exception) { };

                lock (TaskSigintLockObject)
                {
                    if (TaskSigint)
                    {
                        break;
                    }
                }

                Thread.Sleep((int)(1000.0 / Define.Communication.COMM_SEND_FREQ));
            }
        }
    }
}
