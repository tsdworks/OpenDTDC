using OpenDTDC.Controller;
using OpenDTDC.HMI;
using OpenDTDCHost.Properties;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;

namespace OpenDTDCHost
{
    public partial class FormMain : Form
    {
        private readonly ControllerInstance Controller = new ControllerInstance();
        private readonly HMIInstance HMI = new HMIInstance();

        private void ActionRegisterEvents()
        {
            // 控制器
            // 添加事件监听
            Controller.DeviceConnected += portName =>
            {
                _ = Invoke(new Action(() =>
                {
                    buttonControllerCOMRefresh.Enabled = false;
                    buttonControllerConnect.Enabled = false;
                    buttonControllerDisconnect.Enabled = true;
                    labelControllerDeviceInfo.Text = string.Format(Resources.DeviceInfo, portName, Controller.GetModel(), Controller.GetVersion());

                    comboBoxControllerDeviceIO.Items.Clear();

                    comboBoxControllerDeviceIO.Items.AddRange(Controller.GetIONames(OpenDTDC.Controller.Define.HALPorts.IOMode.DEV_MODE_OUTPUT).ToArray());
                    comboBoxControllerDeviceIO.Items.AddRange(Controller.GetIONames(OpenDTDC.Controller.Define.HALPorts.IOMode.DEV_MODE_OUTPUT_ANALOG).ToArray());
                }));
            };

            Controller.DeviceDisconnected += () =>
            {
                _ = Invoke(new Action(() =>
                {
                    buttonControllerCOMRefresh.Enabled = true;
                    buttonControllerConnect.Enabled = true;
                    buttonControllerDisconnect.Enabled = false;
                    labelControllerDeviceInfo.Text = Resources.DeviceInfoNotConnected;
                }));
            };

            Controller.DeviceReceivedDataUpdated += dataList =>
            {
                _ = Invoke(new Action(() =>
                {
                    ActionControllerUpdateListView(dataList);
                }));
            };


            // HMI
            // 添加事件监听
            HMI.DeviceConnected += portName =>
            {
                _ = Invoke(new Action(() =>
                {
                    buttonHMICOMRefresh.Enabled = false;
                    buttonHMIConnect.Enabled = false;
                    buttonHMIDisconnect.Enabled = true;
                    labelHMIDeviceInfo.Text = string.Format(Resources.DeviceInfo, portName, HMI.GetModel(), HMI.GetVersion());

                    comboBoxHMIDeviceIO.Items.Clear();

                    comboBoxHMIDeviceIO.Items.AddRange(HMI.GetIONames().ToArray());
                }));
            };

            HMI.DeviceDisconnected += () =>
            {
                _ = Invoke(new Action(() =>
                {
                    buttonHMICOMRefresh.Enabled = true;
                    buttonHMIConnect.Enabled = true;
                    buttonHMIDisconnect.Enabled = false;
                    labelHMIDeviceInfo.Text = Resources.DeviceInfoNotConnected;
                }));
            };

            HMI.DeviceSendDataUpdated += dataList =>
            {
                _ = Invoke(new Action(() =>
                {
                    ActionHMIUpdateListView(dataList);
                }));
            };
        }

        #region Controller
        private void ActionRefreshControllerCOMList()
        {
            _ = comboBoxControllerCOM.Invoke(new Action(() =>
            {
                comboBoxControllerCOM.Items.Clear();

                comboBoxControllerCOM.Items.AddRange(SerialPort.GetPortNames());
            }));
        }

        private bool ActionControllerConnect()
        {
            return Controller.Connect(comboBoxControllerCOM.Text);
        }

        private bool ActionControllerDisconnect()
        {
            return Controller.Disconnect();
        }

        private bool ActionControllerWrite(string ioName, int value)
        {
            return Controller.SetValue(ioName, value);
        }

        private void ActionControllerUpdateListView(List<Tuple<string, int>> dataList)
        {
            try
            {
                if (Controller.IsConnected())
                {
                    foreach (Tuple<string, int> value in dataList)
                    {
                        int index = -1;

                        for (int i = 0; i < listViewControllerData.Items.Count; i++)
                        {
                            if (listViewControllerData.Items[i].Text == value.Item1)
                            {
                                index = i;

                                break;
                            }
                        }

                        if (index == -1)
                        {
                            ListViewItem listViewItem = new ListViewItem
                            {
                                Text = value.Item1
                            };
                            _ = listViewItem.SubItems.Add(value.Item2.ToString());

                            _ = listViewControllerData.Items.Add(listViewItem);
                        }
                        else
                        {
                            listViewControllerData.Items[index].SubItems[1].Text = value.Item2.ToString();
                        }
                    }
                }
            }
            catch (Exception) { };
        }
        #endregion

        #region HMI
        private void ActionRefreshHMICOMList()
        {
            _ = comboBoxHMICOM.Invoke(new Action(() =>
            {
                comboBoxHMICOM.Items.Clear();

                comboBoxHMICOM.Items.AddRange(SerialPort.GetPortNames());
            }));
        }

        private bool ActionHMIConnect()
        {
            return HMI.Connect(comboBoxHMICOM.Text);
        }

        private bool ActionHMIDisconnect()
        {
            return HMI.Disconnect();
        }

        private bool ActionHMIWrite(string ioName, object value)
        {
            return HMI.SetValue(ioName, value);
        }

        private void ActionHMIUpdateListView(List<Tuple<string, Tuple<OpenDTDC.HMI.Define.HALPorts.IOMode, object>>> dataList)
        {
            try
            {
                if (HMI.IsConnected())
                {
                    foreach (Tuple<string, Tuple<OpenDTDC.HMI.Define.HALPorts.IOMode, object>> value in dataList)
                    {
                        int index = -1;

                        for (int i = 0; i < listViewHMIData.Items.Count; i++)
                        {
                            if (listViewHMIData.Items[i].Text == value.Item1)
                            {
                                index = i;

                                break;
                            }
                        }

                        if (index == -1)
                        {
                            ListViewItem listViewItem = new ListViewItem
                            {
                                Text = value.Item1
                            };
                            _ = listViewItem.SubItems.Add(value.Item2.Item2.ToString());

                            _ = listViewHMIData.Items.Add(listViewItem);
                        }
                        else
                        {
                            listViewHMIData.Items[index].SubItems[1].Text = value.Item2.Item2.ToString();
                        }
                    }
                }
            }
            catch (Exception) { };
        }
        #endregion
    }
}
