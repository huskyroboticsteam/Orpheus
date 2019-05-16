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

            float Lat = 47.655098f;
            float Long = -122.308664f;

            byte[] blat = BitConverter.GetBytes(Lat);
            byte[] blong = BitConverter.GetBytes(Long);

            Console.WriteLine(blat.Length);
            Console.WriteLine(blong.Length);


            Thread Rec = new Thread(new ThreadStart(RecieverThread));
            Rec.Start();

            var client = new UdpClient();
            // IP and port for Rover Beaglebone
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("192.168.0.20"), 2001);
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

                for (int j =0; j < 50; j++)
                {
                    sendBytes[0] = 0;
                    sendBytes[1] = speedarray[0];
                    sendBytes[2] = speedarray[1];
                    sendBytes[3] = headarray[0];
                    sendBytes[4] = headarray[1];

                    client.Send(sendBytes, sendBytes.Length);

                    Console.Write("Data Sent:  ");
                    for (int i = 0; i < sendBytes.Length; i++)
                    {
                        Console.Write(sendBytes[i] + " ");
                    }
                    Thread.Sleep(50);
                }               

                Console.WriteLine();
                Console.WriteLine();
                Thread.Sleep(50);
            }
        }

        static void RecieverThread()
        {
            UdpClient udpServer = new UdpClient(2002); // UDP Port from RaspberryPi
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 2002);

            while (true)
            {
                Console.WriteLine("waiting for packet");
                var recieveByte = udpServer.Receive(ref remoteEP);
                float lon = 0f;
                float lat = 0f;
                float dir = 0f;

                Byte[] lonArray = new Byte[4];
                lonArray[0] = recieveByte[0];
                lonArray[1] = recieveByte[1];
                lonArray[2] = recieveByte[2];
                lonArray[3] = recieveByte[3];
                lon = System.BitConverter.ToSingle(lonArray, 0);

                Byte[] latArray = new Byte[4];
                lonArray[0] = recieveByte[4];
                lonArray[1] = recieveByte[5];
                lonArray[2] = recieveByte[6];
                lonArray[3] = recieveByte[7];
                lat = System.BitConverter.ToSingle(latArray, 0);

                Byte[] dirArray = new Byte[4];
                dirArray[0] = recieveByte[8];
                dirArray[1] = recieveByte[9];
                dirArray[2] = recieveByte[10];
                dirArray[3] = recieveByte[11];
                dir = System.BitConverter.ToSingle(latArray, 0);

                Console.WriteLine("Lon: " + lon + " Lat: " + lat + " Heading: " + dir);
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
