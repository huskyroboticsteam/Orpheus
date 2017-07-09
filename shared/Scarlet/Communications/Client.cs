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

        public static void Start(string ServerIP, int TCPTargetPort, int UDPTargetPort, int ReceiveBufferSize = 64, int OperationPeriod = 20)
        {
            TcpClient = new TcpClient(new IPEndPoint(IPAddress.Parse(ServerIP), TCPTargetPort));
            UdpClient = new UdpClient(new IPEndPoint(IPAddress.Parse(ServerIP), UDPTargetPort));
        }

        public static void Stop()
        {
            
        }

        #region Receive

        #endregion

        #region Send
        
        public static bool Send(Packet SendPacket)
        {
            return true;
        }

        public static bool SendNow(Packet SendPacket)
        {
            return true;
        }

        private static bool SendTCP(Packet SendPacket)
        {
            return true;
        }

        private static bool SendUDP(Packet SendPacket)
        {
            return true;
        }
        
        #endregion

        #region Info
        public static int GetReceiveQueueLength() { return ReceiveQueue.Count; }
        public static int GetSendQueueLength() { return SendQueue.Count; }
        #endregion

    }
}
