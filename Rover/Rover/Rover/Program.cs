using Scarlet.Communications;
using Scarlet.Components.Motors;
using Scarlet.Filters;
using Scarlet.IO;
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
        //static CANBusBBB canName;

        static ICANBus iCAN = CANBBB.CANBus0;
        static IFilter<float> filter = new LowPass<float>(0.75);
        static CANVESC Motor1 = new CANVESC(iCAN, 1, 1.0f, filter);
        static CANVESC Motor2 = new CANVESC(iCAN, 2, 1.0f, filter);
        static CANVESC Motor3 = new CANVESC(iCAN, 3, 1.0f, filter);
        static CANVESC Motor4 = new CANVESC(iCAN, 4, 1.0f, filter);
        static CANVESC Turn = new CANVESC(iCAN, 5, 1.0f, filter);

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

                /*
                CANBBB.CANBus0.Write(1, UtilData.ToBytes((int)(speed * 100000.0)));
                CANBBB.CANBus0.Write(2, UtilData.ToBytes((int)(speed * 100000.0)));
                CANBBB.CANBus0.Write(3, UtilData.ToBytes((int)(speed * 100000.0)));
                CANBBB.CANBus0.Write(4, UtilData.ToBytes((int)(-1 * speed * 100000.0)));
                CANBBB.CANBus0.Write(5, UtilData.ToBytes((int)(0.1 * steer * 100000.0)));
                */

                Motor1.SetSpeed((float)speed);
                Motor2.SetSpeed((float)speed);
                Motor3.SetSpeed((float)speed);
                Motor4.SetSpeed((float)(-1*speed));
                Turn.SetSpeed((float)(steer*0.25));

            }

            return;
        }
    }
}
