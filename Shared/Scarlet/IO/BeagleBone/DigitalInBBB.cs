using BBBCSIO;
using System;

namespace Scarlet.IO.BeagleBone
{
    public class DigitalInBBB : IDigitalIn
    {
        public BBBPin Pin { get; private set; }
        private Port InputPort;

        public bool GetInput()
        {
            if (BeagleBone.Fast) { return ((InputPortMM)this.InputPort).Read(); }
            else { return ((InputPortFS)this.InputPort).Read(); }
        }

        public void Initialize()
        {
            if (BeagleBone.Fast) { this.InputPort = new InputPortMM(IO.BeagleBone.Pin.PinToGPIO(this.Pin)); }
            else { this.InputPort = new InputPortFS(IO.BeagleBone.Pin.PinToGPIO(this.Pin)); }
        }

        public void RegisterInterruptHandler(EventHandler<InputInterrupt> Handler, InterruptType Type)
        {
            throw new NotImplementedException();
        }

        public void SetResistor(ResistorState Resistor)
        {
            throw new NotImplementedException();
            // TODO: Does this actually require device tree changes? O.o
        }

        public void Dispose()
        {
            if(BeagleBone.Fast)
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
