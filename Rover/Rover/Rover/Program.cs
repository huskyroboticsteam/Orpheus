using Scarlet.Communications;
using Scarlet.IO.BeagleBone;
using Scarlet.Utilities;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Rover
{
    class Program
    {
        static CANBusBBB canName;

        static void Main(string[] args)
        {

            /*
            UdpClient udpServer = new UdpClient(9000);
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 9000);
            */

            //Server.Start(1025, 1026);

            BeagleBone.Initialize(SystemMode.DEFAULT, true);
            //canName = CANBBB.CANBus0;
            Server.Start(1025, 1026);
            Log.SetGlobalOutputLevel(Log.Severity.ERROR);
            Parse.SetParseHandler(0x30, messageParser);

            

            /*
            while (true)
            {

                //var data = udpServer.Receive(ref remoteEP);
                

                String dataString = Encoding.ASCII.GetString(data);

                Console.WriteLine(dataString);

                if (!String.IsNullOrEmpty(dataString))
                {

                    String[] speedDis = dataString.Split(',');
                    Double steer = Convert.ToDouble(speedDis[0]);
                    Double speed = Convert.ToDouble(speedDis[1]);

                    if (Math.Abs(speed) < 0.01)
                    {
                        speed = 0;
                    }

                    Console.WriteLine(steer + "," + speed);
                    
                    canName.Write(1, UtilData.ToBytes((int)(speed * 100000.0)));
                    canName.Write(2, UtilData.ToBytes((int)(speed * 100000.0)));
                    canName.Write(3, UtilData.ToBytes((int)(speed * 100000.0)));
                    canName.Write(4, UtilData.ToBytes((int)(-1*speed * 100000.0)));
                    canName.Write(5, UtilData.ToBytes((int)(0.15 * steer * 100000.0)));
                    Thread.Sleep(50);
                }

            }*/
        }

        public static void messageParser(Packet recivedData)
        {
            Console.WriteLine("Hello");
            string message = UtilData.ToString(recivedData.Data.Payload);
            Console.WriteLine(message);

            if (!String.IsNullOrEmpty(message))
            {

                String[] speedDis = message.Split(',');
                Console.WriteLine(message);
                Console.WriteLine(speedDis[0]);
                Console.WriteLine(speedDis[1]);
                Double steer = Convert.ToDouble(speedDis[0]);
                Double speed = Convert.ToDouble(speedDis[1]);

                if (Math.Abs(speed) < 0.05)
                {
                    speed = 0;
                }

                Console.WriteLine(steer + "," + speed);

                CANBBB.CANBus0.Write(1, UtilData.ToBytes((int)(speed * 100000.0)));
                CANBBB.CANBus0.Write(2, UtilData.ToBytes((int)(speed * 100000.0)));
                CANBBB.CANBus0.Write(3, UtilData.ToBytes((int)(speed * 100000.0)));
                CANBBB.CANBus0.Write(4, UtilData.ToBytes((int)(-1 * speed * 100000.0)));
                CANBBB.CANBus0.Write(5, UtilData.ToBytes((int)(0.1 * steer * 100000.0)));
            }

            return;
        }
    }
}
