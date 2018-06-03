using System;
using System.Collections.Generic;
using Scarlet.Utilities;
using Scarlet.Communications;
using Scarlet.IO.BeagleBone;
using Scarlet.IO;
using System.Threading;

namespace ArmMaster
{
    class ArmMaster
    {
        const string CHASSIS_BEAGLEBONE = "192.168.0.5";
        static IUARTBus Slave;

        static void SetupBeagleboneAndPins()
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
            Slave = UARTBBB.UARTBus1;
            BeagleBone.Initialize(SystemMode.DEFAULT, true);
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
            StateStore.Start("ArmMaster");
            SetupBeagleboneAndPins();
            SetupClient();
            MotorControl.Initialize();
            const int PacketSize = sizeof(byte) + sizeof(int) + sizeof(int) + sizeof(int) + sizeof(byte);
            Packet p = new Packet(0xD3, true);
            byte[] ID = new byte[1];
            p.Data.Payload = new byte[PacketSize - 1];
            Thread t = new Thread(() =>
            {
                for (; ; )
                {
                    if (Slave.BytesAvailable() > 0)
                    {
                        Slave.Read(1, ID);
                        while (Slave.BytesAvailable() < PacketSize - 1) { }
                        Slave.Read(PacketSize, p.Data.Payload);

                        List<object> fields = UtilData.ToTypes(p.Data.Payload, typeof(int), typeof(int), typeof(int), typeof(byte));
                        Console.WriteLine($"{ ID[0] }: { (int)fields[0] }, { (int)fields[1] }, { (int)fields[2] }, { (byte)fields[3] }");
                    }
                }
            });
            t.Start();
            for (; ; )
            {
                /*Slave.Read(1, ID);
                Slave.Read(PacketSize, p.Data.Payload);
                p.Data.ID = ID[0];
                Client.SendNow(p);*/
                string @in = Console.ReadLine();
                if (@in == "quit")
                    break;
                if (@in == "s")
                {
                    Slave.Write(new byte[] { 0x80 });
                    MotorControl.EmergencyStop();
                }
                else
                {

                    string[] tokens = @in.Split(' ');
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
}
