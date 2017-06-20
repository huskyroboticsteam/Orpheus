namespace Scarlet.IO.RaspberryPi
{
    public class DigitalInPi : IDigitalIn
    {
        private int PinNumber;

        public DigitalInPi(int PinNumber)
        {
            this.PinNumber = PinNumber;
        }

        /// <summary>
        /// Prepares the GPIO pin for input use. Make sure to call RaspberryPi.SetupGPIO() once in your program before this.
        /// </summary>
        public void Initialize()
        {
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
        /// Does nothing currently. You should still call this when finished in case it does gain functionality later on.
        /// </summary>
        public void Dispose()
        {
            // TODO: Do we need to do anything here?
        }
    }
}
