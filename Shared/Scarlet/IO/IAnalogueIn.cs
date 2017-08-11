namespace Scarlet.IO
{
    public interface IAnalogueIn
    {
        /// <summary> Prepares the input for use. </summary>
        void Initialize();

        /// <summary> Gets the current input level. </summary>
        double GetInput();

        /// <summary> Gets the maximum input level. </summary>
        double GetRange();

        /// <summary> Releases handles to the pin, allowing it to be used by another component or application. </summary>
        void Dispose();
    }
}
