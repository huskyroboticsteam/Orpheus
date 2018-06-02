using System;
using System.Collections.Generic;
using Scarlet.Utilities;
using Scarlet.Communications;
using Scarlet.IO.BeagleBone;
using Scarlet.IO;

namespace ArmMaster
{
    class ArmMaster
    {
        const string CHASSIS_BEAGLEBONE = "192.168.0.5";
        static IUARTBus Slave;

        static void SetupBeaglebone()
        {
            Slave = UARTBBB.UARTBus1;
            StateStore.Start("ArmMaster");
            BeagleBone.Initialize(SystemMode.DEFAULT, true);
        }

        static void SetupPins()
        {
            BBBPinManager.AddMappingUART(Pins.SlaveRX);
            BBBPinManager.AddMappingUART(Pins.SlaveTX);
            BBBPinManager.AddMappingPWM(Pins.BaseRotation);
            BBBPinManager.AddMappingGPIO(Pins.BaseRotationDir, true, ResistorState.NONE);
            BBBPinManager.AddMappingPWM(Pins.Elbow);
            BBBPinManager.AddMappingPWM(Pins.Shoulder);
            BBBPinManager.AddMappingADC(Pins.ShoulderPot);
            BBBPinManager.AddMappingGPIO(Pins.ElbowLimitSwitch, false, ResistorState.PULL_UP);
            BBBPinManager.ApplyPinSettings(BBBPinManager.ApplicationMode.APPLY_IF_NONE);
        }

        static void SetupClient()
        {
            Client.Start(CHASSIS_BEAGLEBONE, 1025, 1026, "ArmMaster");
            Parse.SetParseHandler(0x80, (Pack) =>
            {
                MotorControl.SetAllMotorSpeed(0.0f);
                Slave.Write(new byte[] { 0x80 });
            });
            for (byte i = 0x9A; i <= 0x9C; i++)
            {
                Parse.SetParseHandler(i, (Packet) =>
                {
                    MotorControl.SetMotorSpeed(Packet.Data.ID - 0x9A, UtilData.ToFloat(Packet.Data.Payload));
                });

            }
            for (byte i = 0x9D; i <= 0xA0; i++)
            {
                Parse.SetParseHandler(i, (Pack) =>
                {
                    Slave.Write(new byte[] { Pack.Data.ID });
                    Slave.Write(Pack.Data.Payload);
                });
            }
        }

        static void Main(string[] args)
        {

            SetupBeaglebone();
            SetupPins();
            SetupClient();

            const int PacketSize = sizeof(byte) + sizeof(int) + sizeof(int) + sizeof(int) + sizeof(byte);
            Packet p = new Packet(0xD3, true);
            byte[] ID = new byte[1];
            p.Data.Payload = new byte[PacketSize - 1];
            for (; ; )
            {
                /*Slave.Read(1, ID);
                Slave.Read(PacketSize, p.Data.Payload);
                p.Data.ID = ID[0];
                Client.SendNow(p);*/
                string[] tokens = Console.ReadLine().Split(' ');
                byte id = Byte.Parse(tokens[0], System.Globalization.NumberStyles.HexNumber);
                float speed = float.Parse(tokens[1]);
                if (0x9A <= id && id <= 0x9C)
                    MotorControl.SetMotorSpeed(id - 0x9A, speed);
                else
                {
                    Slave.Write(new byte[] { id });
                    Slave.Write(UtilData.ToBytes(speed));
                }
            }

        }
    }
}
