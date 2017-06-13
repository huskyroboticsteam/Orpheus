using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoboticsLibrary.Communications;
using RoboticsLibrary.Utilities;
using RoboticsLibrary.Commands;

namespace Science
{
    public class PacketHandler
    {

        public PacketHandler()
        {
            Parse.SetParseHandler((int)PacketType.Error, ParseErrorPacket);
            Parse.SetParseHandler((int)PacketType.StopPacket, ParseStopPacket);
        }

        public static void ParseErrorPacket(Message Error)
        {
            Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Error Packet Received");
        }

        public static void ParseStopPacket(Message StopPacket)
        {
            Log.Output(Log.Severity.FATAL, Log.Source.OTHER, "EMERGENCY STOP RECEIVED");
            EmergencyStopListener.Stop = true;
        }
    }
}
