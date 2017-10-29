using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace Scarlet.Communications
{
    /// <summary>
    /// WatchdogManager handles internal
    /// watchdog receiving and sending for
    /// both Clients and Servers. Not meant
    /// for End-User. Is internal to Scarlet.
    /// For more information about watchdogs,
    /// visit the OneNote documentation.
    /// * All watchdogs are send with UDP
    /// </summary>
    internal static class WatchdogManager
    {
        // Define booleans for whether or not the manager is a client manager and whether or not it is started.
        private static bool IsClient, Started;
        // Define the continue state for continuous looping of the watchdog threads
        private static bool Continue = true;
        // The name of the watchdog sender
        private static string MyName;
        // The watchdog packet to send iteratively
        private static Packet WatchdogPacket;
        // A dictionary of watchdog endpoints
        private static Dictionary<string, Watchdog> Watchdogs;
        // The event to fire when a connection has changed
        internal static event EventHandler<ConnectionStatusChanged> ConnectionChanged;

        /// <summary>
        /// Starts a watchdog manager.
        /// Parameters default to a server
        /// watchdog. If you are a server you
        /// can just call WatchdogManager.Start()
        /// Will not change any functionality unless 
        /// WatchdogManager is in a stopped state
        /// </summary>
        /// <param name="IsClient">Whether or not the watchdog sender is a client</param>
        /// <param name="MyName">The name of the watchdog sender</param>
        internal static void Start(bool IsClient = false, string MyName = "Server")
        {
            // As long as the watchdog is not already started, go ahead with initialization.
            if (!Started)
            {
                // Set internal fields to given parameters
                WatchdogManager.MyName = MyName;
                WatchdogManager.IsClient = IsClient;
                // Initialize data structures
                WatchdogManager.Watchdogs = new Dictionary<string, Watchdog>();
                // Construct the watchdog packet (UDP)
                WatchdogManager.WatchdogPacket = new Packet(Constants.WATCHDOG_PING, true);
                WatchdogManager.WatchdogPacket.AppendData(Utilities.UtilData.ToBytes(MyName));
                // If Watchdog sender is a client, solely add the server to the watchdogs
                if (IsClient) { Watchdogs.Add("Server", new Watchdog("Server")); }
                // Start the send thread
                new Thread(new ThreadStart(Send)).Start();
                // Set the state of WatchdogManager to be started
                Started = true;
            }
        }

        /// <summary>
        /// Stops the watchdog. Call Start(...) again to restart the watchdog process
        /// </summary>
        internal static void Stop()
        {
            Continue = false;
            Started = false;
        }

        /// <summary>
        /// Sends all required watchdogs over the network.
        /// </summary>
        private static void Send()
        {
            // While the continue state of watchdog is set
            while (Continue)
            {
                // Update the timestamp on the packet
                WatchdogPacket.UpdateTimestamp();
                // Sleep for a given interval to avoid CPU overload
                Thread.Sleep(Constants.WATCHDOG_INTERVAL);
                // Send the watchdogs
                // The client should send watchdogs regardless of whether or not it detects a connection
                if (IsClient) { Client.SendRegardless(WatchdogPacket); } 
                else
                {
                    string[] Keys; // Stores the keys of the Watchdogs dictionary
                    // Lock the watchdog keys on this thread
                    lock (Watchdogs)
                    {
                        // Copy the keys from the watchdog dictionary into a new array
                        Keys = new string[Watchdogs.Count];
                        Watchdogs.Keys.CopyTo(Keys, 0);
                    }
                    // Loop over the endpoints and send the watchdogs with the server
                    foreach (string EP in Keys)
                    {
                        WatchdogPacket.Endpoint = EP;
                        Server.SendNow(WatchdogPacket);
                    }
                }
            }
        }

        /// <summary>
        /// Adds a watchdog endpoint.
        /// Useful for servers, cannot use with Client Watchdogs. 
        /// Creates a new Watchdog for send-receive
        /// </summary>
        /// <param name="Endpoint">The endpoint to add</param>
        internal static void AddWatchdog(string Endpoint)
        {
            // Check if a client attempts to add a watchdog
            if (IsClient) { throw new InvalidOperationException("Clients cannot add watchdogs"); }
            // Construct the client's watchdog packet
            Packet WatchdogPacket = new Packet(Constants.WATCHDOG_PING, true, Endpoint);
            WatchdogPacket.AppendData(Utilities.UtilData.ToBytes(Endpoint));
            // Append to the dictionary so long as it isn't already there
            // (otherwise and exception will be thrown)
            if (!Watchdogs.ContainsKey(Endpoint))
            {
                Watchdogs.Add(Endpoint, new Watchdog(Endpoint));
            }
        }

        /// <summary>
        /// Removes a watchdog endpoint from the system completely.
        /// Use only if you are a server
        /// </summary>
        /// <param name="Endpoint">The endpoint to remove</param>
        internal static void RemoveWatchdog(string Endpoint)
        {
            // Check if a client attempts to remove a watchdog
            if (IsClient) { throw new InvalidOperationException("Clients cannot remove watchdogs"); }
            // Remove the watchdogs from the data structures
            lock (Watchdogs) { Watchdogs.Remove(Endpoint); }
        }

        /// <summary>
        /// Call this if you have found a watchdog.
        /// If you are client, make sure that the
        /// Endpoint is "Server".
        /// This is the basis of the operation,
        /// when you receive a watchdog packet,
        /// call this method to invoke a watchdog is found.
        /// </summary>
        /// <param name="Endpoint">The endpoint the watchdog was found on</param>
        internal static void FoundWatchdog(string Endpoint)
        {
            // If we have not started a watchdog, and we are a client, start it.
            if (!Started) { Start(true, Client.Name); }
            // Then set the appropriate watchdog to the found state
            Watchdogs[Endpoint].FoundWatchdog();
        }

        /// <summary>
        /// Checks if there is a connection, only for Client
        /// </summary>
        /// <returns>Whether or not there is a server connection</returns>
        internal static bool IsConnected()
        {
            // Check if the system is a server or client. Throw an illegal operation exception if server
            if (!IsClient) { throw new InvalidOperationException("Cannot call IsConnected without a string parameter if not a client"); }
            // Typically would check if there is a key "Server" in the dictionary, but there should be if a client
            // and if there isn't we should throw an exception. This takes care of both scenarios.
            return Watchdogs["Server"].IsConnected;
        }

        /// <summary>
        /// Checks if there is a client connection
        /// given an endpoint. Only for server
        /// </summary>
        /// <param name="Endpoint">Endpoint to check</param>
        /// <returns>Whether or not there is a watchdog connection with the given endpoint</returns>
        internal static bool IsConnected(string Endpoint)
        {
            // Check if the wrong usage occurs
            if (IsClient) { throw new InvalidOperationException("Cannot call IsConnected with a string paramter if a client"); }
            // Check if the data structure contains the endpoint, if not return false
            if (!Watchdogs.ContainsKey(Endpoint)) { return false; }
            // Return the connection status of the given endpoint
            return Watchdogs[Endpoint].IsConnected;
        }

        /// <summary>
        /// Invokes a connection change event. Call
        /// when a connection status changes.
        /// </summary>
        /// <param name="Event">The event to trigger</param>
        private static void OnConnectionChange(ConnectionStatusChanged Event) { ConnectionChanged?.Invoke("Watchdog Timer", Event); }

        /// <summary>
        /// Internal class to the watchdog manager.
        /// Handles processing of watchdogs
        /// </summary>
        private class Watchdog
        {
            // IsConnected determines the connection status of the watchdog
            private bool P_IsConnected;
            public bool IsConnected
            {
                get { return P_IsConnected; }
                private set
                {
                    if (value != P_IsConnected) // Status is changing
                    {
                        // Trigger the event for connection change
                        ConnectionStatusChanged Event = new ConnectionStatusChanged() { StatusEndpoint = Endpoint, StatusConnected = value };
                        OnConnectionChange(Event);
                    }
                    P_IsConnected = value; // Finally, set the variable
                }
            }

            // Whether or not a watchdog has been found on the current cycle
            private volatile bool FoundWatchdogThisCycle;
            // The endpoint of the watchdog receiver (i.e. not the sender)
            private string Endpoint;

            /// <summary>
            /// Constructs a watchdog object
            /// </summary>
            /// <param name="ListenFor">The endpoint to listen for (i.e. not the sender)</param>
            public Watchdog(string ListenFor)
            {
                // Set the internal fields and start the listening thread
                this.Endpoint = ListenFor;
                new Thread(new ThreadStart(Listen)).Start();
            }

            /// <summary>
            /// Call this on the watchdog if the watchdog was found on the cycle
            /// </summary>
            internal void FoundWatchdog() { FoundWatchdogThisCycle = true; }

            /// <summary>
            /// An interative thread that will stop if 
            /// WatchdogManager.Stop() is called
            /// </summary>
            private void Listen()
            {
                // While we are not stopping
                while (Continue)
                {
                    // Sleep for a given period to avoid overloading the CPU
                    Thread.Sleep(Constants.WATCHDOG_WAIT);
                    // Set the IsConnected state to whether or not there was a 
                    // watchdog found on the cycle.
                    IsConnected = FoundWatchdogThisCycle;
                    // Reset the found on cycle state of the watchdog
                    FoundWatchdogThisCycle = false;
                }
            }

        }
    }

    /// <summary>
    /// This class contains the endpoint of the connection,
    /// and the status of the connection. It overloads EventArgs
    /// and is used in triggering connection changes between points
    /// on the communication stream.
    /// </summary>
    public class ConnectionStatusChanged : EventArgs
    {
        // The Endpoint that either is or is not connected to the current system
        public string StatusEndpoint;
        // The status of the endpoints connection with the current system
        public bool StatusConnected;
    }

}
