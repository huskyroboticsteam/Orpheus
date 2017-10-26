using Scarlet.Utilities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Collections;
using System.Diagnostics;

namespace Scarlet.Communications
{

    /// <summary>
    /// Client is a class used to network
    /// a client to a server.
    /// 
    /// Please read the Scarlet documentation
    /// before use.
    /// </summary>
    public static class Client
    {

        #region Private Variables

        private static IPEndPoint ServerEndpointTCP; // Endpoints for TCP and UDP
        private static IPEndPoint ServerEndpointUDP;
        private static UdpClient ServerUDP; // TCP and UDP Clients
        private static TcpClient ServerTCP;

        private static Queue<Packet> SendQueue; // Queue to send packets
        private static Queue<Packet> ReceiveQueue; // Queue to process packets (incoming packet queue)

        private static Thread SendThread, ProcessThread; // Threads for sending and parsing/processing
        private static Thread ReceiveThreadUDP, ReceiveThreadTCP; // Threads for receiving on TCP and UDP
        private static Thread RetryConnection; // Retries Server connection after detecting disconnect. Only runs in disconnected state

        private static int ReceiveBufferSize; // Size of the receive buffer. Change this in Start() to receive larger packets
        private static int OperationPeriod; // The operation period (the higher this is, the longer the wait in between receiving/sending packets)
        private static bool Initialized; // Whether or not the client is initialized
        private static volatile bool StopProcesses; // If true, stops all threading processes
        private static bool HasDetectedDisconnect; // To be set if there is a send or receive error that would likely be caused by a disconnect.

        #endregion

        public static string Name { get; private set; } // Name of the client.
        public static bool IsConnected { get; private set; } // Whether or not the client and server are connected
        public static List<Packet> SentPackets { get; private set; } // Storage for send packets.
        public static List<Packet> ReceivedPackets { get; private set; } // Storage for received packets.
        public static event EventHandler<ConnectionStatusChanged> ClientConnectionChanged; // Invoked when the Client detects a connection change
        public static bool StorePackets; // Whether or not the client stores packets

        /// <summary>
        /// Starts a Client process.
        /// </summary>
        /// <param name="ServerIP">String representation of the IP Address of server.</param>
        /// <param name="PortTCP">Target port for TCP Communications on the server.</param>
        /// <param name="PortUDP">Target port for UDP Communications on the server.</param>
        /// <param name="ReceiveBufferSize">Size of buffer for incoming data.</param>
        /// <param name="OperationPeriod">Time in between receiving and sending individual packets.</param>
        public static void Start(string ServerIP, int PortTCP, int PortUDP, string Name, int ReceiveBufferSize = 64, int OperationPeriod = 20)
        {
            // Initialize PacketHandler
            PacketHandler.Start();
            // Output to debug that the client is starting up
            Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Initializing Client.");
            // Set local variables given the parameters
            Client.Name = Name;
            Client.ReceiveBufferSize = ReceiveBufferSize;
            Client.OperationPeriod = OperationPeriod;
            IPAddress ServerIPA = IPAddress.Parse(ServerIP);
            // Setup Endpoints for TCP and UDP
            ServerEndpointTCP = new IPEndPoint(ServerIPA, PortTCP);
            ServerEndpointUDP = new IPEndPoint(ServerIPA, PortUDP);
            if (!Initialized)
            {
                // Initialize the send and receive queues
                SendQueue = new Queue<Packet>();
                ReceiveQueue = new Queue<Packet>();

                // Initialize packet storage structures
                SentPackets = new List<Packet>();
                ReceivedPackets = new List<Packet>();

                // Initialize (but do not start the threads)
                SendThread = new Thread(new ThreadStart(SendPackets));
                ProcessThread = new Thread(new ThreadStart(ProcessPackets));
                RetryConnection = RetryConnectionThreadFactory();
                ReceiveThreadTCP = new Thread(new ParameterizedThreadStart(ReceiveFromSocket));
                ReceiveThreadUDP = new Thread(new ParameterizedThreadStart(ReceiveFromSocket));

                try
                {
                    // Initialize the TCP and UDP clients.
                    InitializeClients();
                    // Set the Connection status of the Client.
                    // This should be the only time we manually set this. 
                    // We just need to get to the point where the watchdog
                    // takes control of this.
                    IsConnected = true;
                    // Print the connection status to the console...
                    ConnectionAliveOutput(true);
                    // Invoke the Client event for a connection change using the known endpoint "Server" per the Client specification
                    ConnectionStatusChanged Event = new ConnectionStatusChanged() { StatusConnected = true, StatusEndpoint = "Server" };
                    ClientConnectionChanged?.Invoke(Name, Event);
                }
                catch (Exception Exception)
                {
                    Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Could not connect to TCP and UDP Servers. Failed to intitialize Client.");
                    Log.Exception(Log.Source.NETWORK, Exception);
                    return; // Do not let the Client continue through initialization if it did not see the server.
                }

                // Setup Watchdog
                // Client watchdog manager is automatically started
                // when it receives the first watchdog from the server.
                // Subscribe to the watchdog manager
                WatchdogManager.ConnectionChanged += ConnectionChange;

                // Send names to the server
                try { SendNames(); }
                catch { throw new InvalidOperationException("Could not begin communication with Server."); }

                // Start all primary thread procedures
                StartThreads();

                // Set the state of client to initialized.
                Initialized = true;
            }
        }

