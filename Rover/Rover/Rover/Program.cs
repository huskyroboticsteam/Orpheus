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

        static void Main(string[] args)
        {


            UdpClient udpServer = new UdpClient(9000);
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 9000);

            CANBusBBB canName = CANBBB.CANBus0;

            while (true)
            {

                var data = udpServer.Receive(ref remoteEP);
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

            }
        }
    }
}
