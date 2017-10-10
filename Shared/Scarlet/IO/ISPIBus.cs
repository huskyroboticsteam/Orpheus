using Scarlet.IO.BeagleBone;
using System;

namespace Scarlet.IO
{
    public interface ISPIBus
    {
        /// <summary>Reads and writes the given amount of data from the device.</summary>
        byte[] Write(IDigitalOut DeviceSelect, byte[] Data, int DataLength);

        /// <summary>Cleans up, freeing bus resources.</summary>
        void Dispose();
    }
}
