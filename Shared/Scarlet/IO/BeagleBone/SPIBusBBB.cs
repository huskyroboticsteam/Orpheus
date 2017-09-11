using BBBCSIO;
using System;

namespace Scarlet.IO.BeagleBone
{
    public static class SPIBBB
    {
        public static SPIBusBBB SPIBus0 { get; private set; }
        public static SPIBusBBB SPIBus1 { get; private set; }

        static internal void Initialize(bool Enable0, bool Enable1)
        {
            if (Enable0) { SPIBus0 = new SPIBusBBB(0); }
            if (Enable1) { SPIBus1 = new SPIBusBBB(1); }
        }
    }

    public class SPIBusBBB : ISPIBus
    {
        private SPIPortFS Port;

        internal SPIBusBBB(byte ID)
        {
            switch (ID)
            {
                case 0: this.Port = new SPIPortFS(SPIPortEnum.SPIPORT_0); break;
                case 1: this.Port = new SPIPortFS(SPIPortEnum.SPIPORT_1); break;
                default: throw new ArgumentOutOfRangeException("Only SPI ports 0 and 1 are supported.");
            }
        }

        public void Initialize()
        {

        }

        public byte[] Read(BBBPin DeviceSelect, int DataLength)
        { // TODO: Make this use IDigitalOut instead of BBBPin.
            this.Port
            throw new NotImplementedException();
        }

        public void Write(BBBPin DeviceSelect, byte[] Data, int DataLength)
        { // TODO: Make this use IDigitalOut instead of BBBPin.
            throw new NotImplementedException();
        }

        public void Dispose() { }
    }
}
