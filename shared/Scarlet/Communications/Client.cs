﻿using Scarlet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scarlet.Communications
{
    public static class Client
    {
        private static Connection Connection;
        private static Thread SendThread, TCPReceiveThread, UDPReceiveThread, ProcessThread;
        private static Queue<Packet> SendQueue, ReceiveQueue;
        private static bool Initialized = false;
        private static bool Stopping = false;
        private static int ReceiveBufferSize, OperationPeriod;
        private const int TIMEOUT = 5000;

        public static bool StorePackets;
        public static List<Packet> PacketsReceived { get; private set; }
        public static List<Packet> PacketsSent { get; private set; }

        /// <summary>
        /// Starts a Client process.
        /// </summary>
        /// <param name="ServerIP">String representation of the IP Address of server.</param>
        /// <param name="TCPTargetPort">Target port for TCP Communications on the server.</param>
        /// <param name="UDPTargetPort">Target port for UDP Communications on the server.</param>
        /// <param name="ReceiveBufferSize">Size of buffer for incoming data.</param>
        /// <param name="OperationPeriod">Time in between receiving and sending individual packets.</param>
        public static void Start(string ServerIP, int TCPTargetPort, int UDPTargetPort, int ReceiveBufferSize = 64, int OperationPeriod = 20)
        {
            if (!Initialized)
            {
                SendQueue = new Queue<Packet>();
                ReceiveQueue = new Queue<Packet>();
                SendThread = new Thread(new ThreadStart(SendPackets));
                ProcessThread = new Thread(new ThreadStart(ProcessPackets));
				TCPReceiveThread = new Thread(new ParameterizedThreadStart(ReceiveFromSocket));
				UDPReceiveThread = new Thread(new ParameterizedThreadStart(ReceiveFromSocket));
            }
            Connection = new Connection(ServerIP, TCPTargetPort, UDPTargetPort);
            Client.ReceiveBufferSize = ReceiveBufferSize;
            Client.OperationPeriod = OperationPeriod;
            if (!Connection.TCPConnection.Connected) { Log.Output(Log.Severity.INFO, Log.Source.NETWORK, "No TCP Server Found"); }
            Initialized = true;
            StartThreads();
        }

        /// <summary>
        /// Starts all primary threads.
        /// </summary>
        private static void StartThreads()
        {
            SendThread.Start();
            TCPReceiveThread.Start(Connection.TCPConnection.Client);
            UDPReceiveThread.Start(Connection.UDPConnection.Client);
            ProcessThread.Start();
            SendThread.Join();
            TCPReceiveThread.Join();
            UDPReceiveThread.Join();
            ProcessThread.Join();
            Initialized = false;
        }

        /// <summary>
        /// Stops the Client completely.
        /// </summary>
        public static void Stop()
        {
            Stopping = true; // Invokes thread joining in StartThreads() due to thread loops (Recommended on SO)
            Connection.TCPConnection.GetStream().Close();
            Connection.TCPConnection.Close();
            Connection.UDPConnection.Close();
            Initialized = false; // Ensure initialized status is false when stopped
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
                    ReceiveFrom.Receive(ReceiveBuffer);
                    Packet Received = new Packet(new Message(ReceiveBuffer),
                                                 ReceiveFrom.ProtocolType == ProtocolType.Udp,
                                                 (IPEndPoint)Connection.TCPConnection.Client.RemoteEndPoint);
                    if (StorePackets) { PacketsReceived.Add(Received); }
                    lock (ReceiveQueue) { ReceiveQueue.Enqueue(Received); }
                    Thread.Sleep(OperationPeriod);
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
                lock(ReceiveQueue) { HasPackets = ReceiveQueue.Count != 0; }
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
            if (SendPacket.IsUDP) { return SendNow(SendPacket); }
            if (!Connection.TCPConnection.Connected)
            {
                Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Attemping to send TCP packet without TCP server connection. Check connection status.");
                return false;
            }
            else
            {
                lock (SendQueue) { SendQueue.Enqueue(SendPacket); }
                return true;
            }
        }

        /// <summary>
        /// Sends a packet asynchronously, 
        /// handles both UDP and TCP Packets.
        /// </summary>
        /// <param name="SendPacket">Packet to send.</param>
        /// <returns>Success of packet sending.</returns>
        public static bool SendNow(Packet SendPacket)
        {
            if (!Initialized) { throw new InvalidOperationException("Cannot use Client before initialization. Call Client.Start()."); }
            if (SendPacket.IsUDP)
            {
                int BytesSent = Connection.UDPConnection.Send(SendPacket.GetForSend(), SendPacket.GetForSend().Length);
                Thread.Sleep(OperationPeriod);
                if (BytesSent != 0 && StorePackets) { PacketsSent.Add(SendPacket); }
                return BytesSent != 0;
            }
            else
            { // Use TCP
                if (!Connection.TCPConnection.Connected)
                {
                    Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Attemping to send TCP packet without TCP server connection. Check connection status.");
                }
                else
                {
                    Connection.TCPConnection.GetStream().Write(SendPacket.GetForSend(), 0, SendPacket.GetForSend().Length);
                    if (StorePackets) { PacketsSent.Add(SendPacket); }
                    Thread.Sleep(OperationPeriod);
                }
                return true;
            }
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

    }

    class Connection
    {
        public UdpClient UDPConnection { get; private set; }
        public TcpClient TCPConnection { get; private set; }

        public Connection(string IP, int TCPPort, int UDPPort)
        {
            IPAddress IPAddr = IPAddress.Parse(IP);
            UDPConnection = new UdpClient(new IPEndPoint(IPAddr, UDPPort));
            TCPConnection = new TcpClient(new IPEndPoint(IPAddr, TCPPort));
        }
    }
}
