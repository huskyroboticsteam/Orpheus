using System;

namespace Scarlet.IO
{
    public interface II2CBus
    {
        /// <summary>Writes data to the given device by address over the I2C bus.</summary>
        void Write(byte Address, byte[] Data);

        /// <summary>Writes data to the specified register to the given device by address over the I2C bus.</summary>
        void WriteRegister(byte Address, byte Register, byte[] Data);

        /// <summary>Reads the specified amount of data from the device by address.</summary>
        byte[] Read(byte Address, int DataLength);

        /// <summary>Reads the specified amount of data from the specified register from the device by address.</summary>
        byte[] ReadRegister(byte Address, byte Register, int DataLength);

        /// <summary>Cleans up the bus object, freeing resources.</summary>
        void Dispose();
    }
}
