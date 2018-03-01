using Scarlet.Communications;
using Scarlet.Utilities;
using Science.Library;

namespace Science
{
    public class PacketHandler
    {

        public PacketHandler()
        {
            //Parse.SetParseHandler(ScienceConstants.Packets.WATCHDOG_PING, ParseWatchdog); // TODO: What are we doing with this?
            Parse.SetParseHandler(ScienceConstants.Packets.ERROR, ParseErrorPacket);
            Parse.SetParseHandler(ScienceConstants.Packets.EMERGENCY_STOP, ParseStopPacket);
            Parse.SetParseHandler(ScienceConstants.Packets.CONTROL, ParseControlPacket);
        }

        public static void ParseControlPacket(Packet Packet)
        {
            if (Packet == null || Packet.Data == null || Packet.Data.Payload == null || Packet.Data.Payload.Length != 2) { Log.Output(Log.Severity.WARNING, Log.Source.MOTORS, "Control packet invalid length."); }
            RoverMain.IOHandler.DrillController.SetSpeed(Packet.Data.Payload[0], Packet.Data.Payload[1] == 0);
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
