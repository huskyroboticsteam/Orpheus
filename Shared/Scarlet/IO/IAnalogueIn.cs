namespace Scarlet.IO
{
    public interface IAnalogueIn
    {
        /// <summary> Gets the current input level in Volts. </summary>
        double GetInput();

        /// <summary> Gets the maximum input level in Volts. </summary>
        double GetRange();

        /// <summary> Gets the current input level as a raw value. </summary>
        long GetRawInput();

        /// <summary> Gets the maximum input level as a raw value. This is likely 2^(ADC Bits). </summary>
        long GetRawRange();

        /// <summary> Releases handles to the pin, allowing it to be used by another component or application. </summary>
        void Dispose();
    }
}
