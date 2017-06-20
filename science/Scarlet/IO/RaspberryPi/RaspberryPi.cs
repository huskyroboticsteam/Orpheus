using System;
using System.Runtime.InteropServices;

namespace Scarlet.IO.RaspberryPi
{
    public static class RaspberryPi
    {
        private const string MAIN_LIB = "libwiringPi.so";
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
        public static void SetupGPIO()
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
            if (!Initialized) { throw new InvalidOperationException("Cannot perform GPIO operations until the system is initialized. Call RasperryPi.SetupGPIO()."); }
            Ext_SetPinMode(Pin, (int)Mode);
        }

        [DllImport(MAIN_LIB, EntryPoint = "pullUpDnControl")]
        private static extern void Ext_SetResistor(int Pin, int ResMode);

        internal static void SetResistor(int Pin, ResistorState ResMode)
        {
            if (!Initialized) { throw new InvalidOperationException("Cannot perform GPIO operations until the system is initialized. Call RasperryPi.SetupGPIO()."); }
            int ResistorVal = 0;
            if (ResMode == ResistorState.PULL_UP) { ResistorVal = 2; }
            else if (ResMode == ResistorState.PULL_DOWN) { ResistorVal = 1; }
            Ext_SetResistor(Pin, ResistorVal);
        }

        [DllImport(MAIN_LIB, EntryPoint = "digitalRead")]
        private static extern int Ext_DigitalRead(int Pin);

        internal static bool DigitalRead(int Pin)
        {
            if (!Initialized) { throw new InvalidOperationException("Cannot perform GPIO operations until the system is initialized. Call RasperryPi.SetupGPIO()."); }
            return Ext_DigitalRead(Pin) != 0;
        }


        [DllImport(MAIN_LIB, EntryPoint = "digitalWrite")]
        private static extern void Ext_DigitalWrite(int Pin, int Value);

        internal static void DigitalWrite(int Pin, bool OutputVal)
        {
            if (!Initialized) { throw new InvalidOperationException("Cannot perform GPIO operations until the system is initialized. Call RasperryPi.SetupGPIO()."); }
            Ext_DigitalWrite(Pin, OutputVal ? 0 : 1);
        }

        #endregion
    }
}
