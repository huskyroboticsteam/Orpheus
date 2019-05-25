using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainRover
{

    // SEE DOCUMENTATION FOR CAN HERE:
    // https://docs.google.com/spreadsheets/d/1Kh6HH6FuGfRw80UQn9ORNWAsChXJ9NLELxFyO2SJyak/edit?usp=sharing
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
        SERVO_PD_VOLTAGE = 0x22,        // RECEIVE ( IF TALKING TO AN ARM PDB, THIS PACKET IS VOLTAGE SENSE ) 
        LASER_PD_CURRENT = 0x24,        // RECEIVE ( IF TALKING TO AN ARM PDB, THIS PACKET IS CURRENT SENSE ) 
        RELAY_SHUTOFF = 0x26,           // SEND     SEND A SHUTOFF SIGNAL TO RELAY ON POWER DISTRO 
        ERROR_MSG = 0xFF                // RECEIVE  ( PRIORITY )
    }

    public enum Device : byte
    {
        BASE = 0x10,
        SHOULDER = 0x11,
        ELBOW = 0x12,
        WRIST = 0x13,
        DIFFERENTIAL_1 = 0x14,
        DIFFERENTIAL_2 = 0x15,
        HAND = 0x16,
        ARM_POWER_DISTRO_1 = 0x17
    }

    public static class Vals
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
