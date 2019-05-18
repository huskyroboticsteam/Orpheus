using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmInterface
{
    public enum CANPacket : byte
    {
        MODE_SELECT = 0x00,
        SPEED_DIR = 0x00,
        ANGLE_MAX_SPD = 0x00,
        INDEX = 0x00,
        RESET = 0x00,
        SET_P = 0x00,
        SET_I = 0x00,
        SET_D = 0x00,
        SET_TICKS_PER_REV = 0x00,
        MODEL_REQ = 0x00,
        MODEL_NUM = 0x00,
        ENCODER_CNT = 0x00,
        STATUS = 0x00,
        TELEMETRY = 0x00,
        ERROR_MSG = 0x00
    }

    public enum Device : byte
    {
        BASE = 0x00,
        SHOULDER = 0x01,
        ELBOW = 0x02,
        WRIST = 0x03,
        DIFFERENTIAL_1 = 0x04,
        DIFFERENTIAL_2 = 0x05,
        HAND = 0x06
    }

}
