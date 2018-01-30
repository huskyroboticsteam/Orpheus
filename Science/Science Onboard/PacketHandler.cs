using Scarlet.Communications;
using Scarlet.Utilities;
using Science.Library;

namespace Science
{
    public class PacketHandler
    {

        public PacketHandler()
        {
            Parse.SetParseHandler(PacketType.WATCHDOG_PING, ParseWatchdog);
            Parse.SetParseHandler(PacketType.ERROR, ParseErrorPacket);
            Parse.SetParseHandler(PacketType.EMERGENCY_STOP, ParseStopPacket);
        }

        public static void ParseErrorPacket(Packet Error)
        {
            Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Error Packet Received");
        }

        public static void ParseStopPacket(Packet StopPacket)
        {
            Log.Output(Log.Severity.FATAL, Log.Source.OTHER, "EMERGENCY STOP RECEIVED");
            RoverMain.IOHandler.EmergencyStop();
        }

        public static void ParseWatchdog(Packet WatchdogPacket)
        {
            
        }
    }
}
