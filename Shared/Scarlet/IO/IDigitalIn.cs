
using System;

namespace Scarlet.IO
{
    public interface IDigitalIn
    {
        /// <summary>
        /// Prepares the output for use.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Sets the output to on or off.
        /// </summary>
        void SetResistor(ResistorState Resistor);

        /// <summary>
        /// Gets the current input.
        /// </summary>
        /// <returns></returns>
        bool GetInput();

        /// <summary>
        /// Releases handles to the pin, allowing it to be used by another component or application.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Registers a new interrupt handler, asking for the specificed type of interrupt.
        /// </summary>
        void RegisterInterruptHandler(EventHandler<InputInterrupt> Handler, InterruptType Type);
    }
}
