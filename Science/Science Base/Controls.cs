using System;
using System.Linq;
using Scarlet.Communications;
using Scarlet.Utilities;
using Science.Library;

namespace Science_Base
{
    public static class Controls
    {
        public static bool IsDrillEnabled, SampleDoorState;
        private static int DrillSpd = 0;
        private static int RailSpd = 0;

        public static void DrillSpeedChange(int NewValue)
        {
            if (NewValue < -100 || NewValue > 100) { NewValue = 0; }
            if (!IsDrillEnabled) { NewValue = 0; }
            DrillSpd = NewValue;
            SendDrillSpdPacket();
        }
        
        public static void RailSpeedChange(int NewValue)
        {
            if (NewValue < 0 || NewValue > 100) { NewValue = 0; }
            RailSpd = NewValue;
            SendRailSpdPacket();
        }

        public static void RailTargetChange(bool FromTop, float TargetDist)
        {
            byte Command;
            if (FromTop && float.IsNaN(TargetDist)) { Command = 0x00; }
            else if (!FromTop && float.IsNaN(TargetDist)) { Command = 0x01; }
            else { Command = (byte)(FromTop ? 0x02 : 0x03); }
            Packet Packet = new Packet(new Message(ScienceConstants.Packets.RAIL_TARGET_SET, new byte[] { Command }.Concat(UtilData.ToBytes(TargetDist)).ToArray()), false, ScienceConstants.CLIENT_NAME);
            Server.Send(Packet);
        }

        public static void SampleDoorChange(bool NewValue)
        {
            Packet Packet = new Packet(new Message(ScienceConstants.Packets.SERVO_SET, new byte[]
            {
                0x00,
            }.Concat(UtilData.ToBytes((int)(NewValue ? 0x01 : 0x00))).ToArray()), false, ScienceConstants.CLIENT_NAME);
            Server.Send(Packet);
        }

        private static void SendDrillSpdPacket()
        {
            Packet Packet = new Packet(new Message(ScienceConstants.Packets.DRL_CTRL, new byte[]
            {
                (byte)(((DrillSpd < 0) ? 0b10 : 0b00) | (IsDrillEnabled ? 0b01 : 0b00)),
                (byte)Math.Abs(DrillSpd)
            }), false, ScienceConstants.CLIENT_NAME);
            if (Server.GetClients().Contains(ScienceConstants.CLIENT_NAME)) { Server.Send(Packet); }
        }

        private static void SendRailSpdPacket()
        {
            Packet Packet = new Packet(new Message(ScienceConstants.Packets.RAIL_SPEED_SET, new byte[]
            {
                (byte)Math.Abs(RailSpd)
            }), false, ScienceConstants.CLIENT_NAME);
            if (Server.GetClients().Contains(ScienceConstants.CLIENT_NAME)) { Server.Send(Packet); }
        }

    }
}
