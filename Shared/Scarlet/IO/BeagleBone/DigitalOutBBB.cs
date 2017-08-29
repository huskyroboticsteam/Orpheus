using BBBCSIO;
using System;

namespace Scarlet.IO.BeagleBone
{
    public class DigitalOutBBB : IDigitalOut
    {
        public BBBPin Pin { get; private set; }
        private Port OutputPort;

        public DigitalOutBBB(BBBPin Pin)
        {
            if (!IO.BeagleBone.Pin.CheckPin(Pin, BeagleBone.Peripherals)) { throw new ArgumentOutOfRangeException("Cannot use pin " + Enum.GetName(typeof(BBBPin), Pin) + " in current peripheral mode."); }
            this.Pin = Pin;
        }

        /// <summary>Preapres the pin for use.</summary>
        public void Initialize()
        {
            if (BeagleBone.FastGPIO) { this.OutputPort = new OutputPortMM(IO.BeagleBone.Pin.PinToGPIO(this.Pin)); }
            else { this.OutputPort = new OutputPortFS(IO.BeagleBone.Pin.PinToGPIO(this.Pin)); }
        }

        /// <summary>Sets the logic-level ouput.</summary>
        public void SetOutput(bool Output)
        {
            if (BeagleBone.FastGPIO) { ((OutputPortMM)this.OutputPort).Write(Output); }
            else { ((OutputPortFS)this.OutputPort).Write(Output); }
        }

        /// <summary>Cleans up the pin for future use by other software.</summary>
        public void Dispose()
        {
            if(BeagleBone.FastGPIO)
            {
                ((OutputPortMM)this.OutputPort).ClosePort();
                ((OutputPortMM)this.OutputPort).Dispose();
            }
            else
            {
                ((OutputPortFS)this.OutputPort).ClosePort();
                ((OutputPortFS)this.OutputPort).Dispose();
            }
        }
    }
}