        #region Internal

        /// <summary>
        /// Event triggered when WatchdogManager
        /// detects a change in connection status.
        /// </summary>
        /// <param name="Sender">If triggered internally by WatchdogManager, this will be "Watchdog Timer"</param>
        /// <param name="Args">A ConnectionStatusChanged object containing the new status of the Client.</param>
        private static void ConnectionChange(object Sender, ConnectionStatusChanged Args)
        {
            // Need to detect if this is the first time we have connected or not
            bool FirstConnectionInvoke = !IsConnected;
            // Changed IsConnected state (unless the first time called...)
            FirstConnectionInvoke &= Args.StatusConnected;
            // If it is not, let's print the status otherwise, invoke the connection change
            if (!FirstConnectionInvoke) { ConnectionAliveOutput(true); }
            else { ClientConnectionChanged?.Invoke(Name, Args); }

            // Kill the reconnect thread or try to connect
            if (IsConnected && RetryConnection.IsAlive) { RetryConnection.Join(); }
            else if (!IsConnected)
            {
                // Get a new thread from the factory and start retrying the connection
                // Using a factory because you cannot restart a thread
                RetryConnection = RetryConnectionThreadFactory();
                RetryConnection.Start();
                // Output the current connection status (disconnected) to the console
                ConnectionAliveOutput(false);
            }
        }

