using Scarlet.Communications;
using Scarlet.Utilities;
using Science.Library;

namespace Science
{
    public class PacketHandler
    {

        public PacketHandler()
        {
            Parse.SetParseHandler(ScienceConstants.Packets.EMERGENCY_STOP, ParseStopPacket);
            Parse.SetParseHandler(ScienceConstants.Packets.DRL_CTRL, ParseDrillSpeedPacket);
            Parse.SetParseHandler(ScienceConstants.Packets.SERVO_SET, ParseServoPacket);
            Parse.SetParseHandler(ScienceConstants.Packets.RAIL_TARGET_SET, ParseRailTargetPacket);
            Parse.SetParseHandler(ScienceConstants.Packets.RAIL_SPEED_SET, ParseRailSpeedPacket);
            Parse.SetParseHandler(ScienceConstants.Packets.TTB_SET, ParseTurntableTargetPacket);
        }

        public static void ParseDrillSpeedPacket(Packet Packet)
        {
            if (CheckPacket(Packet, 2, "Drill Speed"))
            {
                bool Enable = (Packet.Data.Payload[0] & 0b1) == 0b1;
                bool Reverse = (Packet.Data.Payload[0] & 0b10) == 0b10;
                RoverMain.IOHandler.DrillController.SetSpeed(Packet.Data.Payload[1] / 100.0F * (Reverse ? -1 : 1), Enable);
            }
        }

        public static void ParseServoPacket(Packet Packet)
        {
            if (CheckPacket(Packet, 5, "Servo Set"))
            {
                if(Packet.Data.Payload[0] == 0x00) // Sample Door
                {
                    RoverMain.IOHandler.DrillController.DoorOpen = (UtilData.ToInt(UtilMain.SubArray(Packet.Data.Payload, 1, 4)) == 1); // TODO: See if this is correct.
                }
            }
        }

        public static void ParseRailSpeedPacket(Packet Packet)
        {
            if (CheckPacket(Packet, 1, "Rail Speed"))
            {
                RoverMain.IOHandler.RailController.RailSpeed = Packet.Data.Payload[0] / 100F;
            }
        }

        public static void ParseRailTargetPacket(Packet Packet)
        {
            if (CheckPacket(Packet, 5, "Rail Target"))
            {
                float TargetDist = UtilData.ToFloat(UtilMain.SubArray(Packet.Data.Payload, 1, 4));
                switch (Packet.Data.Payload[0])
                {
                    case 0x00:
                        RoverMain.IOHandler.RailController.GotoTop();
                        break;
                    case 0x01:
                        RoverMain.IOHandler.RailController.GotoDrillGround();
                        break;
                    case 0x02:
                        RoverMain.IOHandler.RailController.TargetLocation = TargetDist;
                        RoverMain.IOHandler.RailController.TargetLocationRefIsTop = true;
                        break;
                    case 0x03:
                        RoverMain.IOHandler.RailController.TargetLocation = TargetDist;
                        RoverMain.IOHandler.RailController.TargetLocationRefIsTop = false;
                        break;
                    case 0x04:
                        RoverMain.IOHandler.RailController.DoInit();
                        break;
                }
            }
        }

        public static void ParseStopPacket(Packet StopPacket)
        {
            Log.Output(Log.Severity.FATAL, Log.Source.OTHER, "EMERGENCY STOP RECEIVED");
            RoverMain.IOHandler.EmergencyStop();
        }

        public static void ParseTurntableTargetPacket(Packet Packet)
        {
            if(CheckPacket(Packet, 5, "Turntable Set"))
            {
                if(Packet.Data.Payload[0] == 0x00) // Move
                {
                    RoverMain.IOHandler.TurntableController.GotoLocation(UtilData.ToInt(UtilMain.SubArray(Packet.Data.Payload, 1, 4)));
                }
                else if(Packet.Data.Payload[0] == 0x01) // Init
                {
                    RoverMain.IOHandler.TurntableController.DoInit();
                }
            }
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
