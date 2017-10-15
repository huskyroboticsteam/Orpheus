using System;

namespace Scarlet.IO.RaspberryPi
{
    public class DigitalInPi : IDigitalIn
    {
        public int PinNumber { get; private set; }
        private bool RisingInit, FallingInit, AnyInit;
        private event EventHandler<InputInterrupt> RisingHandlers;
        private event EventHandler<InputInterrupt> FallingHandlers;
        private event EventHandler<InputInterrupt> AnyHandlers;

        public DigitalInPi(int PinNumber)
        {
            this.PinNumber = PinNumber;
            RaspberryPi.SetPinMode(this.PinNumber, (int)RaspberryPi.PinMode.INPUT);
        }

        /// <summary>
        /// Sets the input to either have a ~50KΩ pullup resistor, pulldown resistor, or no resistor.
        /// </summary>
        public void SetResistor(ResistorState Resistor)
        {
            RaspberryPi.SetResistor(this.PinNumber, Resistor);
        }

        /// <summary>
        /// Gets the current logic-level input.
        /// </summary>
        public bool GetInput()
        {
            return RaspberryPi.DigitalRead(this.PinNumber);
        }

        /// <summary>
        /// Registers an interrupt handler for the specified interrupt type.
        /// </summary>
        public void RegisterInterruptHandler(EventHandler<InputInterrupt> Handler, InterruptType Type)
        {
            switch (Type)
            {
                case InterruptType.RISING_EDGE:
                {
                    if(!this.RisingInit)
                    {
                        RaspberryPi.AddInterrupt(this.PinNumber, 2, this.InterruptRising);
                        this.RisingInit = true;
                    }
                    this.RisingHandlers += Handler;
                    return;
                }
                case InterruptType.FALLING_EDGE:
                {
                    if (!this.FallingInit)
                    {
                        RaspberryPi.AddInterrupt(this.PinNumber, 1, this.InterruptFalling);
                        this.FallingInit = true;
                    }
                    this.FallingHandlers += Handler;
                    return;
                }
                case InterruptType.ANY_EDGE:
                {
                    if (!this.AnyInit)
                    {
                        RaspberryPi.AddInterrupt(this.PinNumber, 3, this.InterruptAny);
                        this.AnyInit = true;
                    }
                    this.AnyHandlers += Handler;
                    return;
                }
            }
        }

        internal void InterruptRising()
        {
            InputInterrupt Event = new InputInterrupt(GetInput());
            this.RisingHandlers?.Invoke(this, Event);
        }

        internal void InterruptFalling()
        {
            InputInterrupt Event = new InputInterrupt(GetInput());
            this.FallingHandlers?.Invoke(this, Event);
        }

        internal void InterruptAny()
        {
            InputInterrupt Event = new InputInterrupt(GetInput());
            this.AnyHandlers?.Invoke(this, Event);
        }

        /// <summary>
        /// Does nothing currently. You should still call this when finished in case it does gain functionality later on.
        /// </summary>
        public void Dispose()
        {
            // TODO: Do we need to do anything here?
        }
    }
}
