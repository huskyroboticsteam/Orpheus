using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace Scarlet.Communications
{ 
    static class WatchdogManager
    {
        private static bool IsClient, Started, Continue;
        private static Dictionary<string, Watchdog> Watchdogs;
        public static event EventHandler<ConnectionStatusChanged> ConnectionChanged;

        public static void Start(bool IsClient=false)
        {
            if (!Started)
            {
                WatchdogManager.IsClient = IsClient;
                if (IsClient) { Watchdogs.Add("Server", new Watchdog(new Packet(Constants.WATCHDOG_PING, true), "Server", true)); }
                Started = true;
            }
        }

        public static void AddWatchdog(string Endpoint)
        {
            if (IsClient) { throw new InvalidOperationException("Clients cannot add watchdogs"); }
            Packet WatchdogPacket = new Packet(Constants.WATCHDOG_PING, true, Endpoint);
            WatchdogPacket.AppendData(Encoding.Unicode.GetBytes(Endpoint));
            Watchdogs.Add(Endpoint, new Watchdog(WatchdogPacket, Endpoint, false));
        }
        
        public static void FoundWatchdog(string Endpoint)
        {
            if (!Started) { Start(true); }
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
            private Packet WatchdogPacket;

            public Watchdog(Packet WatchdogPacket, string Endpoint, bool UseClient)
            {
                this.WatchdogPacket = WatchdogPacket;
                this.UseClient = UseClient;
                this.Endpoint = Endpoint;
                new Thread(new ThreadStart(Send)).Start();
                new Thread(new ThreadStart(Listen)).Start();
            }

            public void FoundWatchdog() { FoundWatchdogThisCycle = true; }

            public void Send()
            {
                while(Continue)
                {
                    Thread.Sleep(Constants.WATCHDOG_DELAY);
                    if (UseClient) { Client.SendNow(WatchdogPacket); }
                    else { Server.SendNow(WatchdogPacket); }
                }
            }

            public void Listen()
            {
                while(Continue)
                { 
                    Thread.Sleep(Constants.WATCHDOG_DELAY);
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
                        if(IsConnected)
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
