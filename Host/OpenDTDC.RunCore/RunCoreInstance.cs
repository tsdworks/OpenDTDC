using OpenDTDC.Controller;
using OpenDTDC.HMI;
using OpenDTDC.Interface;
using OpenDTDC.RunCore.UserInterface;
using System;
using System.Threading;

namespace OpenDTDC.RunCore
{
    public class RunCoreInstance : IRunCore
    {
        // 定义模拟器操作接口实例
        public ITrainManager TrainManager = null;

        // 定义硬件设备枚举
        public enum DeviceEnum
        {
            CONTROLLER = 0,
            HMI = 1
        };

        // 定义驾驶台实例
        private readonly ControllerInstance Controller = new ControllerInstance();

        // 定义 HMI 实例
        private readonly HMIInstance HMI = new HMIInstance();

        // 定义控制脚本实例
        public IControlScript ScriptComponent;

        // 定义监视窗口实例
        public FormMonitor FormMonitor = new FormMonitor();

        // 硬件设备连接相关事件
        public delegate void DisconnectedHandler(DeviceEnum device);

        public event DisconnectedHandler Disconnected;

        public RunCoreInstance(ITrainManager trainManager)
        {
            TrainManager = trainManager;
        }

        public bool Initialize()
        {
            bool retValue = false;

            try
            {
                // 初始化控制脚本
                ScriptComponent = new ControlScript(TrainManager, Controller, HMI);

                // 注册事件
                Controller.DeviceDisconnected += () =>
                {
                    _ = (Disconnected?.BeginInvoke(DeviceEnum.CONTROLLER, null, null));

                    FormMonitor.ActionUpdateConnectionState(false);
                };

                HMI.DeviceDisconnected += () =>
                {
                    _ = (Disconnected?.BeginInvoke(DeviceEnum.HMI, null, null));

                    FormMonitor.ActionUpdateConnectionState(false);
                };

                Controller.DeviceReceivedDataUpdated += (dataList) =>
                {
                    // 首先，判断资源是否可用
                    if (TrainManager == null ||
                        Controller == null || !Controller.IsConnected() ||
                        HMI == null || !HMI.IsConnected())
                    {
                        return;
                    }
                    else
                    {
                        ScriptComponent.Update(DateTime.Now.Ticks, dataList);
                    }
                };

                FormMonitor.ConnectDevices += ConnectDevices;
                FormMonitor.DisconnectDevices += DisconnectDevices;

                // 启动监视窗体
                FormMonitor.Show();

                retValue = true;
            }
            catch (Exception) { };

            return retValue;
        }

        public bool ConnectDevices(object connectParams)
        {
            bool retValue = false;

            try
            {
                int retryCounter = 0;

                // 连接驾驶台
                if (!Controller.IsConnected())
                {
                    do
                    {
                        bool result = Controller.Connect();

                        if (!result)
                        {
                            retryCounter++;

                            if (retryCounter >= 6)
                            {
                                retValue = false;

                                break;
                            }

                            Thread.Sleep(500);
                        }
                        else
                        {
                            retValue = true;

                            break;
                        }
                    } while (true);
                }
                else
                {
                    retValue = true;
                }

                // 连接 HMI
                retryCounter = 0;

                if (retValue)
                {
                    if (!HMI.IsConnected())
                    {
                        do
                        {
                            bool result = HMI.Connect();

                            if (!result)
                            {
                                retryCounter++;

                                if (retryCounter >= 6)
                                {
                                    retValue = false;

                                    break;
                                }

                                Thread.Sleep(500);
                            }
                            else
                            {
                                retValue = true;

                                break;
                            }
                        } while (true);
                    }
                    else
                    {
                        retValue = true;
                    }
                }

                if (!retValue)
                {
                    _ = Controller.Disconnect();
                    _ = HMI.Disconnect();
                }
            }
            catch (Exception) { };

            return retValue;
        }

        public bool DisconnectDevices()
        {
            bool retValue = false;

            try
            {
                _ = Controller.Disconnect();
                _ = HMI.Disconnect();

                retValue = true;
            }
            catch (Exception) { };

            return retValue;
        }
    }
}
