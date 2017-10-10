using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scarlet.IO.RaspberryPi
{
    public class UARTBusPi : IUARTBus
    {
        private int DeviceID;

        public UARTBusPi(byte Device, int Baud)
        {
            DeviceID = RaspberryPi.SerialOpen(Device, Baud);
        }

        public byte[] Read(int Length)
        {
            List<byte> Data = new List<byte>();
            int Avail = RaspberryPi.SerialDataAvailable(DeviceID);
            if (Avail < Length) { Length = Avail; }
            for (int i = 0; i < Length; i++)
            {
                Data.Add(RaspberryPi.SerialGetChar(DeviceID));
            }
            return Data.ToArray();
        }

        public void Write(byte[] Data) { RaspberryPi.SerialPut(DeviceID, Data); }

        public void Dispose() { throw new NotImplementedException(); }

    }
}
