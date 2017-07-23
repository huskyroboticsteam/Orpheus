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
        private static IPAddress DestinationIP;
        private static Thread SendThread;
        private static Thread ReceiveThreadTCP;
        private static Thread ReceiveThreadUDP;
        private static Thread ProcessThread;
        private static Queue<Packet> SendQueue, ReceiveQueue;
        private static int PortUDP, PortTCP;
        private static bool Initialized;
        private static bool Stopping;
        private static int ReceiveBufferSize, OperationPeriod;
        private const int TIMEOUT = 5000;
        private const int RECONNECT_DELAY_TIMEOUT = 500; // In ms

        public static bool StorePackets;
        public static List<Packet> PacketsReceived { get; private set; }
        public static List<Packet> PacketsSent { get; private set; }
        public static string Name { get; private set; }
        public static bool IsConnected { get; private set; }

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
            Stopping = false;
            if (!Initialized)
            {
                PacketHandler.Start();
                WatchdogManager.ConnectionChanged += ChangeConnectionStatus;
                SendQueue = new Queue<Packet>();
                ReceiveQueue = new Queue<Packet>();
                SendThread = new Thread(new ThreadStart(SendPackets));
                ProcessThread = new Thread(new ThreadStart(ProcessPackets));
                ReceiveThreadTCP = new Thread(new ParameterizedThreadStart(ReceiveFromSocket));
                ReceiveThreadUDP = new Thread(new ParameterizedThreadStart(ReceiveFromSocket));
            }
            Client.PortTCP = PortTCP;
            Client.PortUDP = PortUDP;
            Client.Name = Name;
            DestinationIP = IPAddress.Parse(ServerIP);
            Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Attempting to connect to Server on Ports " + PortTCP + " (TCP) and " + PortUDP + " (UDP).");
            ClientTCP = new TcpClient();
            ClientUDP = new UdpClient();
            Connect();
            Client.ReceiveBufferSize = ReceiveBufferSize;
            Client.OperationPeriod = OperationPeriod;
            Initialized = true;
            new Thread(new ThreadStart(StartThreads)).Start();
        }

        /// <summary>
        /// Connects TCP and UDP clients to 
        /// server. Logs errors if they occur.
        /// </summary>
        private static void Connect()
        {
            ClientTCP = new TcpClient();
            ClientUDP = new UdpClient();
            try
            {
                ClientTCP.Connect(new IPEndPoint(DestinationIP, PortTCP));
            }
            catch (SocketException Exception)
            {
                Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Could not connect to TCP Server.");
                Log.Exception(Log.Source.NETWORK, Exception);
                Stop();
                new Thread(new ThreadStart(ListenForReconnect)).Start();
                return;
            }
            try
            {
                ClientUDP.Connect(new IPEndPoint(DestinationIP, PortUDP));
            }
            catch (SocketException Exception)
            {
                Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Could not connect to UDP Server.");
                Log.Exception(Log.Source.NETWORK, Exception);
                Stop();
                new Thread(new ThreadStart(ListenForReconnect)).Start();
                return;
            }
            Log.Output(Log.Severity.INFO, Log.Source.NETWORK, "TCP and UDP Clients Connected to Server. Using ports " + PortTCP.ToString() + " and " + PortUDP + " respectively.");
            SendName();
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

        private static void ListenForReconnect()
        {
            while (!ClientTCP.Connected)
            {
                try
                {
                    lock (ClientTCP) { ClientTCP.Connect(new IPEndPoint(DestinationIP, PortTCP)); }
                    lock (ClientUDP) { ClientUDP.Connect(new IPEndPoint(DestinationIP, PortUDP)); }
                }
                catch
                {
                    Thread.Sleep(OperationPeriod);
                }
            }
            if (Stopping) { Restart(); }
        }

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
        /// Stops the Client completely.
        /// </summary>
        public static void Stop()
        {
            Stopping = true; // Invokes thread joining in StartThreads() due to thread loops (Recommended on SO)
            ClientTCP.GetStream().Close();
            ClientTCP.Close();
            ClientUDP.Close();
            Initialized = false; // Ensure initialized status is false when stopped
        }

        public static void Restart()
        {
            IPEndPoint Endpoint = (IPEndPoint)ClientTCP.Client.RemoteEndPoint;
            Start(Endpoint.Address.ToString(), Endpoint.Port, ((IPEndPoint)ClientUDP.Client.RemoteEndPoint).Port, Name, ReceiveBufferSize, OperationPeriod);
        }

        #region Receive

        /// <summary>
        /// Continuously receives from socket, until 
        /// Client.Stopping is true. Automatically distributes
        /// incoming messages to approprate locations.
        /// </summary>
        /// <param name="Socket">Socket to receive on.</param>
        private static void ReceiveFromSocket(object Socket)
        {
            Socket ReceiveFrom = (Socket)Socket;
            while (!Stopping)
            {
                if (ReceiveFrom.Available > 0)
                {
                    byte[] ReceiveBuffer = new byte[Client.ReceiveBufferSize];
                    try
                    {
                        Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Receiving from socket...");
                        int Size = ReceiveFrom.Receive(ReceiveBuffer);
                        Packet Received = new Packet(new Message(ReceiveBuffer.Take(Size).ToArray()), ReceiveFrom.ProtocolType == ProtocolType.Udp);
                        Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Received: " + Received.ToString());
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
            if (!Stopping)
            {
                if (!Initialized) { throw new InvalidOperationException("Cannot use Client before initialization. Call Client.Start()."); }
                if (SendPacket.IsUDP)
                {
                    int BytesSent = 0;
                    try
                    {
                        BytesSent = ClientUDP.Send(SendPacket.GetForSend(), SendPacket.GetForSend().Length);
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
                            ClientTCP.GetStream().Write(SendPacket.GetForSend(), 0, SendPacket.GetForSend().Length);
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
        /// are in the send queue.
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

        private static void ChangeConnectionStatus(object Sender, ConnectionStatusChanged Event)
        {
            IsConnected = Event.StatusConnected;
            if (!Event.StatusConnected)
            {
                Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Server was disconnected or interuppted, stopping all transmissions and attempting to reconnect...");
                Stop();
                ListenForReconnect();
            }
            else
            {
                if (Stopping) { Restart(); }
                Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Server (re)connected...");
            }
        }

    }
}
