﻿using System;
using System.Collections.Generic;

namespace OpenDTDC.HMI
{
    public class DeviceData
    {
        private readonly object ValueUpdateLockObject = new object();

        public Dictionary<Define.HALPorts.IOEnum, object> Values;

        public DeviceData()
        {
            Values = new Dictionary<Define.HALPorts.IOEnum, object>();

            Array enumValues = Enum.GetValues(new Define.HALPorts.IOEnum().GetType());

            foreach (object value in enumValues)
            {
                switch (Define.HALPorts.ValueDefineList[(int)value].Item2)
                {
                    case Define.HALPorts.IOMode.DATA_MODE_BOOL:
                    case Define.HALPorts.IOMode.DATA_MODE_DOUBLE:
                    case Define.HALPorts.IOMode.DATA_MODE_INT:
                        {
                            Values.Add((Define.HALPorts.IOEnum)value, 0);

                            break;
                        }
                    case Define.HALPorts.IOMode.DATA_MODE_STRING:
                        {
                            Values.Add((Define.HALPorts.IOEnum)value, string.Empty);

                            break;
                        }
                    default:
                        {
                            Values.Add((Define.HALPorts.IOEnum)value, new object());

                            break;
                        }
                };
            }

            // 连接保持符置1
            Values[Define.HALPorts.IOEnum.HEART_BEAT] = 1;
        }

        public bool Update(Define.HALPorts.IOEnum io, object value, bool stringValue = true)
        {
            bool retValue = false;

            lock (ValueUpdateLockObject)
            {
                try
                {
                    switch (Define.HALPorts.ValueDefineList[(int)io].Item2)
                    {
                        case Define.HALPorts.IOMode.DATA_MODE_BOOL:
                            {
                                int data = stringValue ? int.Parse(value.ToString()) : (int)value;

                                data = data > 1 ? 1 : (data < 0 ? 0 : data);

                                Values[io] = data;

                                break;
                            }
                        case Define.HALPorts.IOMode.DATA_MODE_DOUBLE:
                            {
                                Values[io] = stringValue ? double.Parse(value.ToString()) : (double)value;

                                break;
                            }
                        case Define.HALPorts.IOMode.DATA_MODE_INT:
                            {
                                Values[io] = stringValue ? int.Parse(value.ToString()) : (int)value;

                                break;
                            }
                        case Define.HALPorts.IOMode.DATA_MODE_STRING:
                            {
                                Values[io] = stringValue ? (string)value : value.ToString();

                                break;
                            }
                    };

                    retValue = true;
                }
                catch (Exception) { };
            }

            return retValue;
        }

        public Tuple<Define.HALPorts.IOMode, object> Read(Define.HALPorts.IOEnum io, bool lockRead = true)
        {
            Tuple<Define.HALPorts.IOMode, object> retValue = new Tuple<Define.HALPorts.IOMode, object>(Define.HALPorts.IOMode.DATA_MODE_UNDEFINED, new object());

            if (lockRead)
            {
                lock (ValueUpdateLockObject)
                {
                    try
                    {
                        retValue = new Tuple<Define.HALPorts.IOMode, object>(Define.HALPorts.ValueDefineList[(int)io].Item2, Values[io]);
                    }
                    catch (Exception) { };
                }
            }
            else
            {
                try
                {
                    retValue = new Tuple<Define.HALPorts.IOMode, object>(Define.HALPorts.ValueDefineList[(int)io].Item2, Values[io]);
                }
                catch (Exception) { };
            }

            return retValue;
        }

        public List<Tuple<string, Tuple<Define.HALPorts.IOMode, object>>> GetValueTuples()
        {
            List<Tuple<string, Tuple<Define.HALPorts.IOMode, object>>> retValue = new List<Tuple<string, Tuple<Define.HALPorts.IOMode, object>>>();

            try
            {
                Array enumValues = Enum.GetValues(new Define.HALPorts.IOEnum().GetType());

                lock (ValueUpdateLockObject)
                {
                    foreach (object value in enumValues)
                    {
                        Tuple<string, Tuple<Define.HALPorts.IOMode, object>> tuple = new Tuple<string, Tuple<Define.HALPorts.IOMode, object>>(
                            ((Define.HALPorts.IOEnum)value).ToString(), Read((Define.HALPorts.IOEnum)value, false));

                        retValue.Add(tuple);
                    }
                }
            }
            catch (Exception) { };

            return retValue;
        }
    }
}