        /// <summary>
        /// Logs to the console the appropriate information 
        /// if the connection is alive or dead. 
        /// * Will tell the console that the connection will
        /// be retried if the connection is not alive.
        /// </summary>
        /// <param name="IsAlive">Whether or not the connection is now alive</param>
        private static void ConnectionAliveOutput(bool IsAlive)
        {
            // Tell the console that the server is connected if the connection is alive
            // Otherwise tell the console that it is disconnected and is retrying to connect.
            if (IsAlive) { Log.Output(Log.Severity.INFO, Log.Source.NETWORK, "Server Connected..."); }
            else { Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Disconnected from server... Retrying..."); }
        }

        /// <summary>
        /// Retries sending names to the
        /// server until the Watchdog Manager finds 
        /// watchdog ping again.
        /// </summary>
        private static void RetryConnecting()
        {
            while (!IsConnected)
            {
                // Initialized the Client connections
                // (retries the connection establishment)
                try
                {
                    // Both these commands are allowed to fail
                    // Initialize the TCP connection
                    // We only need to initialize the
                    // TCP connection if the TCP connection
                    // has ever began, otherwise server
                    // will open a TCP socket
                    InitializeTCPClient();
                    // Send the name of the client
                    // on the TCP and UDP bus
                    SendNames();
                }
                catch { }
                // Sleep longer than the operation period to reconnect
                // We do not need to reconnect with that much speed.
                Thread.Sleep(Constants.WATCHDOG_WAIT);
            }
        }

        /// <summary>
        /// Assumes TCP and UCP clients are connected.
        /// Sends name to initialize a connection
        /// with server.
        /// </summary>
        private static void SendNames()
        {
            byte[] SendData = UtilData.ToBytes(Name);
            ServerUDP.Client.Send(SendData);
            ServerTCP.Client.Send(SendData);
        }

        /// <summary>
        /// Initializes the Client connections
        /// </summary>
        private static void InitializeClients()
        {
            // Initialize and connect to the UDP and TCP clients
            InitializeTCPClient();
            ServerUDP = new UdpClient();
            ServerUDP.Connect(ServerEndpointUDP);
        }

        /// <summary>
        /// Initializes TCP Client connection
        /// </summary>
        private static void InitializeTCPClient()
        {
            // Initialize and connect to the UDP and TCP clients
            ServerTCP = new TcpClient();
            ServerTCP.Connect(ServerEndpointTCP);
            // These parameters help avoid double-buffering
            ServerTCP.NoDelay = true;                           // Sets the TCP Client to have send delay (i.e. stores no overflow bits in the buffer)
            ServerTCP.ReceiveBufferSize = ReceiveBufferSize;    // Sets the receive buffer size to the max buffer size
        }

        /// <summary>
        /// Starts the threads for the receive, send, and processing
        /// systems.
        /// </summary>
        private static void StartThreads()
        {
            SendThread.Start();                         // Start sending packets
            ProcessThread.Start();                      // Begin processing packets
            ReceiveThreadTCP.Start(ServerTCP.Client);   // Start receiving on the TCP socket
            ReceiveThreadUDP.Start(ServerUDP.Client);   // Start receiving on the UDP socket
        }

        /// <summary>
        /// Creates a new thread for
        /// handling connection retries
        /// </summary>
        /// <returns></returns>
        private static Thread RetryConnectionThreadFactory() { return new Thread(new ThreadStart(RetryConnecting)); }

        #endregion

        #region Receive

        /// <summary>
        /// Receives packets from a given socket
        /// Object type parameter, because it must
        /// be if called from a thread.
        /// </summary>
        /// <param name="ReceiveSocket">The socket to recieve from.</param>
        private static void ReceiveFromSocket(object ReceiveSocket)
        {
            // Cast to a socket
            Socket Socket = (Socket)ReceiveSocket;
            // While we need to continue receiving
            while (!StopProcesses)
            {
                // Sleep for the operation period
                Thread.Sleep(OperationPeriod);

                // Checks if the client is connected and if
                // the server has available data
                if (Socket.Available > 0)
                {
                    // Buffer for the newly received data
                    byte[] ReceiveBuffer = new byte[ReceiveBufferSize];
                    try
                    {
                        // Receives data from the server, and stored the length 
                        // of the received data in bytes.
                        int Size = Socket.Receive(ReceiveBuffer, ReceiveBuffer.Length, SocketFlags.None);
                        // Check if data has correct header
                        if (Size < 4) { Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Incoming Packet is Corrupt"); }
                        // Parses the data into a message
                        Packet Received = new Packet(new Message(ReceiveBuffer.Take(Size).ToArray()), Socket.ProtocolType == ProtocolType.Udp);
                        // Queues the packet for processing
                        lock (ReceiveQueue) { ReceiveQueue.Enqueue(Received); }
                        // Check if the client is storing packets
                        if (StorePackets)
                        {
                            // Lock the packet store
                            // Add the received packet into the store
                            lock (ReceivedPackets) { ReceivedPackets.Add(Received); }
                        }
                        // Set the state of client to have not detected a disconnect or network fault
                        HasDetectedDisconnect = false;
                    }
                    catch (Exception Exception) // Catch any exception
                    {
                        // Iff the client is connected and has not detected a disconnect or network fault, output the fault to log
                        // (The iff check is only there so we don't bog the console)
                        if (IsConnected && !HasDetectedDisconnect)
                        {
                            Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Failed to receive from socket. Check network connectivity.");
                            Log.Exception(Log.Source.NETWORK, Exception);
                            HasDetectedDisconnect = true;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Processes packets/sends them to the parsing system.
        /// </summary>
        private static void ProcessPackets()
        {
            // While we need to continue the processing
            while (!StopProcesses)
            {
                // Sleep for the operation period
                Thread.Sleep(OperationPeriod);
                // Whether or not there are packets in the queue
                bool HasPackets;
                // Determines the length of the queue
                lock (ReceiveQueue) { HasPackets = ReceiveQueue.Count != 0; }
                if (HasPackets)
                {
                    // Locks the receive queue, grabs the receive queue packet
                    // and parses the message
                    Packet Processing;
                    lock (ReceiveQueue) { Processing = ReceiveQueue.Dequeue(); }
                    Parse.ParseMessage(Processing);
                }
            }
        }

        #endregion

        #region Send

        /// <summary>
        /// Sends a packet. Handles both UDP and TCP.
        /// Places packet into send queue to send
        /// later. Use SendNow for more important
        /// packets.
        /// </summary>
        /// <param name="SendPacket">Packet to send</param>
        /// <returns>Success of packet sending</returns>
        public static bool Send(Packet SendPacket)
        {
            // Check initialization status of Client.
            if (!Initialized) { throw new InvalidOperationException("Cannot use client before initialization. Call Client.Start();"); }
            // If we are not stopping
            if (!StopProcesses)
            {
                // Add packet to the send queue
                lock (SendQueue) { SendQueue.Enqueue(SendPacket); }
            }
            // Returns true, because the TCP data will keep getting retried until it succeeds
            // and we must assume that UDP packets are successful since there is no way to 
            // tell otherwise
            return true;
        }

        /// <summary>
        /// Sends a packet asynchronously, 
        /// handles both UDP and TCP Packets.
        /// </summary>
        /// <param name="SendPacket">Packet to send.</param>
        /// <returns>Success of packet sending.</returns>
        public static bool SendNow(Packet SendPacket)
        {
            // Sleep for the operation period
            Thread.Sleep(OperationPeriod);
            // Check initialization status of Client
            if (!Initialized) { throw new InvalidOperationException("Cannot use client before initialization. Call Client.Start();"); }
            // Checks the connection status of client
            if (IsConnected)
            {
                // Check if the Client is storing packets
                // If so, lock that packet store and add
                // the packet into the list
                if (StorePackets)
                {
                    lock (SentPackets) { SentPackets.Add(SendPacket); }
                }
                // Chooses the correct send method for the type of packet (TCP/UDP)
                if (SendPacket.IsUDP) { return SendUDPNow(SendPacket); }
                else { return SendTCPNow(SendPacket); }
            }
            else { return false; }
        }

        /// <summary>
        /// Assumes IsConnected is true.
        /// Sends a packet to the Server UDP
        /// port.
        /// </summary>
        /// <param name="UDPPacket">The Packet to send via UDP</param>
        /// <returns>True always, because there is no way to detect a successful UDP transmission</returns>
        private static bool SendUDPNow(Packet UDPPacket)
        {
            // Sends the data as a byte array
            byte[] Data = UDPPacket.Data.GetRawData();
            ServerUDP.Send(Data, Data.Length);
            // Returns true always, because there is no way to detect if a UDP
            // message is received
            return true;
        }

        /// <summary>
        /// Assumes IsConnected is true.
        /// Sends a packet to the Server TCP
        /// port.
        /// </summary>
        /// <param name="TCPPacket">The Packet to send via TCP</param>
        /// <returns>The success of the TCP transmission</returns>
        private static bool SendTCPNow(Packet TCPPacket)
        {
            // Get the packet's raw data
            byte[] Data = TCPPacket.Data.GetRawData();
            // Set the TCP Send buffer to the size of the data to avoid double buffering
            ServerTCP.SendBufferSize = Data.Length;
            // Try to send the TCP data to the client
            try { ServerTCP.Client.Send(Data); }
            catch (Exception Exception) // Catch any exception, but return that the transmission has failed
            {
                // Iff the client has not detected a possible disconnect log the exception
                if (!HasDetectedDisconnect) { Log.Exception(Log.Source.NETWORK, Exception); }
                // Reset the state of client to having detected a disconnect or network fault
                HasDetectedDisconnect = true;
                // Return the send process failed
                return false;
            }
            // (Re)set the state of client to have not detected a disconnect or network fault
            HasDetectedDisconnect = false;
            return true; // Return that the send process was successful
        }

        /// <summary>
        /// Iteratively sends packets to the server
        /// from the send queue.
        /// </summary>
        private static void SendPackets()
        {
            while (!StopProcesses)
            {
                bool HasPackets; // Stores whether or not the send queue has packets
                lock (SendQueue) { HasPackets = SendQueue.Count != 0; }
                // Determine if there are packets in the queue, if so, send them
                if (HasPackets)
                {
                    Packet ToSend; // Packet for sending
                    // Ensures that the packet has a non-null endpoint before clone
                    lock (SendQueue)
                    {
                        // Temporarily store the object in ToSend
                        ToSend = SendQueue.Peek();
                        // Check that the packet endpoint is not null, set to "Server" otherwise
                        ToSend.Endpoint = ToSend.Endpoint ?? "Server";
                        // Clone the packet so that we do not change the fields of the given packet (other than the Endpoint)
                        ToSend = (Packet)ToSend.Clone();
                    }
                    try
                    {
                        SendNow(ToSend); // Try to send the packet
                    }
                    catch (Exception Exception)
                    {
                        if (IsConnected) // Log an exception iff the client is supposedly connected (so we don't bog down the console)
                        {
                            Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Failed to send packet. Check connection status.");
                            Log.Exception(Log.Source.NETWORK, Exception);
                        }
                    }
                    lock (SendQueue) { SendQueue.Dequeue(); } // Dequeue the send packet

                }

                // Sleep for the operation period
                Thread.Sleep(OperationPeriod);
            }
        }

        /// <summary>
        /// Sends a Packet regardless of the connection
        /// status of Client. This method will throw an
        /// exception if you try and send a TCP exception
        /// without IsConnected being true.
        /// </summary>
        /// <param name="SendPacket">The Packet to send</param>
        /// <returns>Whether or not the packet was sent.</returns>
        internal static bool SendRegardless(Packet SendPacket)
        {
            // Check to make sure that you are not
            // sending a TCP connection without an
            // established connection
            if (SendPacket.IsUDP || IsConnected)
            {
                // Temporarily store the IsConnected variable
                bool Temp = IsConnected;
                // Set the state of Client to Connected
                IsConnected = true;
                // Send the packet
                bool Sent = SendNow(SendPacket);
                // Reset the state of Client
                IsConnected = Temp;
                // Return the success of the send process
                return Sent;
            }
            else
            {
                // Cannot send a TCP message without an established connection
                throw new InvalidOperationException("Must have a known, established connection to send a TCP packet");
            }
        }

        #endregion

        #region Info and Control

        /// <summary>
        /// Stops the Client.
        /// Removes all packets from the receuve and send queues.
        /// Changes the initialization state of the Client.
        /// </summary>
        public static void Stop()
        {
            StopProcesses = true;
            SendThread.Join();
            ProcessThread.Join();
            ReceiveThreadTCP.Join();
            ReceiveThreadUDP.Join();
            Initialized = false;
        }

        /// <summary>
        /// Gets the length of the current receive queue.
        /// (The processing queue) i.e. the packets in this 
        /// queue have yet to be parsed.
        /// </summary>
        /// <returns>The length of the recieve queue</returns>
        public static int GetReceiveQueueCount() { return ReceiveQueue.Count; }

        /// <summary>
        /// Gets the length of the current send queue.
        /// i.e. the packets in this 
        /// queue have yet to be sent.
        /// </summary>
        /// <returns>The length of the send queue</returns>
        public static int GetSendQueueCount() { return SendQueue.Count; }

        #endregion

    }
}
