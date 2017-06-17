using Scarlet.Communications;
using Scarlet.Science;
using Scarlet.Utilities;

namespace Science
{
    public class PacketHandler
    {

        public PacketHandler()
        {
            Parse.SetParseHandler((int)PacketType.Watchdog, ParseWatchdog);
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
            RoverMain.IOHandler.EmergencyStop();
        }

        public static void ParseWatchdog(Message WatchdogPacket)
        {
            
        }
    }
}
