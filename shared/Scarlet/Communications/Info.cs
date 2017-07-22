using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Scarlet.Communications
{
    static class Constants
    {
        #region Communication Defaults
        public const int WATCHDOG_DELAY = 400;  // ms
        #endregion

        #region Reserved Packet IDs
        public const int WATCHDOG_PING = 0xF0;
        #endregion

    }

    static class ConnectionStatusManager
    {
        public static bool Connected;

        private static bool WatchdogCycleFound;
        private static bool Started;
        private static bool ContinueListening = true;
        private static event EventHandler<EventArgs> ConnectionChangeEvent;

        public static void FoundWatchdog()
        {
            if (!Started) { Start(); }
            WatchdogCycleFound = true;
        }

        private static void Start()
        {
            ConnectionChangeEvent += ConnectionChange;
            new Thread(new ThreadStart(Listen)).Start();
            Started = true;
        }

        private static void Listen()
        {
            Stopwatch Timer = new Stopwatch();
            while (ContinueListening)
            {
                Timer.Start();
                while(Timer.ElapsedMilliseconds < Constants.WATCHDOG_DELAY)
                {
                    if (WatchdogCycleFound && !Connected) { ConnectionChange(new EventArgs());  }
                }
                if (!WatchdogCycleFound && Connected) { ConnectionChange(new EventArgs()); }
                Timer.Reset();
                WatchdogCycleFound = false;
            }
        }

        private static void ConnectionChange(EventArgs Event) { ConnectionChangeEvent?.Invoke("Watchdog", Event); }
        private static void ConnectionChange(object Sender, EventArgs Args) { Connected = !Connected; }

        public static void Stop() { ContinueListening = false; Started = false; }
        public static void Continue() { ContinueListening = true; }

    }
}
