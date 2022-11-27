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
            public enum ValueMode
            {
                DATA_MODE_DOUBLE = 0,
                DATA_MODE_INT = 1,
                DATA_MODE_BOOL = 2,
                DATA_MODE_STRING = 3,
                DATA_MODE_UNDEFINED = 4
            };

            public enum ValueEnum
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
                HEART_BEAT = 15
            };

            private static Tuple<string, ValueMode> MakeTuple(string prefix, ValueMode valueMode)
            {
                return new Tuple<string, ValueMode>(prefix, valueMode);
            }

            public static List<Tuple<string, ValueMode>> ValueDefineList = new List<Tuple<string, ValueMode>>()
            {
                MakeTuple("Rev.val=", ValueMode.DATA_MODE_INT),
                MakeTuple("Thr.val=", ValueMode.DATA_MODE_INT),
                MakeTuple("Brk.val=", ValueMode.DATA_MODE_INT),
                MakeTuple("Spd.txt=", ValueMode.DATA_MODE_STRING),
                MakeTuple("SpdLim.val=", ValueMode.DATA_MODE_INT),
                MakeTuple("Time.txt=", ValueMode.DATA_MODE_STRING),
                MakeTuple("Panto.val=", ValueMode.DATA_MODE_BOOL),
                MakeTuple("Vol.val=", ValueMode.DATA_MODE_INT),
                MakeTuple("Amm.val=", ValueMode.DATA_MODE_INT),
                MakeTuple("No.txt=", ValueMode.DATA_MODE_STRING),
                MakeTuple("SigCode.val=", ValueMode.DATA_MODE_INT),
                MakeTuple("OvrSpd.val=", ValueMode.DATA_MODE_INT),
                MakeTuple("Horn.val=", ValueMode.DATA_MODE_INT),
                MakeTuple("Sander.val=", ValueMode.DATA_MODE_INT),
                MakeTuple("Headlight.val=", ValueMode.DATA_MODE_INT),
                MakeTuple("conStart=", ValueMode.DATA_MODE_INT)
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
