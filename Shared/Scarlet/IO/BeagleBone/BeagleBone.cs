using System;

namespace Scarlet.IO.BeagleBone
{
	public static class BeagleBone
	{
        internal static bool Initialized { get; private set; }
        internal static SystemMode Peripherals;
        internal static bool FastGPIO { get; private set; }

        /// <summary>
        /// Prepares the Beaglebone I/O system for use.
        /// </summary>
        /// <param name="UseFastAccess">
        /// This switches between memory mapped I/O (true), and filesystem-based I/O (false).
        /// Filesystem-based I/O is the "correct" Linux way to do it, so is therefore more supported.
        /// Memory mapping is several orders of magnitude faster (Up to 1000x!), but is not technically supported. That said, it almost always works fine.
        /// We suggest you try memory-mapped at first, then if you run into odd issues, try filesystem to see if it helps.
        /// This only affects some I/O types, mainly GPIO.
        /// </param>
        public static void Initialize(SystemMode Mode, bool UseFastAccess)
		{
            Peripherals = Mode;
            FastGPIO = UseFastAccess;
            PWMBBB.Initialize();
		}
	}
}
