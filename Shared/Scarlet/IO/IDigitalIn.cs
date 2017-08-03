using System;

namespace Scarlet.IO
{
    public interface IDigitalIn
    {
        /// <summary> Prepares the input for use. </summary>
        void Initialize();

        /// <summary> Sets the input resistor. </summary>
        void SetResistor(ResistorState Resistor);

        /// <summary> Gets the current input state. </summary>
        bool GetInput();

        /// <summary> Releases handles to the pin, allowing it to be used by another component or application. </summary>
        void Dispose();

        /// <summary> Registers a new interrupt handler, asking for the specificed type of interrupt. </summary>
        void RegisterInterruptHandler(EventHandler<InputInterrupt> Handler, InterruptType Type);
    }
}
