namespace Scarlet.IO
{
    public interface IAnalogueOut
    {
        /// <summary> Sets the output to the specified level. </summary>
        void SetOutput(double Output);

        /// <summary> Gets the maximum output level. </summary>
        double GetRange();

        /// <summary> Releases handles to the output, allowing it to be used by another component or application. </summary>
        void Dispose();
    }
}
