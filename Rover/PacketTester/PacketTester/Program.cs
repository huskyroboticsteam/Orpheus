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
           // UdpClient udpServer = new UdpClient(2001); // UDP Port from RaspberryPi
            //IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 2001);

            var client = new UdpClient();
            // IP and port for Rover Beaglebone
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("192.168.0.22"), 2001);
            client.Connect(ep);

            Byte[] sendBytes;

            int count = 0;
            Console.WriteLine("Loop Started");
            while (true)
            {
                Console.WriteLine("Sending Data: " + count);
                sendBytes = Encoding.ASCII.GetBytes(count.ToString());
                client.Send(sendBytes, sendBytes.Length);
                Thread.Sleep(50);
                count++;
                if (count == 127)
                {
                    count = -127;
                }
            }
        }

        static void testRecieve()
        {
            UdpClient udpServer = new UdpClient(2001); 
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 2001);
            

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
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IP), 2002);
            client.Connect(ep);

            int count = 0;
            Byte[] sendBytes;
            Console.WriteLine("Client Recieve Loop Started");

            while (true)
            {                
                string stringData = count.ToString();
                Console.WriteLine("Sending data: " + stringData);

                sendBytes = Encoding.ASCII.GetBytes(count.ToString());
                client.Send(sendBytes, sendBytes.Length);
                Thread.Sleep(50);
                count++;                
                if (count == 256)
                {
                    count = 0;
                }
            }

        }

        static void legacy()
        {
            bool SendOnly = false;
            //if (args != null && args.Length > 0)
            //{   
            //    SendOnly = true;
            //    Console.WriteLine("Only sending data");
            //}
            UdpClient udpServer = new UdpClient(2001); // UDP Port from RaspberryPi
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 2001);

            var client = new UdpClient();
            // IP and port for Rover Beaglebone
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("192.168.0.20"), 2002);
            client.Connect(ep);

            Byte[] sendBytes;

            int count = 0;
            Console.WriteLine("Loop Started");
            while (true)
            {
                if (!SendOnly)
                {
                    var data = udpServer.Receive(ref remoteEP);
                    var stringData = Encoding.ASCII.GetString(data);
                    Console.Write("Recieved Data: ");
                    for (int i = 0; i < data.Length; i++)
                    {
                        Console.Write(data[i] + " ");
                    }
                    Console.WriteLine();
                }

                Console.WriteLine("Sending Data: " + count);
                sendBytes = Encoding.ASCII.GetBytes(count.ToString());
                client.Send(sendBytes, sendBytes.Length);
                Thread.Sleep(50);
                count++;
                if (count == 256)
                {
                    count = 0;
                }
            }
            //enter ip for client
            /*
            if (args != null && args.Length > 0)
            {
                IP = args[0];
                testSending();
            }
            //enter nothing for server
            else
            {
                testRecieve();                
            } */
        }
    }
}
