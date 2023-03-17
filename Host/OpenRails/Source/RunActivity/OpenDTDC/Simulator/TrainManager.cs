using Orts;
using Orts.Formats.Msts;
using Orts.Simulation.RollingStocks;
using ORTS.Common;
using ORTS.Scripting.Api;
using System;

namespace OpenDTDC.Simulator
{
    class TrainManager : Interface.ITrainManager
    {
        static public TrainCar PlayerLocomotive;

        #region Setters
        static public void Update(TrainCar currentData)
        {
            try
            {
                PlayerLocomotive = currentData;
            }
            catch (Exception) { };
        }

        public void SetTrainPowerHandle(int dataValue)
        {
            try
            {
                if (dataValue >= 0 && dataValue <= 100)
                {
                    ((MSTSLocomotive)PlayerLocomotive).SetThrottlePercent(dataValue);
                }
            }
            catch (Exception) { };
        }

        public void SetTrainBrakeHandle(int dataValue)
        {
            try
            {
                if (dataValue >= 0 && dataValue <= 100)
                {
                    ((MSTSLocomotive)PlayerLocomotive).SetTrainBrakePercent(dataValue);
                }
            }
            catch (Exception) { };
        }

        public void SetDynamicBrakeHandle(int dataValue)
        {
            try
            {
                if (dataValue >= 0 && dataValue <= 100)
                {
                    ((MSTSLocomotive)PlayerLocomotive).SetDynamicBrakePercent(dataValue);
                }
            }
            catch (Exception) { };
        }

        public void SetEngineBrakeHandle(int dataValue)
        {
            try
            {
                if (dataValue >= 0 && dataValue <= 100)
                {
                    ((MSTSLocomotive)PlayerLocomotive).SetEngineBrakePercent(dataValue);
                }
            }
            catch (Exception) { };
        }

        public void SetHandBrakeHandle(int dataValue)
        {
            try
            {
                ((MSTSLocomotive)PlayerLocomotive).SetTrainHandbrake(Convert.ToBoolean(dataValue));
            }
            catch (Exception) { };
        }

        public void SetEmergency(int dataValue)
        {
            try
            {
                ((MSTSLocomotive)PlayerLocomotive).SetEmergency(Convert.ToBoolean(dataValue));
            }
            catch (Exception) { };
        }

        public void SetReverser(int dataValue)
        {
            try
            {
                switch (dataValue)
                {
                    case -1:
                        {
                            ((MSTSLocomotive)PlayerLocomotive).SetDirection(Direction.Reverse);
                            break;
                        }
                    case 0:
                        {
                            ((MSTSLocomotive)PlayerLocomotive).SetDirection(Direction.N);
                            break;
                        }
                    case 1:
                        {
                            ((MSTSLocomotive)PlayerLocomotive).SetDirection(Direction.Forward);
                            break;
                        }
                }
                return;
            }
            catch (Exception) { };
        }

        public void SetHornState(int dataValue)
        {
            try
            {
                dataValue = dataValue > 1 ? 1 : (dataValue < 0 ? 0 : dataValue);

                ((MSTSLocomotive)PlayerLocomotive).ManualHorn = Convert.ToBoolean(dataValue);

                if (dataValue > 0)
                {
                    ((MSTSLocomotive)PlayerLocomotive).AlerterReset(TCSEvent.HornActivated);
                    ((MSTSLocomotive)PlayerLocomotive).Simulator.HazzardManager.Horn();
                }
            }
            catch (Exception) { };
        }

        public void SetHeadlightState(int dataValue)
        {
            try
            {
                ((MSTSLocomotive)PlayerLocomotive).SignalEvent(
                    dataValue > 0 ? Orts.Common.Event._HeadlightOn : Orts.Common.Event._HeadlightOff);
            }
            catch (Exception) { };
        }

        public void SetPantoState(int dataValue)
        {
            try
            {
                if (GetPantoState() != dataValue)
                {
                    ((MSTSLocomotive)PlayerLocomotive).SignalEvent(
                        dataValue > 0 ? Orts.Common.Event.Pantograph1Up : Orts.Common.Event.Pantograph1Down);
                }
            }
            catch (Exception) { };
        }

