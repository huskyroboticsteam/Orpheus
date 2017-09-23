using Scarlet.Utilities;
using System;
using System.Runtime.InteropServices;

namespace Scarlet.IO.RaspberryPi
{
    public static class RaspberryPi
    {
        private const string MAIN_LIB = "libwiringPi.so";
        private const string I2C_LIB = "libwiringPiI2C.so";
        private const string SPI_LIB = "libwiringPiSPI.so";

        private static bool P_Initialized = false;
        internal static bool Initialized
        {
            get { return P_Initialized; }
            private set { P_Initialized = value; }
        }


        /// <returns>Always 0, You can ignore this.</returns>
        [DllImport(MAIN_LIB, EntryPoint = "wiringPiSetupPhys")]
        private static extern int Ext_SetupGPIO();

        /// <summary>
        /// Prepares the Raspberry Pi's GPIO system for use. You should do this before using any GPIO functions.
        /// </summary>
        public static void Initialize()
        {
            Initialized = true;
            Ext_SetupGPIO();
        }


        internal enum PinMode
        {
            INPUT = 0,
            OUTPUT = 1,
            OUTPUT_PWM = 2,
            GPIO_CLOCK = 3
        }

        #region GPIO

        [DllImport(MAIN_LIB, EntryPoint = "pinMode")]
        private static extern void Ext_SetPinMode(int Pin, int Mode);

        internal static void SetPinMode(int Pin, PinMode Mode)
        {
            if (!Initialized) { throw new InvalidOperationException("Cannot perform GPIO operations until the system is initialized. Call RasperryPi.Initialize()."); }
            Ext_SetPinMode(Pin, (int)Mode);
        }

        [DllImport(MAIN_LIB, EntryPoint = "pullUpDnControl")]
        private static extern void Ext_SetResistor(int Pin, int ResMode);

        internal static void SetResistor(int Pin, ResistorState ResMode)
        {
            if (!Initialized) { throw new InvalidOperationException("Cannot perform GPIO operations until the system is initialized. Call RasperryPi.Initialize()."); }
            int ResistorVal = 0;
            if (ResMode == ResistorState.PULL_UP) { ResistorVal = 2; }
            else if (ResMode == ResistorState.PULL_DOWN) { ResistorVal = 1; }
            Ext_SetResistor(Pin, ResistorVal);
        }

        [DllImport(MAIN_LIB, EntryPoint = "digitalRead")]
        private static extern int Ext_DigitalRead(int Pin);

        internal static bool DigitalRead(int Pin)
        {
            if (!Initialized) { throw new InvalidOperationException("Cannot perform GPIO operations until the system is initialized. Call RasperryPi.Initialize()."); }
            return Ext_DigitalRead(Pin) != 0;
        }


        [DllImport(MAIN_LIB, EntryPoint = "digitalWrite")]
        private static extern void Ext_DigitalWrite(int Pin, int Value);

        internal static void DigitalWrite(int Pin, bool OutputVal)
        {
            if (!Initialized) { throw new InvalidOperationException("Cannot perform GPIO operations until the system is initialized. Call RasperryPi.Initialize()."); }
            Ext_DigitalWrite(Pin, OutputVal ? 0 : 1);
        }

        #endregion

        #region Interrupts

        internal delegate void InterruptCallback();

        [DllImport(MAIN_LIB, EntryPoint = "wiringPiISR")]
        private static extern int Ext_AddInterrupt(int Pin, int InterruptType, InterruptCallback Delegate);

        internal static void AddInterrupt(int Pin, int InterruptType, InterruptCallback Delegate)
        {
            if (!Initialized) { throw new InvalidOperationException("Cannot perform GPIO operations until the system is initialized. Call RasperryPi.Initialize()."); }
            Ext_AddInterrupt(Pin, InterruptType, Delegate);
        }

        #endregion

        #region I2C

        [DllImport(I2C_LIB, EntryPoint = "wiringPiI2CSetup")]
        private static extern int Ext_I2CSetup(int DeviceID);

        internal static void I2CSetup(byte DeviceID)
        {
            int SetupVal = Ext_I2CSetup(DeviceID);
            if (SetupVal == -1) { throw new Exception("Unable to setup I2C device: " + DeviceID); }
        }

        [DllImport(I2C_LIB, EntryPoint = "wiringPiI2CRead")]
        private static extern int Ext_I2CRead(int DeviceID);

        internal static byte I2CRead(byte DeviceID)
        {
            int Data = Ext_I2CRead(DeviceID);
            return (byte)Data;
        }

        [DllImport(I2C_LIB, EntryPoint = "wiringPiI2CWrite")]
        private static extern int Ext_I2CWrite(int DeviceID, int Data);

        internal static void I2CWrite(byte DeviceID, byte Data)
        {
            Ext_I2CWrite(DeviceID, Data);
        }

        #endregion

        #region SPI

        [DllImport(SPI_LIB, EntryPoint = "wiringPiSPISetup")]
        private static extern void Ext_SPISetup(int BusNum, int Speed);

        internal static void SPISetup(int BusNum, int Speed) { Ext_SPISetup(BusNum, Speed); }

        [DllImport(SPI_LIB, EntryPoint = "wiringPiSPIDataRW")]
        private static extern int Ext_SPIRW(int BusNum, [In,Out] byte[] Data, int Length);

        internal static byte[] SPIRW(int BusNum, byte[] Data, int Length)
        {
            byte[] DataNew = Data; // Unsure if Data will be changed if sent through WiringPI, but this should clone
            int ReturnData = Ext_SPIRW(BusNum, DataNew, Length);
            byte[] ReturnBytes = UtilData.ToBytes(ReturnData);
            return ReturnBytes;
        }

        #endregion

    }
}
