using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scarlet.Communications
{

    static class PacketHandler
    {
        private static bool Started;

        /// <summary>
        /// Starts the PacketHandler.
        /// PacketHandler ensures Start procedure
        /// is only ran once, so multiple calls
        /// to Start() will not interfere with each
        /// other.
        /// </summary>
        public static void Start()
        {
            if (!Started)
            {
                Started = true;
                // Setup PacketHandler
                // Setup Watchdog
                Parse.SetParseHandler(Constants.WATCHDOG_PING, ParseWatchdogPacket);
            }
        }

        /// <summary>
        /// Watchdog parse handler
        /// </summary>
        /// <param name="WatchdogPacket">Packet to parse</param>
        public static void ParseWatchdogPacket(Message WatchdogPacket) { WatchdogManager.FoundWatchdog(); }

    }
}
