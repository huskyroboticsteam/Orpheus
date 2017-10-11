using BBBCSIO;
using System;

namespace Scarlet.IO.BeagleBone
{
    public class DigitalInBBB : IDigitalIn
    {
        public BBBPin Pin { get; private set; }
        private Port InputPort;

        private InterruptPortMM IntPortRise, IntPortFall, IntPortAny;
        private event EventHandler<InputInterrupt> RisingHandlers;
        private event EventHandler<InputInterrupt> FallingHandlers;
        private event EventHandler<InputInterrupt> AnyHandlers;

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
            switch (Type)
            {
                case InterruptType.RISING_EDGE:
                {
                    if (this.IntPortRise == null)
                    {
                        this.IntPortRise = new InterruptPortMM(IO.BeagleBone.Pin.PinToGPIO(this.Pin), InterruptMode.InterruptEdgeLevelHigh);
                        this.IntPortRise.EnableInterrupt();
                        this.IntPortRise.OnInterrupt += this.InterruptRising;
                    }
                    this.RisingHandlers += Handler;
                    return;
                }
                case InterruptType.FALLING_EDGE:
                {
                    if (this.IntPortFall == null)
                    {
                        this.IntPortFall = new InterruptPortMM(IO.BeagleBone.Pin.PinToGPIO(this.Pin), InterruptMode.InterruptEdgeLevelLow);
                        this.IntPortFall.EnableInterrupt();
                        this.IntPortFall.OnInterrupt += this.InterruptFalling;
                    }
                    this.FallingHandlers += Handler;
                    return;
                }
                case InterruptType.ANY_EDGE:
                {
                    if (this.IntPortAny == null)
                    {
                        this.IntPortAny = new InterruptPortMM(IO.BeagleBone.Pin.PinToGPIO(this.Pin), InterruptMode.InterruptEdgeBoth);
                        this.IntPortAny.EnableInterrupt();
                        this.IntPortAny.OnInterrupt += this.InterruptAny;
                    }
                    this.AnyHandlers += Handler;
                    return;
                }
            }
        }

        internal void InterruptRising(GpioEnum EventPin, bool EventState, DateTime Time, EventData Data)
        {
            if (EventPin == IO.BeagleBone.Pin.PinToGPIO(this.Pin))
            {
                InputInterrupt Event = new InputInterrupt(Data.EvState);
                this.RisingHandlers?.Invoke(this, Event);
            }
        }

        internal void InterruptFalling(GpioEnum EventPin, bool EventState, DateTime Time, EventData Data)
        {
            if (EventPin == IO.BeagleBone.Pin.PinToGPIO(this.Pin))
            {
                InputInterrupt Event = new InputInterrupt(Data.EvState);
                this.FallingHandlers?.Invoke(this, Event);
            }
        }

        internal void InterruptAny(GpioEnum EventPin, bool EventState, DateTime Time, EventData Data)
        {
            if (EventPin == IO.BeagleBone.Pin.PinToGPIO(this.Pin))
            {
                InputInterrupt Event = new InputInterrupt(Data.EvState);
                this.AnyHandlers?.Invoke(this, Event);
            }
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
            if (this.IntPortRise != null)
            {
                this.IntPortRise.DisableInterrupt();
                this.IntPortRise.ClosePort();
                this.IntPortRise.Dispose();
                this.IntPortRise = null;
            }
            if (this.IntPortFall != null)
            {
                this.IntPortFall.DisableInterrupt();
                this.IntPortFall.ClosePort();
                this.IntPortFall.Dispose();
                this.IntPortFall = null;
            }
            if (this.IntPortAny != null)
            {
                this.IntPortAny.DisableInterrupt();
                this.IntPortAny.ClosePort();
                this.IntPortAny.Dispose();
                this.IntPortAny = null;
            }
        }
    }
}
