using BBBCSIO;
using System;

namespace Scarlet.IO.BeagleBone
{
    public static class I2CBBB
    {
        public static I2CBusBBB I2CBus1 { get; private set; }
        public static I2CBusBBB I2CBus2 { get; private set; }

        static internal void Initialize(bool Enable1, bool Enable2)
        {
            if (Enable1) { I2CBus1 = new I2CBusBBB(1); }
            if (Enable2) { I2CBus2 = new I2CBusBBB(2); }
        }
    }

    public class I2CBusBBB : II2CBus
    {
        private I2CPortFS Port;

        internal I2CBusBBB(byte ID)
        {
            switch (ID)
            {
                case 1: this.Port = new I2CPortFS(I2CPortEnum.I2CPORT_1); break;
                case 2: this.Port = new I2CPortFS(I2CPortEnum.I2CPORT_2); break;
                default: throw new ArgumentOutOfRangeException("Only I2C ports 1 and 2 are supported.");
            }
        }

        public void Initialize() { }

        public void Write(byte Address, byte[] Data, int DataLength)
        {
            this.Port.Write(Address, Data, DataLength);
        }

        public byte[] Read(byte Address, int DataLength)
        {
            byte[] Buffer = new byte[DataLength];
            this.Port.Read(Address, Buffer, DataLength);
            return Buffer;
        }

        public void Dispose() { } // TODO: Implement.
    }
}
