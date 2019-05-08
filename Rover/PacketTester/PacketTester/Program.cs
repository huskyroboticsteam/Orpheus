using Scarlet.Communications;
using Scarlet.Components.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PacketTester
{
    class Program
    {
        public static string IP = "192.168.0.20";

        static void Main(string[] args)
        {
            //enter ip for client
            if (args != null && args.Length > 0)
            {
                IP = args[0];
                testRecieve();
            }
            //enter nothing for server
            else
            {
                testSending();
            }
        }

        static void testSending()
        {
            //Scarlet.Components.Sensors.BNO055 Mag;
            Server.Start(1025, 1026, OperationPeriod: 5);
            Thread.Sleep(1500);
            byte id = 0;
            while (true)
            {
                Thread.Sleep(50);
                Packet output = new Packet(0x99, true, "MainRover");
                byte[] test = { id };
                output.AppendData(test);
                Server.Send(output);
                id++;
            }
        }            

        static void testRecieve()
        {
            try
            {                
                Client.Start(IP, 1025, 1026, "Mobile", OperationPeriod: 5);
                Console.WriteLine("Using IP of " + IP);
            }
            catch
            {
                Console.WriteLine("Invaid IP adress using default 192.168.0.25");
                Client.Start("192.168.0.25", 1025, 1026, "Mobile", OperationPeriod: 5);
            }
            Thread.Sleep(1500);
            QueueBuffer Packets = new QueueBuffer();
            for (byte i = 0x99; i <= 0x99; i++)
                Parse.SetParseHandler(i, (Packet) => Packets.Enqueue(Packet, 0));

            while (true)
            {
                const int NUM_PACKETS_TO_PROCESS = 20;
                for (int i = 0; !Packets.IsEmpty() && i < NUM_PACKETS_TO_PROCESS; i++)
                {
                    Packet p = Packets.Dequeue();
                    Console.WriteLine("data is " + (sbyte)p.Data.Payload[0]);
                }
            }
            
        }
    }
}
