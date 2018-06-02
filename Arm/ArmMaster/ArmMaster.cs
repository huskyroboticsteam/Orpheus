using System;
using System.Collections.Generic;
using Scarlet.Utilities;
using Scarlet.Communications;
using Scarlet.IO.BeagleBone;
using Scarlet.IO;

namespace ArmMaster
{
    public class ArmMaster
    {
        const string CHASSIS_BEAGLEBONE = "192.168.0.5";
        public static void Main(string[] args)
        {
            StateStore.Start("ArmMaster");
            BeagleBone.Initialize(SystemMode.DEFAULT, true);
            BBBPinManager.AddMappingUART(BBBPin.P9_24);
            BBBPinManager.AddMappingUART(BBBPin.P9_26);
            BBBPinManager.ApplyPinSettings(BBBPinManager.ApplicationMode.APPLY_IF_NONE);
            Client.Start(CHASSIS_BEAGLEBONE, 1025, 1026, "ArmMaster");
            IUARTBus Slave = UARTBBB.UARTBus1;
            Parse.SetParseHandler(0x80, (Pack) =>
            {
                Slave.Write(new byte[] { 0x80 });
            });
            for (byte i = 0x9A; i <= 0xA0; i++)
            {
                Parse.SetParseHandler(i, (Pack) =>
                {
                    Slave.Write(new byte[] { i });
                    Slave.Write(Pack.Data.Payload);
                });
            }
            const int PacketSize = sizeof(byte) + sizeof(int) + sizeof(int) + sizeof(int) + sizeof(byte);
            Packet p = new Packet(0xD3, true);
            byte[] ID = new byte[1];
            p.Data.Payload = new byte[PacketSize - 1];
            for (; ; )
            {
                Slave.Read(1, ID);
                Slave.Read(PacketSize, p.Data.Payload);
                p.Data.ID = ID[0];
                Client.SendNow(p);
            }

        }
    }
}
