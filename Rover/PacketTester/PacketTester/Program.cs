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

            Byte[] sendBytes = new Byte[5];

            Console.WriteLine("Loop Started");
            while (true)
            {
                Console.Write("Enter desired speed (-127 to 128) ");
                string speed = Console.ReadLine();
                short sspeed = Convert.ToInt16(speed);
                Byte[] speedarray = BitConverter.GetBytes(sspeed);

                Console.Write("Enter desired heading (0 to 360) ");
                string head = Console.ReadLine();
                short shead = Convert.ToInt16(head);
                Byte[] headarray = BitConverter.GetBytes(shead);

                sendBytes[0] = 0;
                sendBytes[1] = speedarray[1];
                sendBytes[2] = speedarray[0];
                sendBytes[3] = headarray[1];
                sendBytes[4] = headarray[0];

                client.Send(sendBytes, sendBytes.Length);

                Console.Write("Data Sent:  ");
                for (int i = 0; i < sendBytes.Length; i++)
                {
                    Console.Write(sendBytes[i] + " ");
                }

                Console.WriteLine();
                Console.WriteLine();
                Thread.Sleep(50);
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
