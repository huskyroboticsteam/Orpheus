using BBBCSIO;
using System;

namespace Scarlet.IO.BeagleBone
{
    public class DigitalInBBB : IDigitalIn
    {
        public BBBPin Pin { get; private set; }
        private Port InputPort;

        public DigitalInBBB(BBBPin Pin)
        {
            this.Pin = Pin;
            if (BeagleBone.FastGPIO) { this.InputPort = new InputPortMM(IO.BeagleBone.Pin.PinToGPIO(this.Pin)); }
            else { this.InputPort = new InputPortFS(IO.BeagleBone.Pin.PinToGPIO(this.Pin)); }
        }

        public bool GetInput()
        {
            if (BeagleBone.FastGPIO) { return ((InputPortMM)this.InputPort).Read(); }
            else { return ((InputPortFS)this.InputPort).Read(); }
        }

        public void RegisterInterruptHandler(EventHandler<InputInterrupt> Handler, InterruptType Type)
        {
            throw new NotImplementedException();
        }

        public void SetResistor(ResistorState Resistor)
        {
            throw new NotImplementedException("Resistor state cannot be changed on BBB without re-applying device tree overlay.");
        }

        public void Dispose()
        {
            if(BeagleBone.FastGPIO)
            {
                ((InputPortMM)this.InputPort).ClosePort();
                ((InputPortMM)this.InputPort).Dispose();
            }
            else
            {
                ((InputPortFS)this.InputPort).ClosePort();
                ((InputPortFS)this.InputPort).Dispose();
            }
        }
    }
}
