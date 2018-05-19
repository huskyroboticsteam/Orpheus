
using Scarlet.IO.BeagleBone;
using Scarlet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace CanTester
{
    class Program
    {
        static void Main(string[] args)
        {

            CANBusBBB canName = CANBBB.CANBus0;

            Console.Write("Enter a can ID: ");
            uint canID = Convert.ToUInt32(Console.ReadLine());   
            // uint canID = 0xFF;
            while (true)
            {
                Console.Write("Enter a speed value from -1 to 1: ");
                double speed  =  Convert.ToDouble(Console.ReadLine());
                canName.Write(canID, UtilData.ToBytes((int)(speed * 100000.0)));


                /* // Working Code
                List<byte> payload = new List<byte>();
                byte[] p = new byte[] { 0x4E, 0x20, 0, 0 };
                canName.Write(canID, p);
                Console.WriteLine("Sent!");
                Thread.Sleep(500);
                */ 
                
            }
        }
    }
}
