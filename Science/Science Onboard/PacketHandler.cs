using Scarlet.Communications;
using Scarlet.Utilities;
using Science.Library;

namespace Science
{
    public class PacketHandler
    {

        public PacketHandler()
        {
            Parse.SetParseHandler(ScienceConstants.Packets.ERROR, ParseErrorPacket);
            Parse.SetParseHandler(ScienceConstants.Packets.EMERGENCY_STOP, ParseStopPacket);
            Parse.SetParseHandler(ScienceConstants.Packets.CONTROL, ParseControlPacket);
        }

        public static void ParseControlPacket(Packet Packet)
        {
            if (CheckPacket(Packet, 4, "Control"))
            {
                RoverMain.IOHandler.DrillController.SetSpeed(Packet.Data.Payload[0] / 100.0F * ((Packet.Data.Payload[1] == 0) ? 1 : -1), true);
                RoverMain.IOHandler.RailController.SetSpeed(Packet.Data.Payload[2] / 100.0F * ((Packet.Data.Payload[3] == 0) ? 1 : -1), true);
            }
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

        private static bool CheckPacket(Packet Packet, int ExpectedLength, string PacketName)
        {
            if (Packet == null || Packet.Data == null || Packet.Data.Payload == null || Packet.Data.Payload.Length != ExpectedLength)
            {
                Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, PacketName + " packet invalid. Discarding.");
                return false;
            }
            return true;
        }
    }
}
