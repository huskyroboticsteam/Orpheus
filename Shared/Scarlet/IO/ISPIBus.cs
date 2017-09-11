using Scarlet.IO.BeagleBone;
using System;

namespace Scarlet.IO
{
    public interface ISPIBus
    {
        void Initialize();

        // These will be changed to IDigitalOut instead of BBBPin.
        void Write(BBBPin DeviceSelect, byte[] Data, int DataLength);

        byte[] Read(BBBPin DeviceSelect, int DataLength);

        void Dispose();
    }
}
