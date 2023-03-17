using System;
using System.Collections.Generic;

namespace OpenDTDC.HMI
{
    public class Define
    {
        public class DeviceInfo
        {
            public static byte Model = 0x00;
            public static byte Version = 0x01;
        };

        public class HALPorts
        {
            public enum IOMode
            {
                DATA_MODE_DOUBLE = 0,
                DATA_MODE_INT = 1,
                DATA_MODE_BOOL = 2,
                DATA_MODE_STRING = 3,
                DATA_MODE_UNDEFINED = 4
            };

            public enum IOEnum
            {
                REVERSER = 0,
                THROTTLE = 1,
                BRAKE = 2,
                SPEED = 3,
                SPEED_LIMIT = 4,
                TIME = 5,
                PANTO = 6,
                VOLTAGE = 7,
                CURRENT = 8,
                LOCO_NO = 9,
                SIGCODE = 10,
                OVERSPEED = 11,
                HORN = 12,
                SANDER = 13,
                HEADLIGHT = 14,
                DOOR = 15,
                HEART_BEAT = 16
            };

            private static Tuple<string, IOMode> MakeTuple(string prefix, IOMode valueMode)
            {
                return new Tuple<string, IOMode>(prefix, valueMode);
            }

            public static List<Tuple<string, IOMode>> ValueDefineList = new List<Tuple<string, IOMode>>()
            {
                MakeTuple("Rev.val=", IOMode.DATA_MODE_INT),
                MakeTuple("Thr.val=", IOMode.DATA_MODE_INT),
                MakeTuple("Brk.val=", IOMode.DATA_MODE_INT),
                MakeTuple("Spd.txt=", IOMode.DATA_MODE_STRING),
                MakeTuple("SpdLim.val=", IOMode.DATA_MODE_INT),
                MakeTuple("Time.txt=", IOMode.DATA_MODE_STRING),
                MakeTuple("Panto.val=", IOMode.DATA_MODE_BOOL),
                MakeTuple("Vol.val=", IOMode.DATA_MODE_INT),
                MakeTuple("Amm.val=", IOMode.DATA_MODE_INT),
                MakeTuple("No.txt=", IOMode.DATA_MODE_STRING),
                MakeTuple("SigCode.val=", IOMode.DATA_MODE_INT),
                MakeTuple("OvrSpd.val=", IOMode.DATA_MODE_INT),
                MakeTuple("Horn.val=", IOMode.DATA_MODE_INT),
                MakeTuple("Sander.val=", IOMode.DATA_MODE_INT),
                MakeTuple("Headlight.val=", IOMode.DATA_MODE_INT),
                MakeTuple("Door.val=", IOMode.DATA_MODE_INT),
                MakeTuple("conStart=", IOMode.DATA_MODE_INT)
            };
        };

        public class Communication
        {
            public static int COMM_BAUDRATE = 921600;

            public static int COMM_SEND_FREQ = 200;
            public static int COMM_TIMEOUT = 250;
            public static int COMM_RECV_FREQ = 50;
            public static int COMM_DELAY_PER_SEND = 0;

            public static string COMM_CONNECT_REQUEST = "conReq=1";
            public static string COMM_CONNECT_RESPONSE = "HMI";
            public static string COMM_START_TRANS_REQUEST = "conStart=1";
            public static byte[] COMM_TAIL = new byte[3] { 0xff, 0xff, 0xff };

            public static int COMM_RECV_BUFFER_SIZE = 32768;
            public static int COMM_SEND_BUFFER_SIZE = 32768;
        }
    }
}
