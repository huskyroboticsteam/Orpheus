using ArmControllerCommonFiles;
using ArmInterface;
using Scarlet.Communications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestArmControllerGUI;
using static ArmControllerCommonFiles.Values;
using static TestArmControllerGUI.Main;

namespace TestArmControllerGUI
{
    internal class Parsing
    {
        public delegate void Update();
        public event Update UpdateGUI;

        internal Parsing()
        {

            Parse.SetParseHandler(BASE_TEL_ID, ParseBaseTel);
            Parse.SetParseHandler(SHOULDER_TEL_ID, ParseShoulderTel);
            Parse.SetParseHandler(ELBOW_TEL_ID, ParseElbowTel);
            Parse.SetParseHandler(WRIST_TEL_ID, ParseWristTel);
            Parse.SetParseHandler(DIFF_1_TEL_ID, ParseDiff1Tel);
            Parse.SetParseHandler(DIFF_2_TEL_ID, ParseDiff2Tel);
            Parse.SetParseHandler(HAND_TEL_ID, ParseHandTel);
        }

        private void ParseBaseTel(Packet incomingPacket)
        {
            ParseData(Device.BASE, incomingPacket.Data.Payload);
        }

        private void ParseShoulderTel(Packet incomingPacket)
        {
            ParseData(Device.SHOULDER, incomingPacket.Data.Payload);
        }

        private void ParseElbowTel(Packet incomingPacket)
        {
            ParseData(Device.ELBOW, incomingPacket.Data.Payload);
        }

        private void ParseWristTel(Packet incomingPacket)
        {
            ParseData(Device.WRIST, incomingPacket.Data.Payload);
        }

        private void ParseDiff1Tel(Packet incomingPacket)
        {
            ParseData(Device.DIFFERENTIAL_1, incomingPacket.Data.Payload);
        }

        private void ParseDiff2Tel(Packet incomingPacket)
        {
            ParseData(Device.BASE, incomingPacket.Data.Payload);
        }

        private void ParseHandTel(Packet incomingPacket)
        {
            ParseData(Device.DIFFERENTIAL_2, incomingPacket.Data.Payload);
        }

        private void ParseData(Device ParseFor, byte[] Payload)
        {
            JointTelemetry tel = Telemetry[ParseFor];
            CANPacket canPack = (CANPacket) Payload[0];
            switch(canPack)
            {
                case CANPacket.TELEMETRY:
                    int Voltage = BitConverter.ToInt16(Payload.Take(3).ToArray(), 1);
                    int Current = BitConverter.ToInt16(Payload, 3);
                    tel.Voltage = Voltage;
                    tel.Current = Current;
                    break;
                case CANPacket.STATUS:
                    // Basically doesn't exist as of now
                    break;
                case CANPacket.ENCODER_CNT:
                    int EncoderCount = BitConverter.ToInt32(Payload, 1);
                    tel.Encoder = EncoderCount;
                    break;
                case CANPacket.ERROR_MSG:
                    int Error = BitConverter.ToInt16(Payload, 0) & 0xFF;
                    tel.ErrorCode = Error;
                    break;
                case CANPacket.MODEL_NUM:
                    tel.ModelNumber = BitConverter.ToInt16(Payload, 0) & 0xFF;
                    break;
                case CANPacket.SERVO:
                    tel.ServoPos = BitConverter.ToInt16(Payload, 0) & 0xFF;
                    break;
                case CANPacket.LASER:
                    // Does not exist yet
                    break;
            }
            UpdateGUI?.Invoke();
            Telemetry[ParseFor] = tel;
        }
    }
    
}
