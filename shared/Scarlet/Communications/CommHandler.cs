using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Scarlet.Utilities;

namespace Scarlet.Communications
{
    public static class CommHandler
    {
        public static IPEndPoint DefaultTarget { get; private set; }
        private static TcpListener Listener;
        private static Queue<Packet> SendQueue, ReceiveQueue;
        private static Thread SendThread, ReceiveThread, ProcessThread;
        private static bool Initialized = false;
        private static bool Stopping;
		public static bool StorePackets { get; set; }
		public static List<Packet> PacketsReceived, PacketsSent; // Packets will only be added to this if StorePackets is true.
        private static int ReceiveBufferSize, OperationPeriod;
        private const int TIMEOUT = 5000;

        public static void Start(int ReceivePort, int DefaultSendPort, string DefaultSendIP, int ReceiveBufferSize = 64, int OperationPeriod = 20)
        {
            DefaultTarget = new IPEndPoint(IPAddress.Parse(DefaultSendIP), DefaultSendPort);
            IPEndPoint ListenerEndpoint = new IPEndPoint(IPAddress.Any, ReceivePort);
            CommHandler.ReceiveBufferSize = ReceiveBufferSize;
            CommHandler.OperationPeriod = OperationPeriod;
            if(!Initialized)
            {
                Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Initializing CommHandler.");
                Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Listening on port " + ListenerEndpoint.Port + ", and defaulting to sending to " + DefaultTarget.ToString() + ".");
                Listener = new TcpListener(ListenerEndpoint);
                SendQueue = new Queue<Packet>();
                ReceiveQueue = new Queue<Packet>();
				PacketsReceived = new List<Packet>();
				PacketsSent = new List<Packet>();
                ReceiveThread = new Thread(new ThreadStart(WaitForClient));
                ReceiveThread.Start();
                SendThread = new Thread(new ThreadStart(SendPackets));
                SendThread.Start();
                ProcessThread = new Thread(new ThreadStart(ProcessPackets));
                ProcessThread.Start();
                Initialized = true;
            }
            else { Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Attempted to initialize CommHandler twice."); }
            Stopping = false;
        }

        public static void Stop()
        {
			Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Stopping CommHandler.");
			Stopping = true;
        }

        #region Receive

        /// <summary>
        /// Waits for, and establishes connection with all incoming clients.
        /// This must be started on a thread, as it will block until CommHandler.Stopping is true.
        /// </summary>
        private static void WaitForClient()
        {
			if (!Initialized) { throw new InvalidOperationException("Cannot use CommHandler before initialization. Call CommHandler.Start()."); }
			Listener.Start();
            while(!CommHandler.Stopping)
            {
                // Wait for a client.
                TcpClient Client = Listener.AcceptTcpClient();
                Log.Output(Log.Severity.INFO, Log.Source.NETWORK, "Client has connected.");
                // Start sub-threads for every client.
                Thread ClientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                ClientThread.Start(Client);
                Thread.Sleep(OperationPeriod);
            }
        }

        /// <summary>
        /// Waits for, and receives data from a connected client.
        /// This must be started on a thread, as it will block until CommHandler.Stopping is true, or the client disconnects.
        /// </summary>
        /// <param name="ClientObj">The client to receive data from. Must be TcpClient.</param>
        private static void HandleClient(object ClientObj)
        {
            TcpClient Client = (TcpClient)ClientObj;
            NetworkStream Receive = Client.GetStream();
            if(!Receive.CanRead)
            {
                Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Client connection does not permit reading.");
                throw new Exception("NetworkStream does not support reading");
            }
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
                    if(DataSize >= 5)
                    {
                        byte[] Data = DataBuffer.Take(DataSize).ToArray();
                        Packet ReceivedPack = new Packet(new Message(Data), (IPEndPoint)Client.Client.RemoteEndPoint);
                        lock (ReceiveQueue)
                        {
                            ReceiveQueue.Enqueue(ReceivedPack);
                        }
						if(StorePackets) { PacketsReceived.Add(ReceivedPack); }
                    }
                    else { Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Data received from client was too short. Discarding."); }
                }
                catch(IOException IOExc)
                {
                    if(IOExc.InnerException is SocketException)
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
                catch(Exception OtherExc)
                {
                    Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Failed to read data from connected client.");
                    Log.Exception(Log.Source.NETWORK, OtherExc);
                }
                Thread.Sleep(OperationPeriod);
            }
            Receive.Close();
        }

        /// <summary>
        /// Pushes received packets through to Parse for processing.
        /// This must be started on a thread, as it will block until CommHandler.Stopping is true.
        /// Assumes that packets will not be removed from ReceiveQueue anywhere but inside this method.
        /// </summary>
        private static void ProcessPackets()
        {
			if (!Initialized) { throw new InvalidOperationException("Cannot use CommHandler before initialization. Call CommHandler.Start()."); }
			while (!Stopping)
            {
                bool HasPacket;
                lock (ReceiveQueue) { HasPacket = ReceiveQueue.Count > 0; }
                if(HasPacket)
                {
                    Packet ToProcess;
                    lock (ReceiveQueue) { ToProcess = (Packet)(ReceiveQueue.Dequeue().Clone()); }
                    try
                    {
                        Parse.ParseMessage(ToProcess.Data);
                    }
                    catch(Exception Exc)
                    {
                        Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Failed to process packet. Discarding.");
                        Log.Exception(Log.Source.NETWORK, Exc);
                    }
                }
                Thread.Sleep(OperationPeriod);
            }
        }

        #endregion

        #region Send

        /// <summary>
        /// Adds a packet to the queue of packets to be sent.
        /// </summary>
        public static void Send(Packet Packet)
        {
			if (!Initialized) { throw new InvalidOperationException("Cannot use CommHandler before initialization. Call CommHandler.Start()."); }
			lock (SendQueue)
            {
                SendQueue.Enqueue(Packet);
            }
        }

        /// <summary>
        /// Immediately sends a packet. Blocks until sending is complete.
        /// </summary>
        /// <param name="Packet"></param
        public static void SendNow(Packet ToSend)
        {
			if (!Initialized) { throw new InvalidOperationException("Cannot use CommHandler before initialization. Call CommHandler.Start()."); }
			TcpClient Client = new TcpClient(new IPEndPoint(IPAddress.Any, 8000)); // TODO: See if this port needs to be configurable.
            if (Client.ConnectAsync(ToSend.Endpoint.Address, ToSend.Endpoint.Port).Wait(TIMEOUT))
            {
                byte[] Data = ToSend.GetForSend();
                NetworkStream Stream = Client.GetStream();
                Stream.Write(Data, 0, Data.Length);
                Stream.Close();
                Client.Close();
				if (StorePackets) { PacketsSent.Add(ToSend); }
			}
            else { throw new TimeoutException("Connection timed out while trying to send packet."); }
        }

        /// <summary>
        /// Sends packets from the queue.
        /// This must be started on a thread, as it will block until CommHandler.Stopping is true.
        /// Assumes that packets will not be removed from SendQueue anywhere but inside this method.
        /// </summary>
        private static void SendPackets()
        {
			if (!Initialized) { throw new InvalidOperationException("Cannot use CommHandler before initialization. Call CommHandler.Start()."); }
			while (!Stopping)
            {
                bool HasPacket;
                lock(SendQueue) { HasPacket = SendQueue.Count > 0; }
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
    }
}
