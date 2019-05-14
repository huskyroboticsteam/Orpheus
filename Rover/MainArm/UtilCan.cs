using System;
using Scarlet.IO;

namespace MainRover
{ 
    public static class UtilCan 
    {

        private static uint ConstructCanID(bool Priority, byte Sender, byte Receiver)
        {
            return Convert.ToUInt16(Convert.ToUInt16(!Priority) * (1024) + Sender * (32) + Receiver);
        }

        private static void TwoData(ICANBus CANBus, bool Priority, byte Sender, byte Receiver, byte DataID, UInt16 val1, UInt16 val2)
        {
            byte[] Payload = new byte[5];
            Payload[0] = DataID;
            Payload[1] = (byte)(val1 & 0xFF);
            Payload[2] = (byte)((val1 >> 8) & 0xFF);
            Payload[3] = (byte)(val2 & 0xFF);
            Payload[4] = (byte)((val2 >> 8) & 0xFF);
            CANBus.Write(ConstructCanID(Priority, Sender, Receiver), Payload);
        }

        public static void ModeSelect(ICANBus CANBus, bool Priority, byte Sender, byte Receiver, byte Mode)
        {
            byte[] Payload = new byte[2];
            Payload[0] = 0;
            Payload[1] = Mode;
            CANBus.Write(ConstructCanID(Priority, Sender, Receiver), Payload);
        }

        public static void SpeedDir(ICANBus CANBus, bool Priority, byte Sender, byte Receiver, byte Speed, byte Direction)
        {
            byte[] Payload = new byte[3];
            Payload[0] = 2;
            Payload[1] = Speed;
            Payload[2] = Direction;
            CANBus.Write(ConstructCanID(Priority, Sender, Receiver), Payload);
        }

        public static void AngleSpeed(ICANBus CANBus, bool Priority, byte Sender, byte Receiver, UInt16 Angle, UInt16 Velocity)
        {
            TwoData(CANBus, Priority, Sender, Receiver, 4, Angle, Velocity);
        }

        public static void Index(ICANBus CANBus, bool Priority, byte Sender, byte Receiver)
        {
            byte[] Payload = new byte[1];
            Payload[0] = 6;
            CANBus.Write(ConstructCanID(Priority, Sender, Receiver), Payload);
        }

        public static void Reset(ICANBus CANBus, bool Priority, byte Sender, byte Receiver)
        {
            byte[] Payload = new byte[1];
            Payload[0] = 8;
            CANBus.Write(ConstructCanID(Priority, Sender, Receiver), Payload);
        }

        public static void SetP(ICANBus CANBus, bool Priority, byte Sender, byte Receiver, UInt16 val1, UInt16 val2)
        {
            TwoData(CANBus, Priority, Sender, Receiver, 10, val1, val2);
        }

        public static void SetI(ICANBus CANBus, bool Priority, byte Sender, byte Receiver, UInt16 val1, UInt16 val2)
        {
            TwoData(CANBus, Priority, Sender, Receiver, 12, val1, val2);
        }

        public static void SetD(ICANBus CANBus, bool Priority, byte Sender, byte Receiver, UInt16 val1, UInt16 val2)
        {
            TwoData(CANBus, Priority, Sender, Receiver, 14, val1, val2);
        }

        public static void SetTicksPerRev(ICANBus CANBus, bool Priority, byte Sender, byte Receiver, UInt16 TPR)
        {
            byte[] Payload = new byte[3];
            Payload[0] = 15;
            Payload[1] = (byte)(TPR & 0xFF);
            Payload[2] = (byte)((TPR >> 8) & 0xFF);
            CANBus.Write(ConstructCanID(Priority, Sender, Receiver), Payload);
        }

        public static void ModelReq(ICANBus CANBus, bool Priority, byte Sender, byte Receiver)
        {
            byte[] Payload = new byte[1];
            Payload[0] = 16;
            CANBus.Write(ConstructCanID(Priority, Sender, Receiver), Payload);
        }

        public static Tuple<short, short> GetTele(byte[] data)
        {
            short voltage = -1;
            short current = -1;

            if(data[0] == 0x18)
            {
                byte[] va = new byte[2];
                byte[] ca = new byte[2];

                va[0] = data[1];
                va[1] = data[2];
                ca[0] = data[3];
                ca[1] = data[4];

                voltage = BitConverter.ToInt16(va, 0);
                current = BitConverter.ToInt16(va, 0);
            }

           return new Tuple<short, short>(voltage, current);
        }

    }

}