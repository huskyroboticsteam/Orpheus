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

        public static void DrillSpeedChange(int NewValue)
        {
            if (NewValue < -100 || NewValue > 100) { NewValue = 0; }
            Packet Packet = new Packet(new Message(ScienceConstants.Packets.CONTROL, new byte[] { (byte)Math.Abs(NewValue), ((NewValue < 0) ? (byte)0x01 : (byte)0x00) }), true, ScienceConstants.CLIENT_NAME);
            if (IsDrillEnabled && Server.GetClients().Contains(ScienceConstants.CLIENT_NAME)) { Server.Send(Packet); }
        }

    }
}
