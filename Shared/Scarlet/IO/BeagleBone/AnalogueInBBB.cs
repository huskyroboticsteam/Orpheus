using BBBCSIO;
using Scarlet.Utilities;
using System;
using System.IO;
using System.Threading;

namespace Scarlet.IO.BeagleBone
{
    public class AnalogueInBBB : IAnalogueIn
    {
        public BBBPin Pin { get; private set; }
        private A2DPortFS Port;

        public AnalogueInBBB(BBBPin Pin) { this.Pin = Pin; }
        public void Initialize()
        {
            // The ADC input file is not always available immediately, it can take a bit for it to be available after device tree overaly application.
            // Therefore, we will wait for it for up to 5 seconds, then throw an error if it is not ready at that time.
            string Filename = "/sys/bus/iio/devices/iio:device0/in_voltage";
            switch(this.Pin)
            {
                case BBBPin.P9_39: Filename += 0; break;
                case BBBPin.P9_40: Filename += 1; break;
                case BBBPin.P9_37: Filename += 2; break;
                case BBBPin.P9_38: Filename += 3; break;
                case BBBPin.P9_33: Filename += 4; break;
                case BBBPin.P9_36: Filename += 5; break;
                case BBBPin.P9_35: Filename += 6; break;
                default: throw new Exception("Given pin is not a valid ADC pin!");
            }
            Filename += "_raw";
            DateTime Timeout = DateTime.Now.Add(TimeSpan.FromSeconds(5));
            while(!File.Exists(Filename))
            {
                if (DateTime.Now > Timeout)
                {
                    Log.Output(Log.Severity.ERROR, Log.Source.HARDWAREIO, "ADC Failed to initialize.");
                    throw new TimeoutException("ADC failed to initialize within the timeout period.");
                }
                Thread.Sleep(50);
            }
            this.Port = new A2DPortFS(IO.BeagleBone.Pin.PinToA2D(this.Pin));
        }

        public long GetRawRange() { return 4096; } // This may change depending on device tree settings.
        public double GetRange() { return 1.8D; }

        public double GetInput() { return (double)GetRawInput() * GetRange() / (double)(GetRawRange() - 1); }
        public long GetRawInput() { return this.Port.Read(); }

        public void Dispose() { this.Port.Dispose(); }
    }
}
