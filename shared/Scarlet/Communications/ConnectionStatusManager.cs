using System;
using System.Diagnostics;
using System.Threading;

namespace Scarlet.Communications
{
    static class ConnectionStatusManager
    {
        public static bool Connected;

        private static bool WatchdogCycleFound;
        private static bool Started;
        private static bool ContinueListening = true;
        private static event EventHandler<EventArgs> ConnectionChangeEvent;

        /// <summary>
        /// Call this method every time a watchdog packet is
        /// found on the network.
        /// </summary>
        public static void FoundWatchdog()
        {
            if (!Started) { Start(); }
            WatchdogCycleFound = true;
        }

        /// <summary>
        /// Starts the ConnectionStatusManager
        /// </summary>
        private static void Start()
        {
            ConnectionChangeEvent += ConnectionChange;
            new Thread(new ThreadStart(Listen)).Start();
            Started = true;
        }

        /// <summary>
        /// Listens for FoundWatchdog(),
        /// sets Connection status
        /// </summary>
        private static void Listen()
        {
            Stopwatch Timer = new Stopwatch();
            while (ContinueListening)
            {
                Timer.Start();
                while (Timer.ElapsedMilliseconds < Constants.WATCHDOG_DELAY)
                {
                    if (WatchdogCycleFound && !Connected) { ConnectionChange("Watchdog", new EventArgs()); }
                }
                if (!WatchdogCycleFound && Connected) { ConnectionChange("Watchdog", new EventArgs()); }
                Timer.Reset();
                WatchdogCycleFound = false;
            }
        }

        /// <summary>
        /// Event handler for connection changes.
        /// </summary>
        /// <param name="Sender">Event sender</param>
        /// <param name="Args">Event arguments</param>
        private static void ConnectionChange(object Sender, EventArgs Args) { Connected = !Connected; }

        /// <summary>
        /// Stops the connection status manager
        /// </summary>
        public static void Stop() { ContinueListening = false; Started = false; }

        /// <summary>
        /// Restarts the connection status manager
        /// </summary>
        public static void Continue() { ContinueListening = true; }

    }
}
