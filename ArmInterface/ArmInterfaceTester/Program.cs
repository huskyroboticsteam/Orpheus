using ArmInterface;
using Scarlet.Communications;
using Scarlet.IO.BeagleBone;
using Scarlet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ArmControllerCommonFiles.Values;

namespace ArmControllerCommonFiles
{
    public class Program
    {
        public static bool ENABLED = true;
        public static string ServerIP = "192.168.7.1";
        public static int TCPPort = 1765;
        public static int UDPPort = 2765;


        private static bool MODEL_NUM_FLG = false;      // RECEIVE
        private static bool ENCODER_CNT_FLG = false;    // RECEIVE
        private static bool STATUS_FLG = false;         // RECEIVE
        private static bool TELEMETRY_FLG = false;      // RECEIVE
        private static bool SERVO_FLG = false;          // RECEIVE
        private static bool LASER_FLG = false;          // RECEIVE
        private static bool ERROR_MSG_FLG = false;      // RECEIVE  ( PRIORITY )

        public static Dictionary<Device, JointTelemetry> Telemetry;

        public static void Main(string[] args)
        {
            Telemetry = new Dictionary<Device, JointTelemetry>
            {
                { Device.BASE, new JointTelemetry() },
                { Device.SHOULDER, new JointTelemetry() },
                { Device.ELBOW, new JointTelemetry() },
                { Device.WRIST, new JointTelemetry() },
                { Device.DIFFERENTIAL_1, new JointTelemetry() },
                { Device.DIFFERENTIAL_2, new JointTelemetry() },
                { Device.HAND, new JointTelemetry() }
            };

            Log.Begin();
            Log.Output(Log.Severity.INFO, Log.Source.ALL, "Initializing CAN Bus...");
            BeagleBone.Initialize(SystemMode.NO_HDMI, true);
            BBBPinManager.AddBusCAN(0, false);
            BBBPinManager.ApplyPinSettings(BBBPinManager.ApplicationMode.APPLY_IF_NONE);
            Log.Output(Log.Severity.INFO, Log.Source.ALL, "CAN Initialized.");

            Client.Start(ServerIP, TCPPort, UDPPort, "ArmTestClient");

            Arm arm = new Arm(CANBBB.CANBus0);
            Parsing.Start(arm);

            while (ENABLED)
            {
                ArmPacket? next = arm.ReadNext();
                if (next != null)
                {
                    UpdateTelemetry((ArmPacket)next);
                }
                SendTelemetry();
                Thread.Sleep(Constants.DEFAULT_MIN_THREAD_SLEEP);
            }
        }

        private static void UpdateTelemetry(ArmPacket incomingPacket)
        {
            JointTelemetry tel = Telemetry[incomingPacket.TargetDeviceID];
            switch (incomingPacket.PacketType)
            {
                case CANPacket.ENCODER_CNT:
                    tel.Encoder = UtilData.ToInt(incomingPacket.Payload);
                    ENCODER_CNT_FLG = false;
                    break;
                case CANPacket.MODEL_NUM:
                    tel.ModelNumber = UtilData.ToInt(incomingPacket.Payload);
                    break;
                case CANPacket.TELEMETRY:
                    // Break up into Voltage and Current
                    byte[] voltageData = incomingPacket.Payload.Take(2).ToArray();
                    byte[] currentData = incomingPacket.Payload.Skip(2).Take(2).ToArray();
                    tel.Current = UtilData.ToInt(currentData);
                    tel.Voltage = UtilData.ToInt(voltageData);
                    break;
                case CANPacket.SERVO:
                    tel.ServoPos = UtilData.ToInt(incomingPacket.Payload);
                    break;
                case CANPacket.ERROR_MSG:
                    tel.ErrorCode = incomingPacket.Payload[0];
                    break;
            }
            Telemetry[incomingPacket.TargetDeviceID] = tel;
        }

        private static void SendTelemetry()
        {
            // Send encoder data
            if (!ENCODER_CNT_FLG)
            {
                foreach (Device dev in Vals.DEVICES)
                {
                    byte[] encoderData = new byte[5];
                    encoderData[0] = (byte)CANPacket.ENCODER_CNT;
                    byte[] encoderVals = BitConverter.GetBytes(Telemetry[dev].Encoder);
                    for (int i = 0; i < 4; i++) { encoderData[i + 1] = encoderVals[i]; }
                    Message thisTelemetry = new Message(Values.DeviceToTelID(dev), encoderData);
                    Client.Send(new Packet(thisTelemetry, true));
                }
                ENCODER_CNT_FLG = true;
            }

            // Send Current / Voltage data
            if (!TELEMETRY_FLG)
            {
                foreach (Device dev in Vals.DEVICES)
                {
                    byte[] telemetryData = new byte[5];
                    telemetryData[0] = (byte)CANPacket.TELEMETRY;
                    byte[] currentVals = BitConverter.GetBytes((ushort)Telemetry[dev].Current);
                    byte[] voltageVals = BitConverter.GetBytes((ushort)Telemetry[dev].Voltage);
                    for (int i = 0; i < 2; i++)
                    {
                        telemetryData[i + 1] = voltageVals[i];
                        telemetryData[i + 3] = currentVals[i];
                    }
                    Message thisTelemetry = new Message(Values.DeviceToTelID(dev), telemetryData);
                    Client.Send(new Packet(thisTelemetry, true));
                }
                TELEMETRY_FLG = true;
            }

            // Send Error Information
            if (!ERROR_MSG_FLG)
            {
                foreach (Device dev in Vals.DEVICES)
                {
                    byte[] errorData = new byte[5];
                    errorData[0] = (byte)CANPacket.ERROR_MSG;
                    byte[] errorVals = BitConverter.GetBytes((ushort)Telemetry[dev].ErrorCode);
                    for (int i = 0; i < 2; i++)
                    {
                        errorData[i + 1] = errorVals[i];
                    }
                    Message thisTelemetry = new Message(Values.DeviceToTelID(dev), errorData);
                    Client.Send(new Packet(thisTelemetry, true));
                }
                ERROR_MSG_FLG = true;
            }

            // Model Number
            if (!MODEL_NUM_FLG)
            {
                foreach (Device dev in Vals.DEVICES)
                {
                    byte[] modelNumberData = new byte[2];
                    modelNumberData[0] = (byte)CANPacket.MODEL_NUM;
                    modelNumberData[1] = (byte)Telemetry[dev].ModelNumber;
                    Message thisTelemetry = new Message(Values.DeviceToTelID(dev), modelNumberData);
                    Client.Send(new Packet(thisTelemetry, true));
                }
                MODEL_NUM_FLG = true;
            }

            // Servos
            if (!SERVO_FLG)
            {
                foreach (Device dev in Vals.DEVICES)
                {
                    byte[] servoData = new byte[2];
                    servoData[0] = (byte)CANPacket.SERVO;
                    servoData[1] = (byte)Telemetry[dev].ServoPos;
                    Message thisTelemetry = new Message(Values.DeviceToTelID(dev), servoData);
                    Client.Send(new Packet(thisTelemetry, true));
                }
                SERVO_FLG = true;
            }
        }

    }
}
