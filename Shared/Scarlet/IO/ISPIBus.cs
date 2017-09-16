using Scarlet.IO.BeagleBone;
using System;

namespace Scarlet.IO
{
    public interface ISPIBus
    {
        void Initialize();

        // This will be changed to IDigitalOut instead of BBBPin.
        byte[] Write(IDigitalOut DeviceSelect, byte[] Data, int DataLength);

        void Dispose();
    }
}
