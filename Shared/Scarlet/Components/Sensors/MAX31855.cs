using Scarlet.IO;
using Scarlet.IO.BeagleBone;
using Scarlet.Utilities;
using System;

namespace Scarlet.Components.Sensors
{
    /// <summary>
    /// Cold-Junction Compensated Thermocouple-to-Digital Converter
    /// Datasheet: https://datasheets.maximintegrated.com/en/ds/MAX31855.pdf
    /// </summary>
    public class MAX31855 : ISensor
    {
        private ISPIBus Bus;
        private BBBPin ChipSelect;
        private uint LastReading;

        /// <summary>
        /// Lists the possible reported faults.
        /// SHORT_VCC: The thermoucouple is shorted to the Vcc line.
        /// SHORT_GND: The thermoucouple is shorted to the GND line.
        /// NO_THERMOCOUPLE: The thermoucouple is not properly connected or is defective.
        /// </summary>
        [Flags]
        public enum Fault { NONE = 0, NO_THERMOCOUPLE = 1, SHORT_GND = 2, SHORT_VCC = 4 }

        public MAX31855(ISPIBus Bus, BBBPin ChipSelect)
        {
            this.Bus = Bus;
            this.ChipSelect = ChipSelect;
        }

        public void Initialize() { }

        public bool Test()
        {
            UpdateState();
            return ConvertFaultFromRaw(this.LastReading) == Fault.NONE;
        }

        public void UpdateState()
        {
            byte[] InputData = this.Bus.Write(this.ChipSelect, new byte[] { 0x00, 0x00, 0x00, 0x00 }, 4);
            this.LastReading = UtilData.ToUShort(InputData);
        }

        public float GetInternalTemp() { return ConvertInternalFromRaw(this.LastReading); }
        public float GetExternalTemp() { return ConvertExternalFromRaw(this.LastReading); }
        public Fault GetFaults() { return ConvertFaultFromRaw(this.LastReading); }

        public static float ConvertInternalFromRaw(uint RawData)
        {
            ushort InputBits = (ushort)((RawData >> 4) & 0b1111_1111_1111);
            short OutputBits = 0;
            if (InputBits > 0b1000_0000_0000) { OutputBits = (short)((0x1000 - InputBits) * -1); } // Convert 12-bit to 16-bit value.
            else { OutputBits = (short)InputBits; }
            return OutputBits / 16.0000F;
        }

        public static float ConvertExternalFromRaw(uint RawData)
        {
            ushort InputBits = (ushort)((RawData >> 18) & 0b11_1111_1111_1111);
            short OutputBits = 0;
            if (InputBits > 0b10_0000_0000_0000) { OutputBits = (short)((0x4000 - InputBits) * -1); } // Convert 14-bit to 16-bit value.
            else { OutputBits = (short)InputBits; }
            return OutputBits / 4.00F;
        }

        public static Fault ConvertFaultFromRaw(uint RawData)
        {
            if (((RawData >> 17) & 1) == 1) // FAULT bit is set.
            {
                return (Fault)((RawData & 0b111));
            }
            else { return Fault.NONE; }
        }

        public void EventTriggered(object Sender, EventArgs Event)
        {
            throw new NotImplementedException();
        }
    }
}
