using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmInterface;
using Scarlet.Communications;

namespace ArmControllerCommonFiles
{
    public static class Parsing
    {
        private static Arm ArmController;

        public static void Start(Arm arm)
        {
            ArmController = arm;
            Parse.SetParseHandler(Values.BASE_CMD_ID, ParseBaseCmd);
            Parse.SetParseHandler(Values.SHOULDER_CMD_ID, ParseShoulderCmd);
            Parse.SetParseHandler(Values.ELBOW_CMD_ID, ParseElbowCmd);
            Parse.SetParseHandler(Values.WRIST_CMD_ID, ParseWristCmd);
            Parse.SetParseHandler(Values.DIFF_1_CMD_ID, ParseDiff1Cmd);
            Parse.SetParseHandler(Values.DIFF_2_CMD_ID, ParseDiff2Cmd);
            Parse.SetParseHandler(Values.HAND_CMD_ID, ParseHandCmd);
        }

        private static void ParseBaseCmd(Packet incomingPacket)
        {
            ArmPacket packet = new ArmPacket()
            {
                TargetDeviceID = Device.BASE,
                PacketType = (CANPacket)incomingPacket.Data.Payload[0],
                Payload = incomingPacket.Data.Payload,
                Priority = false
            };
            ArmController.Send(packet);
        }

        private static void ParseShoulderCmd(Packet incomingPacket)
        {
            ArmPacket packet = new ArmPacket()
            {
                TargetDeviceID = Device.SHOULDER,
                PacketType = (CANPacket)incomingPacket.Data.Payload[0],
                Payload = incomingPacket.Data.Payload,
                Priority = false
            };
            ArmController.Send(packet);
        }

        private static void ParseElbowCmd(Packet incomingPacket)
        {
            ArmPacket packet = new ArmPacket()
            {
                TargetDeviceID = Device.ELBOW,
                PacketType = (CANPacket)incomingPacket.Data.Payload[0],
                Payload = incomingPacket.Data.Payload,
                Priority = false
            };
            ArmController.Send(packet);
        }

        private static void ParseWristCmd(Packet incomingPacket)
        {
            ArmPacket packet = new ArmPacket()
            {
                TargetDeviceID = Device.WRIST,
                PacketType = (CANPacket)incomingPacket.Data.Payload[0],
                Payload = incomingPacket.Data.Payload,
                Priority = false
            };
            ArmController.Send(packet);
        }

        private static void ParseDiff1Cmd(Packet incomingPacket)
        {
            ArmPacket packet = new ArmPacket()
            {
                TargetDeviceID = Device.DIFFERENTIAL_1,
                PacketType = (CANPacket)incomingPacket.Data.Payload[0],
                Payload = incomingPacket.Data.Payload,
                Priority = false
            };
            ArmController.Send(packet);
        }

        private static void ParseDiff2Cmd(Packet incomingPacket)
        {
            ArmPacket packet = new ArmPacket()
            {
                TargetDeviceID = Device.DIFFERENTIAL_2,
                PacketType = (CANPacket)incomingPacket.Data.Payload[0],
                Payload = incomingPacket.Data.Payload,
                Priority = false
            };
            ArmController.Send(packet);
        }

        private static void ParseHandCmd(Packet incomingPacket)
        {
            ArmPacket packet = new ArmPacket()
            {
                TargetDeviceID = Device.HAND,
                PacketType = (CANPacket)incomingPacket.Data.Payload[0],
                Payload = incomingPacket.Data.Payload,
                Priority = false
            };
            ArmController.Send(packet);
        }
    }
}
