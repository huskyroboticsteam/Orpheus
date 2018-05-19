
using Scarlet.IO.BeagleBone;
using Scarlet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CanTester
{
    class Program
    {
        static void Main(string[] args)
        {

            CANBusBBB canName = CANBBB.CANBus0;
            var data = new byte[5];

            Console.Write("Enter a can ID: ");
            UInt64 canID = Convert.ToUInt64(Console.ReadLine());   

            while (true)
            {
                Console.Write("Enter a speed value from -1 to 1: ");
                double speed  =  Convert.ToDouble(Console.ReadLine());
                //data[0] = UtilData.ToBytes((Int32)(Speed * 100000.0));
                //data[0] = UtilData.ToBytes((int)(speed * 100000));
                List<byte> payload = new List<byte>();
                //payload.Add(5);
                //payload.AddRange(UtilData.ToBytes((float)(speed * 100000.0)));
                //canName.Write((uint) canID, payload.ToArray());
                canName.Write((uint)canID, UtilData.ToBytes((float)(speed * 100000.0)));

            }
        }
    }
}
