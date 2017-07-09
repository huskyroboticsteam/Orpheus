using Scarlet.Utilities;
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
        private static TcpClient TcpClient;
        private static UdpClient UdpClient;
        private static Thread SendThread, ReceiveThread, ProcessThread;
        private static Queue<Packet> SendQueue, ReceiveQueue;
        private static bool Initialized = false;
        private static bool Stopping = false;
        private static int ReceiveBufferSize, OperationPeriod;
        private const int TIMEOUT = 5000;

        public static bool StorePackets;
        public static List<Packet> PacketsReceived { get; private set; }
        public static List<Packet> PacketsSent { get; private set; }
        public static bool TCPConnected { get; private set; }

        public static void Start(string ServerIP, int TCPTargetPort, int UDPTargetPort, int ReceiveBufferSize = 64, int OperationPeriod = 20)
        {
            SendQueue = new Queue<Packet>();
            ReceiveQueue = new Queue<Packet>();
            SendThread = new Thread(new ThreadStart(SendPackets));
            TcpClient = new TcpClient(new IPEndPoint(IPAddress.Parse(ServerIP), TCPTargetPort));
            UdpClient = new UdpClient(new IPEndPoint(IPAddress.Parse(ServerIP), UDPTargetPort));
            if (!TcpClient.Connected)
            {
                Log.Output(Log.Severity.INFO, Log.Source.NETWORK, "No TCP Server Found");
            }
        }

        public static void Stop()
        {

        }

        #region Receive

        #endregion

        #region Send

        public static bool Send(Packet SendPacket)
        {
            if (!Initialized) { throw new InvalidOperationException("Cannot use CommHandler before initialization. Call CommHandler.Start()."); }
            if (SendPacket.IsUDP) { return SendNow(SendPacket); }
            else { return SendTCP(SendPacket); }
        }

        public static bool SendNow(Packet SendPacket)
        {
            if (!Initialized) { throw new InvalidOperationException("Cannot use CommHandler before initialization. Call CommHandler.Start()."); }
            if (SendPacket.IsUDP)
            {
                int ByteSent = UdpClient.Send(SendPacket.GetForSend(), SendPacket.GetForSend().Length);
                return ByteSent != 0;
            }
            else
            { // Use TCP
                if (!TcpClient.Connected)
                {
                    Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Attemping to send TCP packet without TCP server connection. Check connection status.");
                } else
                {
                    TcpClient.GetStream().Write(SendPacket.GetForSend(), 0, SendPacket.GetForSend().Length);
                }
                return true;
            }
        }

        private static bool SendTCP(Packet SendPacket)
        {
            lock (SendQueue)
            {
                SendQueue.Enqueue(SendPacket);
            }
            return true;
        }

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
