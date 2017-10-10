using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scarlet.IO.RaspberryPi
{
    public class I2CBusPi : II2CBus
    {
        
        public I2CBusPi(int Bus)
        {
            RaspberryPi.I2CSetup((byte)Bus);
        }

        public byte[] Read(byte Address, int DataLength)
        {
            byte[] Buffer = new byte[DataLength];
            for (int i = 0; i < DataLength; i++)
            {
                Buffer[i] = RaspberryPi.I2CRead(Address);
            }
            return Buffer;
        }

        public byte[] ReadRegister(byte Address, byte Register, int DataLength)
        {
            Write(Address, new byte[] { Register });
            return Read(Address, DataLength);
        }

        public void Write(byte Address, byte[] Data)
        {
            foreach (byte Byte in Data) { RaspberryPi.I2CWrite(Address, Byte); }
        }

        public void WriteRegister(byte Address, byte Register, byte[] Data)
        {
            byte[] NewData = new byte[Data.Length + 1];
            NewData[0] = Register;
            for(int i = 1; i < NewData.Length; i++) { NewData[i] = Data[i - 1]; }
        }

        public void Dispose()
        {
            // TODO: Implement this
            throw new NotImplementedException();
        }
    }
}
