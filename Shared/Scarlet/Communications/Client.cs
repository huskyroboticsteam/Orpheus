using Scarlet.Utilities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace Scarlet.Communications
{

    public static class Client
    {
        private static TcpClient ClientTCP;
        private static UdpClient ClientUDP;
        private static Thread SendThread;
        private static Thread ReceiveThreadTCP;
        private static Thread ReceiveThreadUDP;
        private static Thread ProcessThread;
        private static Queue<Packet> SendQueue, ReceiveQueue;
        private static IPEndPoint EndpointUDP, EndpointTCP;
        private static bool Initialized, HasConnected, Stopping;
        private static int ReceiveBufferSize, OperationPeriod;
        private const int TIMEOUT = 5000; // In ms
        private const int RECONNECT_DELAY_TIMEOUT = 500; // In ms

        public static bool StorePackets;
        public static List<Packet> PacketsReceived { get; private set; }
        public static List<Packet> PacketsSent { get; private set; }
        public static string Name { get; private set; }
        public static bool IsConnected { get; private set; }
        public static bool OutputWatchdogDebug = false;

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
            Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Initializing Client.");
            Client.Name = Name;
            Client.ReceiveBufferSize = ReceiveBufferSize;
            Client.OperationPeriod = OperationPeriod;
            IPAddress DestinationIP = IPAddress.Parse(ServerIP);
            Client.EndpointTCP = new IPEndPoint(DestinationIP, PortTCP);
            Client.EndpointUDP = new IPEndPoint(DestinationIP, PortUDP);
            Client.Stopping = false;
            if (!Initialized)
            {
                PacketHandler.Start();
                WatchdogManager.ConnectionChanged += ChangeConnectionStatus;
                Client.SendQueue = new Queue<Packet>();
                Client.ReceiveQueue = new Queue<Packet>();
                Client.SendThread = new Thread(new ThreadStart(SendPackets));
                Client.ProcessThread = new Thread(new ThreadStart(ProcessPackets));
                Client.ReceiveThreadTCP = new Thread(new ParameterizedThreadStart(ReceiveFromSocket));
                Client.ReceiveThreadUDP = new Thread(new ParameterizedThreadStart(ReceiveFromSocket));
                Client.ClientTCP = new TcpClient();
                Client.ClientUDP = new UdpClient();
            }
            Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Attempting to connect to Server on Ports " + PortTCP + " (TCP) and " + PortUDP + " (UDP).");
            Connect();
            Initialized = true;
        }

        #region Internal

        /// <summary>
        /// Connects TCP and UDP clients to 
        /// server. Logs errors if they occur.
        /// </summary>
        private static void Connect()
        {
            if (HasConnected) { return; }
            try { ClientTCP.Connect(EndpointTCP); }
            catch (SocketException Exception)
            {
                Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Could not connect to TCP Server.");
                Log.Exception(Log.Source.NETWORK, Exception);
                Reconnect();
                return;
            }
            try { ClientUDP.Connect(EndpointUDP); }
            catch (SocketException Exception)
            {
                Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Could not connect to UDP Server.");
                Log.Exception(Log.Source.NETWORK, Exception);
                Reconnect();
                return;
            }
            Log.Output(Log.Severity.INFO, Log.Source.NETWORK, "TCP and UDP Clients Connected to Server. UDP Port " + EndpointUDP.Port.ToString() + " and TCP Port " + EndpointTCP.Port.ToString());
            Reconnect();
            IsConnected = true;
            ClientTCP.Client.SendTimeout = TIMEOUT; // Set send timeout for TCP Client
        }

        /// <summary>
        /// Sends the name of the client to the 
        /// server.
        /// </summary>
        private static void SendName()
        {
            byte[] Bytes = UtilData.ToBytes(Name);
            ClientTCP.Client.Send(Bytes);
            ClientUDP.Client.Send(Bytes);
        }

        /// <summary>
        /// Blocks until reconnect found
        /// </summary>
        private static void ListenForReconnect()
        {
            Stopping = true;
            bool ConnectionFound = false;
            Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Listening for connection...");
            while (!ConnectionFound)
            {
                try
                {
                    SendName();
                    Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Connection found...");
                    ConnectionFound = true;
                }
                catch { Thread.Sleep(RECONNECT_DELAY_TIMEOUT); }
            }
            Stopping = false;
            HasConnected = true;
            Restart();
        }

        /// <summary>Starts a thread to reconnect the Server and Client</summary>
        private static void Reconnect() { new Thread(new ThreadStart(ListenForReconnect)).Start(); }
        /// <summary>Starts all primary threads.</summary>
        private static void Restart() { new Thread(new ThreadStart(StartThreads)).Start(); }

        /// <summary>
        /// Starts all primary threads.
        /// </summary>
        private static void StartThreads()
        {
            SendThread.Start();
            ReceiveThreadTCP.Start(ClientTCP.Client);
            ReceiveThreadUDP.Start(ClientUDP.Client);
            ProcessThread.Start();
            SendThread.Join();
            ReceiveThreadTCP.Join();
            ReceiveThreadUDP.Join();
            ProcessThread.Join();
            Initialized = false;
        }

        /// <summary>
        /// Happens when watchdog timer sees a connection change.
        /// </summary>
        /// <param name="Sender">Event sender parameter, usually "Watchdog Timer" in this case.</param>
        /// <param name="Event">The event passed to the handler.</param>
        private static void ChangeConnectionStatus(object Sender, ConnectionStatusChanged Event)
        {
            IsConnected = Event.StatusConnected;
            if (!Event.StatusConnected)
            {
                Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Server was disconnected or interuppted, stopping all transmissions and attempting to reconnect...");
                Reconnect();
            }
            else { Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Server (re)connected..."); }
        }

        #endregion

        #region Receive

        /// <summary>
        /// Continuously receives from socket, until 
        /// Client.Stopping is true. Automatically distributes
        /// incoming messages to approprate locations.
        /// </summary>
        /// <param name="Socket">Socket to receive on. Must be of type Socket</param>
        private static void ReceiveFromSocket(object Socket)
        {
            Socket ReceiveFrom = (Socket)Socket;
            while (!Stopping)
            {
                if (ReceiveFrom.Available > 0 && ClientTCP.Connected)
                {
                    byte[] ReceiveBuffer = new byte[ReceiveBufferSize];
                    try
                    {
                        Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Receiving from socket...");
                        int Size = ReceiveFrom.Receive(ReceiveBuffer);
                        Packet Received = new Packet(new Message(ReceiveBuffer.Take(Size).ToArray()), ReceiveFrom.ProtocolType == ProtocolType.Udp);
                        bool Output = true;
                        if (Received.Data.ID == Constants.WATCHDOG_PING) { Output = OutputWatchdogDebug; }
                        if (Output) { Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Received: " + Received.ToString()); }
                        if (StorePackets) { PacketsReceived.Add(Received); }
                        lock (ReceiveQueue) { ReceiveQueue.Enqueue(Received); }
                        Thread.Sleep(OperationPeriod);
                    }
                    catch (SocketException Exception)
                    {
                        Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Failed to receive from socket. Check connection status.");
                        Log.Exception(Log.Source.NETWORK, Exception);
                    }
                }
            }
        }

        /// <summary>
        /// Handles packets as they are received
        /// from server.
        /// </summary>
        private static void ProcessPackets()
        {
            while (!Stopping)
            {
                bool HasPackets = false;
                lock (ReceiveQueue) { HasPackets = ReceiveQueue.Count != 0; }
                if (HasPackets)
                {
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
        /// </summary>
        /// <param name="SendPacket">Packet to send</param>
        /// <returns>Success of packet sending</returns>
        public static bool Send(Packet SendPacket)
        {
            if (!Initialized) { throw new InvalidOperationException("Cannot use Client before initialization. Call Client.Start()."); }
            if (!Stopping)
            {
                if (SendPacket.IsUDP) { return SendNow(SendPacket); }
                lock (SendQueue) { SendQueue.Enqueue(SendPacket); }
            }
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
            if (SendPacket.Endpoint == null) { SendPacket.Endpoint = "Server"; }
            if (!Stopping)
            {
                if (!Initialized) { throw new InvalidOperationException("Cannot use Client before initialization. Call Client.Start()."); }
                if (SendPacket.IsUDP)
                {
                    int BytesSent = 0;
                    try
                    {
                        lock (ClientUDP) { BytesSent = ClientUDP.Send(SendPacket.GetForSend(), SendPacket.GetForSend().Length); }
                    }
                    catch (SocketException Exception)
                    {
                        Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "An error occurred when accessing the socket. Check connection status.");
                        Log.Exception(Log.Source.NETWORK, Exception);
                        return false;
                    }
                    catch (ObjectDisposedException Exception)
                    {
                        Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Client UDP socket stream is closed. Consider restart, check connection status.");
                        Log.Exception(Log.Source.NETWORK, Exception);
                        return false;
                    }
                    Thread.Sleep(OperationPeriod);
                    if (BytesSent != 0 && StorePackets) { PacketsSent.Add(SendPacket); }
                    return true;
                }
                else
                { // Use TCP
                    if (!ClientTCP.Connected)
                    {
                        Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Attemping to send TCP packet without TCP server connection. Check connection status.");
                        return false;
                    }
                    else
                    {
                        try
                        {
                            lock (ClientTCP)
                            {
                                ClientTCP.GetStream().Write(SendPacket.GetForSend(), 0, SendPacket.GetForSend().Length);
                            }
                        }
                        catch (IOException Exception)
                        {
                            Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Failed to write to socket stream. Check connection status.");
                            Log.Exception(Log.Source.NETWORK, Exception);
                            return false;
                        }
                        catch (ObjectDisposedException Exception)
                        {
                            Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Client TCP socket stream is closed. Consider restart, check connection status.");
                            Log.Exception(Log.Source.NETWORK, Exception);
                            return false;
                        }
                        if (StorePackets) { PacketsSent.Add(SendPacket); }
                        Thread.Sleep(OperationPeriod);
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Iteratively sends packets that 
        /// are in the send queue. Blocks
        /// while Client is running.
        /// </summary>
        private static void SendPackets()
        {
            while (!Stopping)
            {
                bool HasPacket;
                lock (SendQueue) { HasPacket = SendQueue.Count > 0; }
                if (HasPacket)
                {
                    Packet ToSend;
                    lock (SendQueue) { ToSend = (Packet)(SendQueue.Peek().Clone()); }
                    try
                    {
                        SendNow(ToSend);
                        lock (SendQueue) { SendQueue.Dequeue(); } // Remove the packet from the queue when it has been sent sucessfully.
                    }
                    catch (Exception Exc)
                    {
                        Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Failed to send packet.");
                        Log.Exception(Log.Source.NETWORK, Exc);
                    }
                }
                Thread.Sleep(OperationPeriod);
            }
        }

        #endregion

        #region Info
        public static int GetReceiveQueueLength() { return ReceiveQueue.Count; }
        public static int GetSendQueueLength() { return SendQueue.Count; }
        #endregion

    }
}
