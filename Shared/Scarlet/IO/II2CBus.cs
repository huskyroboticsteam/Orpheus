using System;

namespace Scarlet.IO
{
    public interface II2CBus
    {
        void Initialize();

        void Write(byte Address, byte[] Data);

        void WriteRegister(byte Address, byte Register, byte[] Data);

        byte[] Read(byte Address, int DataLength);

        byte[] ReadRegister(byte Address, byte Register, int DataLength);

        void Dispose();
    }
}
