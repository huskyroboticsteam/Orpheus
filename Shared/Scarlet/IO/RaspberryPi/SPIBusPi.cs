using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scarlet.IO.RaspberryPi
{
    public class SPIBusPi : ISPIBus
    {
        private const int DEFAULT_SPEED = 50000;

        private int BusNum;

        public SPIBusPi(int Bus)
        {
            BusNum = Bus;
            RaspberryPi.SPISetup(Bus, DEFAULT_SPEED);
        }

        public void SetBusSpeed(int Speed)
        {
            RaspberryPi.SPISetup(BusNum, Speed);
        }

        public byte[] Write(IDigitalOut DeviceSelect, byte[] Data, int DataLength)
        {
            DeviceSelect.SetOutput(false);
            byte[] DataReturn = RaspberryPi.SPIRW(BusNum, Data, DataLength);
            DeviceSelect.SetOutput(true);
            return DataReturn;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