        public void SetSanderState(int dataValue)
        {
            try
            {
                if (GetSanderState() != dataValue)
                {
                    ((MSTSLocomotive)PlayerLocomotive).SignalEvent(
                        dataValue > 0 ? Orts.Common.Event.SanderOn : Orts.Common.Event.SanderOff);
                }
            }
            catch (Exception) { };
        }

        public void SetDoorState(int dataValue)
        {
            try
            {
                if (GetDoorState() != dataValue)
                {
                    ((MSTSLocomotive)PlayerLocomotive).SignalEvent(
                        dataValue > 0 ? Orts.Common.Event.DoorOpen : Orts.Common.Event.DoorClose);
                }
            }
            catch (Exception) { };
        }
        #endregion

        #region Getters
        public int GetPowerHandle()
        {
            int retValue = 0;

            try
            {
                retValue = Convert.ToInt32(((MSTSLocomotive)PlayerLocomotive).ThrottlePercent);
            }
            catch (Exception) { };

            return retValue;
        }

        public int GetTrainBrakeHandle()
        {
            int retValue = 0;

            try
            {
                retValue = Convert.ToInt32(((MSTSLocomotive)PlayerLocomotive).TrainBrakeController.CurrentValue * 100.0);
            }
            catch (Exception) { };

            return retValue;
        }

        public int GetDynamicBrakeHandle()
        {
            int retValue = 0;

            try
            {
                retValue = Convert.ToInt32(((MSTSLocomotive)PlayerLocomotive).DynamicBrakeController.CurrentValue * 100.0);
            }
            catch (Exception) { };

            return retValue;
        }

        public int GetEngineBrakeHandle()
        {
            int retValue = 0;

            try
            {
                retValue = Convert.ToInt32(((MSTSLocomotive)PlayerLocomotive).EngineBrakeController.CurrentValue * 100.0);
            }
            catch (Exception) { };

            return retValue;
        }

        public int GetHandBrakeState()
        {
            int retValue = 0;

            try
            {
                retValue = Convert.ToInt32(((MSTSLocomotive)PlayerLocomotive).HandBrakePresent);
            }
            catch (Exception) { };

            return retValue;
        }

        public int GetEmergencyState()
        {
            int retValue = 0;

            try
            {
                CabViewControl virtualControl = new CabViewControl
                {
                    ControlType = CABViewControlTypes.EMERGENCY_BRAKE
                };

                retValue = (int)((MSTSLocomotive)PlayerLocomotive).GetDataOf(virtualControl);

                retValue = retValue < 0 ? 0 : (retValue > 1 ? 1 : retValue);
            }
            catch (Exception) { };

            return retValue;
        }

        public int GetReverserState()
        {
            int retValue = 0;

            try
            {
                switch (((MSTSLocomotive)PlayerLocomotive).Direction)
                {
                    case Direction.Reverse:
                        {
                            retValue = -1;

                            break;
                        }
                    case Direction.N:
                        {
                            retValue = 0;

                            break;
                        }
                    case Direction.Forward:
                        {
                            retValue = 1;

                            break;
                        }
                }
            }
            catch (Exception) { };

            return retValue;
        }

        public int GetHornState()
        {
            int retValue = 0;

            try
            {
                retValue = Convert.ToInt32(((MSTSLocomotive)PlayerLocomotive).ManualHorn);
            }
            catch (Exception) { };

            return retValue;
        }

        public int GetHeadlightState()
        {
            int retValue = 0;

            try
            {
                CabViewControl virtualControl = new CabViewControl
                {
                    ControlType = CABViewControlTypes.FRONT_HLIGHT
                };

                retValue = (int)((MSTSLocomotive)PlayerLocomotive).GetDataOf(virtualControl);

                retValue = retValue > 1 ? 1 : (retValue < 0 ? 0 : retValue);
            }
            catch (Exception) { };

            return retValue;
        }

        public int GetPantoState()
        {
            int retValue = 0;

            try
            {
                CabViewControl virtualControl = new CabViewControl
                {
                    ControlType = CABViewControlTypes.PANTOGRAPH
                };

                retValue = (int)((MSTSLocomotive)PlayerLocomotive).GetDataOf(virtualControl);

                retValue = retValue > 1 ? 1 : (retValue < 0 ? 0 : retValue);
            }
            catch (Exception) { };

            return retValue;
        }

