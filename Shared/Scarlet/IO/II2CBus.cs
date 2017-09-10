using System;

namespace Scarlet.IO
{
    public interface II2CBus
    {
        void Initialize();

        void Write(byte Address, byte[] Data, int DataLength);

        byte[] Read(byte Address, int DataLength);

        void Dispose();
    }
}
