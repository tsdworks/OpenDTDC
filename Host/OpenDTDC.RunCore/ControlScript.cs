using OpenDTDC.Controller;
using OpenDTDC.HMI;
using OpenDTDC.Interface;
using System;
using System.Collections.Generic;

namespace OpenDTDC.RunCore
{
    public class ControlScript : IControlScript
    {
        // 定义模拟器操作接口实例
        public ITrainManager TrainManager = null;

        // 定义驾驶台实例
        public ControllerInstance Controller = null;

        // 定义 HMI 实例
        public HMIInstance HMI = null;

        public ControlScript(ITrainManager train, ControllerInstance controller, HMIInstance HMI)
        {
            TrainManager = train;
            Controller = controller;
            this.HMI = HMI;
        }

        public void Update(long time, List<Tuple<string, int>> dataList)
        {
            // 简单的脚本样例
            // 存储临时变量
            int reverserIdle = 1;

            // 进行控制量映射
            foreach (Tuple<string, int> data in dataList)
            {
                switch ((Controller.Define.HALPorts.IOEnum)Enum.Parse(typeof(Controller.Define.HALPorts.IOEnum), data.Item1))
                {
                    // 受电弓
                    case OpenDTDC.Controller.Define.HALPorts.IOEnum.SDS6:
                        {
                            TrainManager.SetPantoState(data.Item2);
                            break;
                        }
                    // 撒沙
                    case OpenDTDC.Controller.Define.HALPorts.IOEnum.SDS7:
                        {
                            TrainManager.SetSanderState(data.Item2);
                            break;
                        }
                    // 头灯
                    case OpenDTDC.Controller.Define.HALPorts.IOEnum.SDS8:
                        {
                            TrainManager.SetHeadlightState(data.Item2);
                            break;
                        }
                    // 预留
                    case OpenDTDC.Controller.Define.HALPorts.IOEnum.SDS9:
                        {
                            // 预留
                            break;
                        }
                    // 汽笛
                    case OpenDTDC.Controller.Define.HALPorts.IOEnum.SDS10:
                        {
                            TrainManager.SetHornState(data.Item2);
                            break;
                        }
                    // 紧急制动
                    case OpenDTDC.Controller.Define.HALPorts.IOEnum.SDS11:
                        {
                            TrainManager.SetEmergency(data.Item2);
                            break;
                        }
                    // 换向器
                    case OpenDTDC.Controller.Define.HALPorts.IOEnum.DDS01:
                        {
                            if (data.Item2 > 0)
                            {
                                TrainManager.SetReverser(1);

                                reverserIdle = 0;
                            }

                            break;
                        }
                    case OpenDTDC.Controller.Define.HALPorts.IOEnum.DDS02:
                        {
                            if (data.Item2 > 0)
                            {
                                TrainManager.SetReverser(-1);

                                reverserIdle = 0;
                            }

                            break;
                        }
                    // 手柄
                    case OpenDTDC.Controller.Define.HALPorts.IOEnum.ANA0:
                        {
                            // 换算关系
                            int value = (int)((data.Item2 - 52) * (100.0 / 16));

                            value = value > 100 ? 100 : (value < -100 ? -100 : value);

                            // 功率位
                            if (value > 0)
                            {
                                TrainManager.SetTrainBrakeHandle(0);
                                TrainManager.SetTrainPowerHandle(value);
                            }
                            else if (value < 0)
                            {
                                TrainManager.SetTrainBrakeHandle(-1 * value);
                                TrainManager.SetTrainPowerHandle(0);
                            }
                            else
                            {
                                TrainManager.SetTrainBrakeHandle(0);
                                TrainManager.SetTrainPowerHandle(0);
                            }

                            break;
                        }
                }
            }

            // 换向器中立位
            if (reverserIdle > 0)
            {
                TrainManager.SetReverser(0);
            }

            // 进行数据输出
            // 换向器
            _ = HMI.SetValue(OpenDTDC.HMI.Define.HALPorts.IOEnum.REVERSER, TrainManager.GetReverserState(), false);

            // 功率手柄
            _ = HMI.SetValue(OpenDTDC.HMI.Define.HALPorts.IOEnum.THROTTLE, TrainManager.GetPowerHandle(), false);

            // 制动手柄
            _ = HMI.SetValue(OpenDTDC.HMI.Define.HALPorts.IOEnum.BRAKE, TrainManager.GetTrainBrakeHandle(), false);

            // 当前速度
            _ = HMI.SetValue(OpenDTDC.HMI.Define.HALPorts.IOEnum.SPEED, Math.Abs(TrainManager.GetSpeed() * 3.6).ToString("f1"), false);

            // 当前限速
            _ = HMI.SetValue(OpenDTDC.HMI.Define.HALPorts.IOEnum.SPEED_LIMIT, (int)Math.Abs(TrainManager.GetSectionSpeedLimit() * 3.6), false);

            // 当前时间
            _ = HMI.SetValue(OpenDTDC.HMI.Define.HALPorts.IOEnum.TIME, TrainManager.GetTime(), false);

            // 当前受电弓
            _ = HMI.SetValue(OpenDTDC.HMI.Define.HALPorts.IOEnum.PANTO, TrainManager.GetPantoState(), false);

            // 当前电压
            _ = HMI.SetValue(OpenDTDC.HMI.Define.HALPorts.IOEnum.VOLTAGE, (int)TrainManager.GetVoltage(), false);

            // 当前电流
            _ = HMI.SetValue(OpenDTDC.HMI.Define.HALPorts.IOEnum.CURRENT, (int)TrainManager.GetCurrent(), false);

            // 当前车号
            _ = HMI.SetValue(OpenDTDC.HMI.Define.HALPorts.IOEnum.LOCO_NO, TrainManager.GetTrainName(), false);

            // 当前信号
            _ = HMI.SetValue(OpenDTDC.HMI.Define.HALPorts.IOEnum.SIGCODE, TrainManager.GetNextSectionSignal(), false);

            // 是否超速
            _ = HMI.SetValue(OpenDTDC.HMI.Define.HALPorts.IOEnum.OVERSPEED, Math.Abs(TrainManager.GetSpeed()) > Math.Abs(TrainManager.GetSectionSpeedLimit()) ? 1 : 0, false);

            // 风笛状态
            _ = HMI.SetValue(OpenDTDC.HMI.Define.HALPorts.IOEnum.HORN, TrainManager.GetHornState(), false);

            // 撒沙状态
            _ = HMI.SetValue(OpenDTDC.HMI.Define.HALPorts.IOEnum.SANDER, TrainManager.GetSanderState(), false);

            // 头灯状态
            _ = HMI.SetValue(OpenDTDC.HMI.Define.HALPorts.IOEnum.HEADLIGHT, TrainManager.GetHeadlightState(), false);

            // 信号机
            switch (TrainManager.GetNextSectionSignal())
            {
                // 白灯
                case 0:
                    {
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L0, 0);
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L1, 0);
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L2, 0);
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L3, 0);
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L4, 1);

                        break;
                    }
                // 红黄灯
                case 3:
                    {
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L0, 0);
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L1, 0);
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L2, 1);
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L3, 1);
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L4, 0);

                        break;
                    }
                // 黄灯
                case 5:
                    {
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L0, 0);
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L1, 0);
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L2, 1);
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L3, 0);
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L4, 0);

                        break;
                    }
                // 绿灯
                case 7:
                    {
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L0, 1);
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L1, 0);
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L2, 0);
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L3, 0);
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L4, 0);

                        break;
                    }
                default:
                    {
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L0, 0);
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L1, 0);
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L2, 0);
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L3, 0);
                        _ = Controller.SetValue(OpenDTDC.Controller.Define.HALPorts.IOEnum.L4, 0);

                        break;
                    }
            }
        }
    }
}