        public int GetSanderState()
        {
            int retValue = 0;

            try
            {
                CabViewControl virtualControl = new CabViewControl
                {
                    ControlType = CABViewControlTypes.SANDING
                };

                retValue = (int)((MSTSLocomotive)PlayerLocomotive).GetDataOf(virtualControl);

                retValue = retValue > 1 ? 1 : (retValue < 0 ? 0 : retValue);
            }
            catch (Exception) { };

            return retValue;
        }

        public int GetDoorState()
        {
            int retValue = 0;

            try
            {
                CabViewControl virtualControl = new CabViewControl
                {
                    ControlType = CABViewControlTypes.DOORS_DISPLAY
                };

                retValue = (int)((MSTSLocomotive)PlayerLocomotive).GetDataOf(virtualControl);

                retValue = retValue > 1 ? 1 : (retValue < 0 ? 0 : retValue);
            }
            catch (Exception) { };

            return retValue;
        }

        public double GetSpeed()
        {
            double retValue = 0;

            try
            {
                retValue = ((MSTSLocomotive)PlayerLocomotive).SpeedMpS;
            }
            catch (Exception) { };

            return retValue;
        }

        public double GetSectionSpeedLimit()
        {
            double retValue = 30 / 3.6;

            try
            {
                retValue = PlayerLocomotive.Train.GetTrainInfo().allowedSpeedMpS;
            }
            catch (Exception) { };

            return retValue;
        }

        public int GetNextSectionSignal()
        {
            int retValue = 0;

            try
            {
                retValue = Convert.ToInt32(
                    PlayerLocomotive.Train.GetNextSignalAspect(
                        PlayerLocomotive.Train.GetTrainInfo().direction));
            }
            catch (Exception) { };

            return retValue;
        }

        public double GetNextSectionSignalDistance()
        {
            double retValue = 0;

            try
            {
                retValue = Convert.ToDouble(PlayerLocomotive.Train.DistanceToSignal);
            }
            catch (Exception) { };

            return retValue;
        }

        public double GetRouteGradient()
        {
            double retValue = 0;

            try
            {
                retValue = -1 * PlayerLocomotive.Train.GetTrainInfo().currentElevationPercent;
            }
            catch (Exception) { };

            return retValue;
        }

        public double GetVoltage()
        {
            double retValue = 0;

            try
            {
                CabViewControl virtualControl = new CabViewControl
                {
                    ControlType = CABViewControlTypes.LINE_VOLTAGE
                };

                retValue = (double)((MSTSLocomotive)PlayerLocomotive).GetDataOf(virtualControl);
            }
            catch (Exception) { };

            return retValue;
        }

        public double GetCurrent()
        {
            double retValue = 0;

            try
            {
                CabViewControl virtualControl = new CabViewControl
                {
                    ControlType = CABViewControlTypes.AMMETER_ABS
                };

                retValue = (double)((MSTSLocomotive)PlayerLocomotive).GetDataOf(virtualControl);
            }
            catch (Exception) { };

            return retValue;
        }

        public string GetRouteName()
        {
            string retValue = string.Empty;

            try
            {
                retValue = PlayerLocomotive.Simulator.RouteName;
            }
            catch (Exception) { };

            return retValue;
        }

        public string GetTrainName()
        {
            string retValue = string.Empty; 

            try
            {
                retValue = PlayerLocomotive.Train.Name;
            }
            catch (Exception) { };

            return retValue;
        }

        public string GetConsistName()
        {
            string retValue = string.Empty;

            try
            {
                retValue = PlayerLocomotive.Simulator.conFileName;
            }
            catch (Exception) { };

            return retValue;
        }

        public string GetActivityName()
        {
            string retValue = string.Empty;

            try
            {
                retValue = Program.Viewer.NextStationWindow.ActMessage;
            }
            catch (Exception) { };

            return retValue;
        }

        public string GetTime()
        {
            string retValue = string.Empty;

            try
            {
                int seconds = (int)(Program.Simulator.ClockTime % 60);
                int minutes = (int)((Program.Simulator.ClockTime / 60.0) % 60);
                int hours = (int)((Program.Simulator.ClockTime / 3600.0) % 60);

                retValue = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
            }
            catch (Exception) { };

            return retValue;
        }

        #endregion
    }
}
