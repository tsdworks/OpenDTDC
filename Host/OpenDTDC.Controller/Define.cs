using System.Collections.Generic;

namespace OpenDTDC.Controller
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
                DEV_MODE_UNDEFINED = 0x00,
                DEV_MODE_INPUT = 0x01,
                DEV_MODE_INPUT_PULLUP = 0x02,
                DEV_MODE_INPUT_ANALOG = 0x03,
                DEV_MODE_OUTPUT = 0x04,
                DEV_MODE_OUTPUT_ANALOG = 0x05,
                DEV_MODE_SERIAL = 0x06
            };

            public enum IOEnum
            {
                // ACT0 - ACT2
                ACT0 = 0, ACT1 = 1, ACT2 = 2,
                // BP0
                BP0 = 3,
                // L0 - L4
                L0 = 4, L1 = 5, L2 = 6, L3 = 7, L4 = 8,
                // ANA0 - ANA2
                ANA0 = 9, ANA1 = 10, ANA2 = 11,
                // DDS01 - DDS42
                DDS01 = 12, DDS02 = 13,
                DDS11 = 14, DDS12 = 15,
                DDS21 = 16, DDS22 = 17,
                DDS31 = 18, DDS32 = 19,
                DDS41 = 20, DDS42 = 21,
                // SDS0 - SDS5
                SDS0 = 22, SDS1 = 23, SDS2 = 24, SDS3 = 25, SDS4 = 26, SDS5 = 27,
                // SDS6 - SDS11
                SDS6 = 28, SDS7 = 29, SDS8 = 30, SDS9 = 31, SDS10 = 32, SDS11 = 33,
                // SER0
                SER0 = 34
            };

            public static List<IOMode> IOModeList = new List<IOMode>()
            {
                // ACT0 - ACT2
                IOMode.DEV_MODE_OUTPUT, IOMode.DEV_MODE_OUTPUT, IOMode.DEV_MODE_OUTPUT,
                // BP0
                IOMode.DEV_MODE_OUTPUT,
                // L0 - L4
                IOMode.DEV_MODE_OUTPUT, IOMode.DEV_MODE_OUTPUT, IOMode.DEV_MODE_OUTPUT, IOMode.DEV_MODE_OUTPUT, IOMode.DEV_MODE_OUTPUT,
                // ANA0 - ANA2
                IOMode.DEV_MODE_INPUT_ANALOG, IOMode.DEV_MODE_INPUT_ANALOG, IOMode.DEV_MODE_INPUT_ANALOG,
                // DDS01 - DDS42
                IOMode.DEV_MODE_INPUT_PULLUP, IOMode.DEV_MODE_INPUT_PULLUP,
                IOMode.DEV_MODE_INPUT_PULLUP, IOMode.DEV_MODE_INPUT_PULLUP,
                IOMode.DEV_MODE_INPUT_PULLUP, IOMode.DEV_MODE_INPUT_PULLUP,
                IOMode.DEV_MODE_INPUT_PULLUP, IOMode.DEV_MODE_INPUT_PULLUP,
                IOMode.DEV_MODE_INPUT_PULLUP, IOMode.DEV_MODE_INPUT_PULLUP,
                // SDS0 - SDS5
                IOMode.DEV_MODE_INPUT, IOMode.DEV_MODE_INPUT, IOMode.DEV_MODE_INPUT, IOMode.DEV_MODE_INPUT, IOMode.DEV_MODE_INPUT, IOMode.DEV_MODE_INPUT,
                // SDS6 - SDS11
                IOMode.DEV_MODE_INPUT, IOMode.DEV_MODE_INPUT, IOMode.DEV_MODE_INPUT, IOMode.DEV_MODE_INPUT, IOMode.DEV_MODE_INPUT, IOMode.DEV_MODE_INPUT,
                // SER0
                IOMode.DEV_MODE_SERIAL
            };
        };

        public class Communication
        {
            public static int COMM_BAUDRATE = 250000;

            public static int COMM_SEND_FREQ = 30;
            public static int COMM_RECV_FREQ = 200;
            public static int COMM_TIMEOUT = 250;
            public static int COMM_PACK_MIN_SIZE = 5;

            public static string COMM_CONNECT_REQUEST = "Connect";
            public static string COMM_CONNECT_RESPONSE = "Controller";
            public static string COMM_START_TRANS_REQUEST = "Start";
            public static byte COMM_HEAD = 0x6a;
            public static byte COMM_TAIL = 0xa6;
            public static byte COMM_NODATA = 0xff;

            public static int COMM_HEAD_POS = 0;
            public static int COMM_TYPE_POS = 1;
            public static int COMM_VER_POS = 2;
            public static int COMM_NUM_POS = 3;

            public static int COMM_RECV_BUFFER_SIZE = 4096;
            public static int COMM_SEND_BUFFER_SIZE = 4096;
        }
    }
}
