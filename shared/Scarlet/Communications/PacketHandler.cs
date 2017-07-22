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

        public static void ParseWatchdogPacket(Message WatchdogPacket) { ConnectionStatusManager.FoundWatchdog(); }

    }
}
