
namespace Scarlet.IO
{
    public interface IDigitalOut
    {
        /// <summary>
        /// Prepares the output for use.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Sets the output to on or off.
        /// </summary>
        void SetOutput(bool Output);

        /// <summary>
        /// Releases handles to the output, allowing it to be used by another component or application.
        /// </summary>
        void Dispose();
    }
}
