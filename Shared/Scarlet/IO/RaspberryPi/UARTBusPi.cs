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
            if (Device == 0)
            {
                RaspberryPi.SetPinMode(8, RaspberryPi.PinMode.OUTPUT);
                RaspberryPi.SetPinMode(10, RaspberryPi.PinMode.INPUT);
            }
            this.DeviceID = RaspberryPi.SerialOpen(Device, Baud);
        }

        public byte[] Read(int Length)
        {
            List<byte> Data = new List<byte>();
            int Avail = RaspberryPi.SerialDataAvailable(this.DeviceID);
            if (Avail < Length) { Length = Avail; }
            for (int i = 0; i < Length; i++)
            {
                Data.Add(RaspberryPi.SerialGetChar(this.DeviceID));
            }
            return Data.ToArray();
        }

        public void Write(byte[] Data) { RaspberryPi.SerialPut(this.DeviceID, Data); }

        public void Dispose() { throw new NotImplementedException(); }

    }
}
