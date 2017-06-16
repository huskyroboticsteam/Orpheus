using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using RoboticsLibrary.Errors;
using RoboticsLibrary.Utilities;

namespace RoboticsLibrary.Communications
{
    /// <summary>
    /// CommsHandler
    /// handles communications
    /// send/receive
    /// including parsing information
    /// from source.
    /// </summary>
    public static class CommHandler
    {

        private static TcpListener TcpListener;  // TcpListener for comms activity
        private static IPEndPoint Endpoint;      // Comms endpoint
        private static Queue<Packet> SendQueue;  // Send queue for comms 
        private static Thread SendThread, ReceiveThread;
        private static float PacketIntervalTime; // Minimum time in between packet sending. (Is not accounted for in SendAsyncPacket)
        private static int ReceiveBufferSize, PortNumber;
        private static bool Continue;    // Whether or not the process continues
        private static bool Initialized; // Whether or not the system is initialized

        /// <summary>
        /// Starts the CommHandler
        /// </summary>
        /// <param name="Port">
        /// Port to listen on</param>
        /// <param name="CycleTime">
        /// Minimum cycle time in between packet send/receive</param>
        /// <param name="ReceiveBufferSize">
        /// Buffer receive size.</param>
        /// <returns>
        /// Returns true is initialization succeeded, false otherwise.
        /// </returns>
        public static bool Start(int Port = 2024, float CycleTime = 0.02f, int ReceiveBufferSize = 64)
        {
            CommHandler.PacketIntervalTime = CycleTime;
            CommHandler.ReceiveBufferSize = ReceiveBufferSize;
            CommHandler.PortNumber = Port;
            CommHandler.Continue = true;
            if (!CommHandler.Initialized)
            {
                return CommHandler.Initialize();
            }
            return true;            
        }

        /// <summary>
        /// Initializes CommHandler (internal use only)
        /// </summary>
        /// <returns>
        /// Returns whether or not initialization succeeded
        /// </returns>
        private static bool Initialize()
        {
            CommHandler.Initialized = true;
            try
            {
                Log.Output(Log.Severity.DEBUG, Log.Source.NETWORK, "Initializing CommHandler to listen on port " + CommHandler.PortNumber + ".");
                CommHandler.Endpoint = new IPEndPoint(IPAddress.Parse("0.0.0.0"), CommHandler.PortNumber);
                CommHandler.TcpListener = new TcpListener(CommHandler.Endpoint);
                CommHandler.SendQueue = new Queue<Packet>();
                CommHandler.SendThread = new Thread(new ThreadStart(CommHandler.Send));
                CommHandler.ReceiveThread = new Thread(new ThreadStart(CommHandler.Receive));
                CommHandler.TcpListener.Start();
                CommHandler.SendThread.Start();
                CommHandler.ReceiveThread.Start();
            }
            catch (Exception Except)
            {
                Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Could not initialize CommHandler.");
                Log.Exception(Log.Source.NETWORK, Except);
                CommHandler.Initialized = false;
            }
            return CommHandler.Initialized;
        }

        /// <summary>
        /// Stops communications.
        /// </summary>
        public static void Stop()
        {
            CommHandler.Continue = false;
        }

        /// <summary>
        /// Use for restarting Comms.
        /// * * * Important!!
        /// </summary>
        public static void Restart()
        {
            if (!CommHandler.Continue)
            {
                CommHandler.Continue = true;
                CommHandler.Start(CommHandler.PortNumber, 
                                  CommHandler.PacketIntervalTime, 
                                  CommHandler.ReceiveBufferSize);
            }
        }

        /// <summary>
        /// Adds packet to send buffer.
        /// </summary>
        /// <param name="Packet">Packet to add.</param>
        public static void AddCyclePacket(Packet Packet)
        {
            CommHandler.SendQueue.Enqueue(Packet);
        }

        /// <summary>
        /// Sends a packet out of phase with
        /// the send cycles.
        /// </summary>
        /// <param name="Packet">Packet to send asynchronously</param>
        /// <returns>
        /// Whether or not the send succeeded.
        /// </returns>
        public static bool SendAsyncPacket(Packet Packet)
        {
            try
            {
                Packet.Send();
            }
            catch (Exception Except)
            {
                Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Failed to send packet.");
                Log.Exception(Log.Source.NETWORK, Except);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Internal use only.
        /// Sends the next packet in the queue.
        /// </summary>
        /// <returns>
        /// Whether or not the send succeeded.</returns>
        private static bool SendNextPacket()
        {
            try
            {
                if (CommHandler.SendQueue.Count > 0)
                {
                    CommHandler.SendQueue.Dequeue().Send();
                }
            }
            catch (Exception Except)
            {
                Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Failed to send next packet.");
                Log.Exception(Log.Source.NETWORK, Except);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Send loop start.
        /// Internal use only.
        /// Meant for threading.
        /// </summary>
        private static void Send()
        {
            while (CommHandler.Continue)
            {
                // Send the next packet
                CommHandler.SendNextPacket();
                // Sleep for specified time interval
                // in-between packet sending
                Thread.Sleep((int)(CommHandler.PacketIntervalTime * 1000));
            }
        }

        /// <summary>
        /// Receives messages and sends them
        /// to be parsed.
        /// </summary>
        private static void Receive()
        {
            while (CommHandler.Continue)
            {
                try
                {
                    Socket Socket = TcpListener.AcceptSocket(); // Blocking method waits for socket connection
                    byte[] BytesReceived = new byte[CommHandler.ReceiveBufferSize];
                    Socket.Receive(BytesReceived); // Stores received byes into byte buffer
                    Message Received = new Message(BytesReceived, (IPEndPoint)(Socket.RemoteEndPoint));
                    Parse.ParseMessage(Received);
                }
                catch (Exception Except)
                {
                    Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Failed to receive packet.");
                    Log.Exception(Log.Source.NETWORK, Except);
                }
                
            }
        }

    }
}
