using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;

namespace Scarlet.Communications
{
    static class WatchdogManager
    {
        public static bool Connected;
        public static List<string> WatchdogTargets { get; private set; }

        private static bool WatchdogCycleFound;
        private static bool Started;
        private static bool Continue = true;
        private static bool IsClient;
        private static event EventHandler<EventArgs> ConnectionChangeEvent;
        private static List<Packet> WatchdogPackets;

        /// <summary>
        /// Adds a new Endpoint to send watchdogs to.
        /// Not necessary for Clients
        /// </summary>
        /// <param name="NewEndpoint">Endpoint to add</param>
        public static void AddWatchdogEndpoint(string NewEndpoint)
        {
            WatchdogTargets.Add(NewEndpoint);
            WatchdogPackets.Add(new Packet(Constants.WATCHDOG_PING, true, NewEndpoint));
        }

        /// <summary>
        /// Call this method every time a watchdog packet is
        /// found on the network.
        /// </summary>
        public static void FoundWatchdog()
        {
            if (!Started) { Start(true); } // Server must start the watchdogs
            WatchdogCycleFound = true;
        }

        /// <summary>
        /// Starts the ConnectionStatusManager
        /// </summary>
        private static void Start(bool IsClient)
        {
            WatchdogManager.IsClient = IsClient;
            WatchdogTargets = new List<string>();
            WatchdogPackets = new List<Packet>();
            if (IsClient) { WatchdogPackets.Add(new Packet(Constants.WATCHDOG_PING, true)); }
            ConnectionChangeEvent += ConnectionChange;
            new Thread(new ThreadStart(Listen)).Start();
            new Thread(new ThreadStart(SendWatchdogs)).Start();
            Started = true;
        }

        /// <summary>
        /// Starts sending the watchdogs
        /// </summary>
        private static void SendWatchdogs()
        {
            while (Continue)
            {
                if (IsClient) { Client.SendNow(WatchdogPackets[0]); }
                else { foreach (Packet Watchdog in WatchdogPackets) { Server.SendNow(Watchdog); } }
            }
        }

        /// <summary>
        /// Listens for FoundWatchdog(),
        /// sets Connection status
        /// </summary>
        private static void Listen()
        {
            Stopwatch Timer = new Stopwatch();
            while (Continue)
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
        public static void Stop() { Continue = false; Started = false; }

        /// <summary>
        /// Restarts the connection status manager
        /// </summary>
        public static void Restart() { Continue = true; }

    }
}
