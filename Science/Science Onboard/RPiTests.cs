using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Scarlet.Components.Sensors;
using Scarlet.IO;
using Scarlet.IO.RaspberryPi;
using Scarlet.Utilities;

namespace Science
{
    class RPiTests
    {
        internal static void TestSPI()
        {
            IDigitalOut CS_Thermo = new DigitalOutPi(7);
            MAX31855 Thermo = new MAX31855(new SPIBusPi(0), CS_Thermo);
            Log.SetSingleOutputLevel(Log.Source.SENSORS, Log.Severity.DEBUG);
            for (int i = 0; i < 100; i++)
            {
                Thermo.UpdateState();
                Log.Output(Log.Severity.DEBUG, Log.Source.SENSORS, "Thermocouple Data, Faults: " + string.Format("{0:G}", Thermo.GetFaults()) + ", Internal: " + Thermo.GetInternalTemp() + ", External: " + Thermo.GetExternalTemp() + " (Raw: " + Thermo.GetRawData() + ")");
                Thread.Sleep(500);
            }
        }

        internal static void TestUART()
        {
            IUARTBus UART = new UARTBusPi(0, 9600);
            for (byte i = 0; i < 255; i++)
            {
                UART.Write(new byte[] { i });
                Thread.Sleep(250);
            }
        }
    }
}
