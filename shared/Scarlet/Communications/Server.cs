using Scarlet.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scarlet.Communications
{
    public static class Server
    {
        private static Dictionary<IPEndPoint, TcpClient> ClientsTCP;
        private static Dictionary<IPEndPoint, UdpClient> ClientsUDP;
        private static Dictionary<IPEndPoint, Queue<Packet>> SendQueues;
        private static Queue<Packet> ReceiveQueue;
        private static Dictionary<IPEndPoint, Thread> SendThreads;
        private static Thread ReceiveThreadTCP, ReceiveThreadUDP, ProcessThread;
        private static bool Initialized = false;
        private static bool Stopping = false;
        public static bool StorePackets;
        public static List<Packet> PacketsReceived, PacketsSent;
        private static int ReceiveBufferSize, OperationPeriod;
        private const int TIMEOUT = 5000;

        /// <summary>
        /// Prepares the server for use, and starts listening for clients.
        /// </summary>
        /// <param name="PortTCP">The port to listen on for clients communicating via TCP.</param>
        /// <param name="PortUDP">The port to listen on for clients communicating via UDP.</param>
        /// <param name="ReceiveBufferSize">The size, in bytes, of the receive data buffers. Increase this if your packets are longer than the default.</param>
        /// <param name="OperationPeriod">The time, in ms, between network operations. If you are sending/receiving a lot of packets, and notice delays, lower this.</param>
        public static void Start(int PortTCP, int PortUDP, int ReceiveBufferSize = 64, int OperationPeriod = 20)
        {
            Server.ReceiveBufferSize = ReceiveBufferSize;
            Server.OperationPeriod = OperationPeriod;
            if (!Initialized)
            {
                Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Initializing Server.");
                Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Listening on ports " + PortTCP + " (TCP), and " + PortUDP + " (UDP).");
                SendQueues = new Dictionary<IPEndPoint, Queue<Packet>>();
                SendThreads = new Dictionary<IPEndPoint, Thread>();

                ReceiveQueue = new Queue<Packet>();

                PacketsSent = new List<Packet>();
                PacketsReceived = new List<Packet>();
                Initialized = true;
                new Thread(new ParameterizedThreadStart(StartThreads)).Start(new Tuple<int, int>(PortTCP, PortUDP));
            }
            else { Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Attempted to start Server when already started."); }
        }

        private static void StartThreads(object Ports)
        {
            ReceiveThreadTCP = new Thread(new ParameterizedThreadStart(WaitForClientsTCP));
            ReceiveThreadTCP.Start(((Tuple<int, int>)Ports).Item1);

            ReceiveThreadUDP = new Thread(new ParameterizedThreadStart(WaitForClientsUDP));
            ReceiveThreadUDP.Start(((Tuple<int, int>)Ports).Item2);

            //ProcessThread = new Thread(new ThreadStart(ProcessPackets));
            //ProcessThread.Start();

            ReceiveThreadTCP.Join();
            ReceiveThreadUDP.Join();
            ProcessThread.Join();
            Initialized = false;
        }

        public static void Stop()
        {
            Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Stopping Server.");
            Stopping = true;
            while (Initialized) { Thread.Sleep(50); } // Wait for all threads to stop.
        }

        private static void WaitForClientsTCP(object ReceivePort)
        {
            if (!Initialized) { throw new InvalidOperationException("Cannot use Server before initialization. Call Server.Start()."); }
            TcpListener Listener = new TcpListener(new IPEndPoint(IPAddress.Any, (int)ReceivePort));
            Listener.Start();
            while (!Stopping)
            {
                // Wait for a client.
                Task<TcpClient> ClientListener = Listener.AcceptTcpClientAsync();
                if (ClientListener.Wait(5000))
                {
                    TcpClient Client = ClientListener.Result;
                    Log.Output(Log.Severity.INFO, Log.Source.NETWORK, "Client has connected.");
                    // Start sub-threads for every client.
                    Thread ClientThread = new Thread(new ParameterizedThreadStart(HandleTCPClient));
                    ClientThread.Start(Client);
                    Thread.Sleep(OperationPeriod);
                }
            }
            Listener.Stop();
        }

        /// <summary>
        /// Waits for, and receives data from a connected TCP client.
        /// This must be started on a thread, as it will block until CommHandler.Stopping is true, or the client disconnects.
        /// </summary>
        /// <param name="ClientObj">The client to receive data from. Must be TcpClient.</param>
        private static void HandleTCPClient(object ClientObj)
        {
            TcpClient Client = (TcpClient)ClientObj;
            NetworkStream Receive = Client.GetStream();
            if (!Receive.CanRead)
            {
                Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Client connection does not permit reading.");
                throw new Exception("NetworkStream does not support reading");
            }
            lock (ClientsTCP) { ClientsTCP.Add((IPEndPoint)Client.Client.RemoteEndPoint, Client); }
            byte[] DataBuffer = new byte[ReceiveBufferSize];
            while (!Stopping)
            {
                try
                {
                    int DataSize = Receive.Read(DataBuffer, 0, DataBuffer.Length);
                    Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Received data from client.");
                    if (DataSize == 0)
                    {
                        Log.Output(Log.Severity.INFO, Log.Source.NETWORK, "Client has disconnected.");
                        break;
                    }
                    if (DataSize >= 5)
                    {
                        byte[] Data = DataBuffer.Take(DataSize).ToArray();
                        Packet ReceivedPack = new Packet(new Message(Data), false, (IPEndPoint)Client.Client.RemoteEndPoint);
                        lock (ReceiveQueue) { ReceiveQueue.Enqueue(ReceivedPack); }
                        if (StorePackets) { PacketsReceived.Add(ReceivedPack); }
                    }
                    else { Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Data received from client was too short. Discarding."); }
                }
                catch (IOException IOExc)
                {
                    if (IOExc.InnerException is SocketException)
                    {
                        int Error = ((SocketException)IOExc.InnerException).ErrorCode;
                        Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Failed to read data from connected client with SocketExcpetion code " + Error);
                        Log.Exception(Log.Source.NETWORK, IOExc);
                    }
                    else
                    {
                        Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Failed to read data from connected client because of IO exception.");
                        Log.Exception(Log.Source.NETWORK, IOExc);
                    }
                }
                catch (Exception OtherExc)
                {
                    Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Failed to read data from connected client.");
                    Log.Exception(Log.Source.NETWORK, OtherExc);
                }
                Thread.Sleep(OperationPeriod);
            }
            ClientsTCP.Remove((IPEndPoint)Client.Client.RemoteEndPoint);
            Receive.Close();
        }

        public static List<IPEndPoint> GetTCPClients() { return ClientsTCP.Keys.ToList(); }

        private static void WaitForClientsUDP(object ReceivePort)
        {

        }
    }
}
