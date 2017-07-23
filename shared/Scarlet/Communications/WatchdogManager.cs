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
                WatchdogManager.WatchdogPacket = new Packet(Constants.WATCHDOG_PING, false);
                WatchdogManager.WatchdogPacket.AppendData(Utilities.UtilData.ToBytes(MyName));
                if (IsClient) { Watchdogs.Add(MyName, new Watchdog(MyName, true)); }
                new Thread(new ThreadStart(Send)).Start();
                Started = true;
            }
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
            Packet WatchdogPacket = new Packet(Constants.WATCHDOG_PING, false, Endpoint);
            WatchdogPacket.AppendData(Encoding.Unicode.GetBytes(Endpoint));
            Watchdogs.Add(Endpoint, new Watchdog(Endpoint, false));
        }

        public static void FoundWatchdog(string Endpoint)
        {
            if (!Started) { Start(true, Client.Name); }
            Watchdogs[Endpoint].FoundWatchdog();
        }

        public static bool IsConnected() { return Watchdogs["Server"].IsConnected; } // Only for Client
        public static bool IsConnected(string Endpoint) { return Watchdogs[Endpoint].IsConnected; } // Only for Server
        public static void OnConnectionChange(ConnectionStatusChanged Event) { ConnectionChanged?.Invoke("Watchdog Timer", Event); }

        private class Watchdog
        {
            public bool IsConnected { get; private set; }

            private bool FoundWatchdogThisCycle;
            private bool UseClient;
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
                    if (FoundWatchdogThisCycle)
                    {
                        if (!IsConnected)
                        {
                            new ConnectionStatusChanged() { StatusEndpoint = Endpoint, StatusConnected = true };
                        }
                        IsConnected = true;
                    }
                    else
                    {
                        if (IsConnected)
                        {
                            new ConnectionStatusChanged() { StatusEndpoint = Endpoint, StatusConnected = false };
                        }
                        IsConnected = false;
                    }
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
