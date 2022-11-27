using System;
using System.Collections.Generic;

namespace OpenDTDC.Controller
{
    public class DeviceData
    {
        private readonly object ValueUpdateLockObject = new object();

        public Dictionary<Define.HALPorts.IOEnum, int> Values;

        public DeviceData()
        {
            Values = new Dictionary<Define.HALPorts.IOEnum, int>();

            Array enumValues = Enum.GetValues(new Define.HALPorts.IOEnum().GetType());

            foreach (object value in enumValues)
            {
                Values.Add((Define.HALPorts.IOEnum)value, 0x00);
            }
        }

        public bool Update(Define.HALPorts.IOEnum io, int value)
        {
            bool retValue = false;

            lock (ValueUpdateLockObject)
            {
                try
                {
                    Values[io] = value;

                    retValue = true;
                }
                catch (Exception) { };
            }

            return retValue;
        }

        public int Read(Define.HALPorts.IOEnum io, bool lockRead = true)
        {
            int retValue = 0x00;

            if (lockRead)
            {
                lock (ValueUpdateLockObject)
                {
                    try
                    {
                        retValue = Values[io];
                    }
                    catch (Exception) { };
                }
            }
            else
            {
                try
                {
                    retValue = Values[io];
                }
                catch (Exception) { };
            }

            return retValue;
        }

        public List<Tuple<string, int>> GetValueTuples()
        {
            List<Tuple<string, int>> retValue = new List<Tuple<string, int>>();

            try
            {
                Array enumValues = Enum.GetValues(new Define.HALPorts.IOEnum().GetType());

                lock (ValueUpdateLockObject)
                {
                    foreach (object value in enumValues)
                    {
                        Tuple<string, int> tuple = new Tuple<string, int>(
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
