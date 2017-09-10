using Scarlet.IO;
using System;

namespace Scarlet.Components.Sensors
{
    /// <summary>
    /// VEML6070 UV-A Light Sensor with I2C Interface
    /// Datasheet: http://www.vishay.com/docs/84277/veml6070.pdf
    /// </summary>
    public class VEML6070 : ISensor
    {
        // LSB address is also used for write functions.
        private byte AddressLSB, AddressMSB;
        private II2CBus Bus;
        private ushort LastReading;

        public VEML6070(II2CBus Bus, byte Address = 0x38)
        {
            this.AddressLSB = Address;
            this.AddressMSB = (byte)(Address + 1);
        }

        public void Initialize()
        {
            this.Bus.Write(this.AddressLSB, new byte[] { (0x02 << 2) | 2 }, 1); // Set timing.
        }

        /// <summary>
        /// Gets the current UV light level.
        /// </summary>
        /// <returns>UV light level in μW/cm/cm.</returns>
        public int GetData()
        {
            return ConvertFromRaw(this.LastReading);
        }

        public static int ConvertFromRaw(ushort RawData)
        {
            return RawData * 5;
        }

        public bool Test() { return true; } // TODO: See if there is a way to test.

        public void UpdateState()
        {
            ushort Data = 0x00_00;
            Data = this.Bus.Read(this.AddressLSB, 1)[0];
            Data |= (ushort)(this.Bus.Read(this.AddressMSB, 1)[0] << 8);
            this.LastReading = Data;
        }

        public void EventTriggered(object Sender, EventArgs Event)
        {
            throw new NotImplementedException();
        }
    }
}
