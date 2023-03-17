using OpenDTDC.Interface;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenDTDC.Controller
{
    public class ControllerInstance : IHardware<Define.HALPorts.IOMode, Define.HALPorts.IOEnum, int>
    {
        private byte Model;
        private byte Version;

        private readonly SerialPort SerialPort = new SerialPort();

        private readonly byte[] ReceiveBuffer = new byte[Define.Communication.COMM_RECV_BUFFER_SIZE];
        private readonly byte[] SendBuffer = new byte[Define.Communication.COMM_SEND_BUFFER_SIZE];

        private int receivedSize = 0;

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
        public delegate void DeviceReceivedDataUpdatedHandler(List<Tuple<string, int>> dataList);
        // public delegate void DeviceSendDataUpdatedHandler(List<Tuple<string, int>> dataList);

        public event DeviceReceivedDataUpdatedHandler DeviceReceivedDataUpdated;
        // public event DeviceSendDataUpdatedHandler DeviceSendDataUpdated;

        public bool Connect(string connectionString = "")
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
                    if (connectionString == string.Empty)
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

                                Thread.Sleep(100);

                                string response = SerialPort.ReadExisting();

                                if (response.IndexOf(Define.Communication.COMM_CONNECT_RESPONSE) != -1)
                                {
                                    SerialPort.Write(Define.Communication.COMM_START_TRANS_REQUEST);

                                    connectionString = serialPort;

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
                        SerialPort.PortName = connectionString;
                        SerialPort.ReadTimeout = Define.Communication.COMM_TIMEOUT;
                        SerialPort.WriteTimeout = Define.Communication.COMM_TIMEOUT;

                        SerialPort.ReadBufferSize = Define.Communication.COMM_RECV_BUFFER_SIZE;
                        SerialPort.WriteBufferSize = Define.Communication.COMM_SEND_BUFFER_SIZE;

                        SerialPort.Open();

                        Thread.Sleep(100);

                        SerialPort.Write(Define.Communication.COMM_CONNECT_REQUEST);

                        Thread.Sleep(100);

                        string response = SerialPort.ReadExisting();

                        if (response.IndexOf(Define.Communication.COMM_CONNECT_RESPONSE) != -1)
                        {
                            SerialPort.Write(Define.Communication.COMM_START_TRANS_REQUEST);
                        }
                        else
                        {
                            SerialPort.Close();
                        }
                    }

                    if (SerialPort.IsOpen && connectionString != string.Empty)
                    {
                        DeviceData = new DeviceData();

                        receivedSize = 0;

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
                        _ = (DeviceConnected?.BeginInvoke(connectionString, null, null));

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

                    receivedSize = 0;

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

        public List<string> GetIONames()
        {
            List<string> retValue = new List<string>();

            if (IsConnected())
            {
                for (int i = 0; i < Define.HALPorts.IOModeList.Count; i++)
                {
                    retValue.Add(((Define.HALPorts.IOEnum)i).ToString());
                }
            }

            return retValue;
        }

        public List<string> GetIONames(Define.HALPorts.IOMode typeFilter)
        {
            List<string> retValue = new List<string>();

            if (IsConnected())
            {
                for (int i = 0; i < Define.HALPorts.IOModeList.Count; i++)
                {
                    if (Define.HALPorts.IOModeList[i] == typeFilter)
                    {
                        retValue.Add(((Define.HALPorts.IOEnum)i).ToString());
                    }
                }
            }

            return retValue;
        }

        public List<Tuple<string, int>> GetValueTuples()
        {
            List<Tuple<string, int>> retValue = new List<Tuple<string, int>>();

            if (IsConnected())
            {
                retValue = DeviceData.GetValueTuples();
            }

            return retValue;
        }

        public int GetValue(Define.HALPorts.IOEnum io)
        {
            int retValue = 0x00;

            if (IsConnected())
            {
                retValue = DeviceData.Read(io);
            }

            return retValue;
        }

        public int GetValue(string ioName)
        {
            int retValue = 0x00;

            try
            {
                retValue = GetValue(
                    (Define.HALPorts.IOEnum)Enum.Parse(typeof(Define.HALPorts.IOEnum), ioName));
            }
            catch (Exception) { };

            return retValue;
        }

        public bool SetValue(Define.HALPorts.IOEnum io, object value, bool stringValue = false)
        {
            bool retValue = false;

            if (IsConnected())
            {
                int intValue = int.Parse(value.ToString());

                intValue = intValue > 100 ? 100 : (intValue < 0 ? 0 : intValue);

                retValue = DeviceData.Update(io, intValue);
            }

            return retValue;
        }

        public bool SetValue(string ioName, object value, bool stringValue = false)
        {
            bool retValue = false;

            try
            {
                retValue = SetValue(
                    (Define.HALPorts.IOEnum)Enum.Parse(typeof(Define.HALPorts.IOEnum), ioName), value);
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
                    int currentSize = SerialPort.BytesToRead;

                    if (receivedSize + currentSize > Define.Communication.COMM_SEND_BUFFER_SIZE)
                    {
                        receivedSize = 0;

                        SerialPort.DiscardInBuffer();
                    }

                    if (currentSize > 0)
                    {
                        int packetStartIndex = 0;
                        int packetEndIndex = 0;

                        bool findPacketStart = false;
                        bool findPacketEnd = false;

                        for (int i = 0; i < currentSize; i++)
                        {
                            ReceiveBuffer[receivedSize + i] = (byte)SerialPort.ReadByte();
                        }

                        receivedSize += currentSize;

                        for (int i = receivedSize - 1; i >= 0; i--)
                        {
                            if (ReceiveBuffer[i] == Define.Communication.COMM_TAIL)
                            {
                                packetEndIndex = i;

                                findPacketEnd = true;

                                break;
                            }
                        }

                        if (findPacketEnd)
                        {
                            for (int i = packetEndIndex - 1; i >= 0; i--)
                            {
                                if (ReceiveBuffer[i] == Define.Communication.COMM_HEAD)
                                {
                                    packetStartIndex = i;

                                    findPacketStart = true;

                                    break;
                                }
                            }
                        }

                        if ((findPacketStart & findPacketEnd) &&
                            packetEndIndex - packetStartIndex + 1 >= Define.Communication.COMM_PACK_MIN_SIZE &&
                            (packetEndIndex - packetStartIndex + 1 - Define.Communication.COMM_PACK_MIN_SIZE) == ReceiveBuffer[packetStartIndex + Define.Communication.COMM_NUM_POS])
                        {
                            Model = ReceiveBuffer[packetStartIndex + Define.Communication.COMM_TYPE_POS];
                            Version = ReceiveBuffer[packetStartIndex + Define.Communication.COMM_VER_POS];

                            for (int i = packetStartIndex + Define.Communication.COMM_NUM_POS + 1; i < packetEndIndex; i++)
                            {
                                if (ReceiveBuffer[i] != Define.Communication.COMM_NODATA)
                                {
                                    Define.HALPorts.IOEnum ioAddr =
                                        (Define.HALPorts.IOEnum)(i - (packetStartIndex + Define.Communication.COMM_NUM_POS + 1));

                                    _ = SetValue(ioAddr, ReceiveBuffer[i]);
                                }
                            }

                            receivedSize = 0;

                            // 消息事件
                            new Task(() =>
                            {
                                List<Tuple<string, int>> dataList = GetValueTuples();

                                DeviceReceivedDataUpdated?.Invoke(dataList);
                            }).Start();
                        }
                    }
                }
                catch (Exception){ };

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
                    int size = 0;

                    SendBuffer[Define.Communication.COMM_HEAD_POS] = Define.Communication.COMM_HEAD;
                    SendBuffer[Define.Communication.COMM_TYPE_POS] = Define.DeviceInfo.Model;
                    SendBuffer[Define.Communication.COMM_VER_POS] = Define.DeviceInfo.Version;

                    size += 4;

                    List<Tuple<string, int>> dataList = GetValueTuples();

                    for (int i = 0; i < Define.HALPorts.IOModeList.Count; i++)
                    {
                        SendBuffer[size] = Define.Communication.COMM_NODATA;

                        if (Define.HALPorts.IOModeList[i] == Define.HALPorts.IOMode.DEV_MODE_OUTPUT ||
                            Define.HALPorts.IOModeList[i] == Define.HALPorts.IOMode.DEV_MODE_OUTPUT_ANALOG)
                        {
                            SendBuffer[size] = (byte)dataList[i].Item2;
                        }

                        size++;
                    }

                    SendBuffer[Define.Communication.COMM_NUM_POS] = (byte)(size - 4);

                    SendBuffer[size++] = Define.Communication.COMM_TAIL;

                    SerialPort.Write(SendBuffer, 0, size);
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
