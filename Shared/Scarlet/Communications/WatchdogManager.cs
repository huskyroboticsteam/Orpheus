using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace Scarlet.Communications
{
    static class WatchdogManager
    {
        private static bool IsClient, Started;
        private static bool Continue = true;
        private static string MyName;
        private static Packet WatchdogPacket;
        private static Dictionary<string, Watchdog> Watchdogs;
        public static event EventHandler<ConnectionStatusChanged> ConnectionChanged;

        public static void Start(bool IsClient=false, string MyName="Server")
        {
            if (!Started)
            {
                WatchdogManager.MyName = MyName;
                WatchdogManager.IsClient = IsClient;
                WatchdogManager.Watchdogs = new Dictionary<string, Watchdog>();
                WatchdogManager.WatchdogPacket = new Packet(Constants.WATCHDOG_PING, true);
                WatchdogManager.WatchdogPacket.AppendData(Utilities.UtilData.ToBytes(MyName));
                if (IsClient) { Watchdogs.Add("Server", new Watchdog("Server", true)); }
                new Thread(new ThreadStart(Send)).Start();
                Started = true;
            }
        }

        public static void Stop()
        {
            Continue = false;
            Started = false;
        }

        public static void Send()
        {
            while (Continue)
            {
                WatchdogPacket.UpdateTimestamp();
                Thread.Sleep(Constants.WATCHDOG_INTERVAL);
                if (IsClient) { Client.SendNow(WatchdogPacket); }
                else
                {
                    foreach (string EP in Watchdogs.Keys)
                    {
                        WatchdogPacket.Endpoint = EP;
                        Server.SendNow(WatchdogPacket);
                    }
                }
            }
        }

        public static void AddWatchdog(string Endpoint)
        {
            if (IsClient) { throw new InvalidOperationException("Clients cannot add watchdogs"); }
            Packet WatchdogPacket = new Packet(Constants.WATCHDOG_PING, true, Endpoint);
            WatchdogPacket.AppendData(Utilities.UtilData.ToBytes(Endpoint));
            Watchdogs.Add(Endpoint, new Watchdog(Endpoint, false));
        }

        public static void FoundWatchdog(string Endpoint)
        {
            if (!Started) { Start(true, Client.Name); }
            Watchdogs[Endpoint].FoundWatchdog();
        }

        public static bool IsConnected() { return Watchdogs["Server"].IsConnected; } // Only for Client
        public static bool IsConnected(string Endpoint) { return Watchdogs[Endpoint].IsConnected; } // Only for Server
        private static void OnConnectionChange(ConnectionStatusChanged Event) { ConnectionChanged?.Invoke("Watchdog Timer", Event); }

        private class Watchdog
        {
            private bool P_IsConnected;
            public bool IsConnected
            {
                get { return P_IsConnected; }
                private set
                {
                    if(value != P_IsConnected) // Status is changing
                    {
                        ConnectionStatusChanged Event = new ConnectionStatusChanged() { StatusEndpoint = Endpoint, StatusConnected = value };
                        OnConnectionChange(Event);
                    }
                    P_IsConnected = value;
                }
            }

            private volatile bool FoundWatchdogThisCycle;
            private string Endpoint;

            public Watchdog(string ListenFor, bool UseClient)
            {
                this.Endpoint = ListenFor;
                new Thread(new ThreadStart(Listen)).Start();
            }

            public void FoundWatchdog() { FoundWatchdogThisCycle = true; }

            public void Listen()
            {
                while (Continue)
                {
                    Thread.Sleep(Constants.WATCHDOG_WAIT);
                    IsConnected = FoundWatchdogThisCycle;
                    FoundWatchdogThisCycle = false;
                }
            }

        }
    }

    public class ConnectionStatusChanged : EventArgs
    {
        public string StatusEndpoint;
        public bool StatusConnected;
    }

}
