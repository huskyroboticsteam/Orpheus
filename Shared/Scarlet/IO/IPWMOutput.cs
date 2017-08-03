namespace Scarlet.IO
{
    public interface IPWMOutput
    {
        /// <summary>
        /// Prepares the output for use.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Sets the PWM Frequency.
        /// </summary>
        /// <param name="Frequency">The output frequency in Hz.</param>
        void SetFrequency(int Frequency);

        /// <summary>
        /// Sets the output duty cycle.
        /// </summary>
        /// <param name="DutyCycle">% duty cycle from 0.0 to 1.0.</param>
        void SetOutput(float DutyCycle);

        /// <summary>
        /// Releases handles to the output, allowing it to be used by another component or application.
        /// </summary>
        void Dispose();
    }
}
