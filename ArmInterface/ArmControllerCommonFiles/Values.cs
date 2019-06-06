using ArmInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmControllerCommonFiles
{
    public static class Values
    {
        public struct JointTelemetry
        {
            public int Encoder;
            public int Current;
            public int Voltage;
            public int ErrorCode;
            public int ModelNumber;
            public int ServoPos;
        }

        public const int BASE_CMD_ID        = 0x11;
        public const int BASE_TEL_ID        = 0x21;
                                                
        public const int SHOULDER_TEL_ID    = 0x12;
        public const int SHOULDER_CMD_ID    = 0x22;
                                                
        public const int ELBOW_CMD_ID       = 0x13;
        public const int ELBOW_TEL_ID       = 0x23;
                                                
        public const int WRIST_CMD_ID       = 0x14;
        public const int WRIST_TEL_ID       = 0x24;
                                                
        public const int DIFF_1_CMD_ID      = 0x15;
        public const int DIFF_1_TEL_ID      = 0x25;
                                                
        public const int DIFF_2_CMD_ID      = 0x16;
        public const int DIFF_2_TEL_ID      = 0x26;
                                                
        public const int HAND_CMD_ID        = 0x17;
        public const int HAND_TEL_ID        = 0x27;

        public static byte DeviceToTelID(Device dev)
        {
            switch (dev)
            {
                case Device.BASE:
                    return BASE_TEL_ID;
                case Device.SHOULDER:
                    return SHOULDER_TEL_ID;
                case Device.ELBOW:
                    return ELBOW_TEL_ID;
                case Device.WRIST:
                    return WRIST_TEL_ID;
                case Device.DIFFERENTIAL_1:
                    return DIFF_1_TEL_ID;
                case Device.DIFFERENTIAL_2:
                    return DIFF_2_TEL_ID;
                case Device.HAND:
                    return WRIST_TEL_ID;
                default:
                    return 0x00;
            }
        }

        public static byte DeviceToCmdID(Device dev)
        {
            switch (dev)
            {
                case Device.BASE:
                    return BASE_CMD_ID;
                case Device.SHOULDER:
                    return SHOULDER_CMD_ID;
                case Device.ELBOW:
                    return ELBOW_CMD_ID;
                case Device.WRIST:
                    return WRIST_CMD_ID;
                case Device.DIFFERENTIAL_1:
                    return DIFF_1_CMD_ID;
                case Device.DIFFERENTIAL_2:
                    return DIFF_2_CMD_ID;
                case Device.HAND:
                    return WRIST_CMD_ID;
                default:
                    return 0x00;
            }
        }
    }
}
