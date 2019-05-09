using Scarlet.Communications;
using Scarlet.Components.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
                testSending();
            }
            //enter nothing for server
            else
            {
                testRecieve();                
            }
        }

        static void testRecieve()
        {
            UdpClient udpServer = new UdpClient(8000); 
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 8000);
            

            Console.WriteLine("Sever Recieving Loop Started");
            
            while (true)
            {
                var data = udpServer.Receive(ref remoteEP);
                var stringData = Encoding.ASCII.GetString(data);
                Console.WriteLine("Recieving data: " + stringData);
            } 
        }            

        static void testSending()
        {
            var client = new UdpClient();
            // IP and port for Rover Beaglebone
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IP), 8000);
            client.Connect(ep);

            int count = 0;
            Byte[] sendBytes;
            Console.WriteLine("Client Recieve Loop Started");

            while (true)
            {                
                string stringData = count.ToString();
                Console.WriteLine("Sending data: " + stringData);

                sendBytes = Encoding.ASCII.GetBytes(stringData);
                client.Send(sendBytes, sendBytes.Length);
                Thread.Sleep(50);
                count++;                
                if (count == 256)
                {
                    count = 0;
                }
            }

        }
    }
}
