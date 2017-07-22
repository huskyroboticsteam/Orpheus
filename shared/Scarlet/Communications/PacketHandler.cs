using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scarlet.Communications
{
    
    /// <summary>
    /// Singleton Pattern.
    /// Call GetInstance() instead of constructor
    /// </summary>
    class PacketHandler
    {
        private PacketHandler Instance = null;

        private PacketHandler()
        {
            // Setup PacketHandler
            // Setup Watchdog
            Parse.SetParseHandler(Constants.WATCHDOG_PING, ParseWatchdogPacket);
        }

        public PacketHandler GetInstance()
        {
            if (Instance == null) { return new PacketHandler(); }
            return Instance;
        }

        public static void ParseWatchdogPacket(Message WatchdogPacket) { ConnectionStatusManager.FoundWatchdog(); }

    }
}
