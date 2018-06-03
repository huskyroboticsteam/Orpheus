using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scarlet.Communications;
using Scarlet.Utilities;
using Science.Library;

namespace Science_Base
{
    public static class Controls
    {

        public static bool IsDrillEnabled;
        private static int DrillSpd = 0;
        private static int RailSpd = 0;

        public static void DrillSpeedChange(int NewValue)
        {
            if (NewValue < -100 || NewValue > 100) { NewValue = 0; }
            if (!IsDrillEnabled) { NewValue = 0; }
            DrillSpd = NewValue;
            SendControlPacket();
        }
        
        public static void RailSpeedChange(int NewValue)
        {
            if (NewValue < -100 || NewValue > 100) { NewValue = 0; }
            RailSpd = NewValue;
            SendControlPacket();
        }

        private static void SendControlPacket()
        {
            Log.Output(Log.Severity.INFO, Log.Source.MOTORS, "Driving at " + DrillSpd + " and " + RailSpd + "%.");
            Packet Packet = new Packet(new Message(ScienceConstants.Packets.CONTROL, new byte[] { (byte)Math.Abs(DrillSpd), ((DrillSpd < 0) ? (byte)0x01 : (byte)0x00), (byte)Math.Abs(RailSpd), ((RailSpd < 0) ? (byte)0x01 : (byte)0x00) }), true, ScienceConstants.CLIENT_NAME);
            if (Server.GetClients().Contains(ScienceConstants.CLIENT_NAME)) { Server.Send(Packet); }
        }

    }
}
