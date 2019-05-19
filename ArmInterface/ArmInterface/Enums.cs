using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmInterface
{
    public enum CANPacket : byte
    {
        MODE_SELECT = 0x00,             // SEND
        SPEED_DIR = 0x02,               // SEND
        ANGLE_MAX_SPD = 0x04,           // SEND
        INDEX = 0x06,                   // SEND
        RESET = 0x08,                   // SEND     ( PRIORITY )
        SET_P = 0x0A,                   // SEND
        SET_I = 0x0C,                   // SEND
        SET_D = 0x0E,                   // SEND
        SET_TICKS_PER_REV = 0x0F,       // SEND
        MODEL_REQ = 0x10,               // SEND
        MODEL_NUM = 0x12,               // RECEIVE
        ENCODER_CNT = 0x14,             // RECEIVE
        STATUS = 0x16,                  // RECEIVE
        TELEMETRY = 0x18,               // RECEIVE
        SERVO = 0x22,                   // RECEIVE
        LASER = 0x24,                   // RECEIVE
        ERROR_MSG = 0xFF                // RECEIVE  ( PRIORITY )
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

    public class Vals
    {
        public static readonly Device[] DEVICES = new Device[] 
        {
            Device.BASE,
            Device.SHOULDER,
            Device.ELBOW,
            Device.WRIST,
            Device.DIFFERENTIAL_1,
            Device.DIFFERENTIAL_2,
            Device.HAND
        };
    }

}
