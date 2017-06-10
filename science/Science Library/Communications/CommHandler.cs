using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using RoboticsLibrary.Errors;

namespace RoboticsLibrary.Communications
{
    public static class CommHandler
    {

        private static TcpListener TcpListener;
        private static IPEndPoint Endpoint;
        private static Queue<Packet> SendQueue;
        private static Thread SendThread, ReceiveThread;
        private static float PacketIntervalTime; // Minimum time in between packet sending. (Is not accounted for in SendAsyncPacket)
        private static int ReceiveBufferSize, PortNumber;
        private static bool Continue;    // Whether or not the process continues
        private static bool Initialized; // Whether or not the system is initialized

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

        private static bool Initialize()
        {
            try
            {
                CommHandler.Endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), CommHandler.PortNumber);
                CommHandler.TcpListener = new TcpListener(CommHandler.Endpoint);
                CommHandler.SendQueue = new Queue<Packet>();
                CommHandler.SendThread = new Thread(new ThreadStart(CommHandler.Send));
                CommHandler.ReceiveThread = new Thread(new ThreadStart(CommHandler.Send));
                CommHandler.TcpListener.Start();
                CommHandler.SendThread.Start();
                CommHandler.ReceiveThread.Start();
            }
            catch (Exception e)
            {
                ErrorHandler.Throw(e);
                return false;
            }
            return true;
        }

        public static void Stop()
        {
            CommHandler.Continue = false;
        }

        /// <summary>
        /// Use for restarting system. Important!!
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

        public static void AddCyclePacket(Packet Packet)
        {
            CommHandler.SendQueue.Enqueue(Packet);
        }

        public static bool SendAsyncPacket(Packet Packet)
        {
            try
            {
                Packet.Send();
            }
            catch (Exception e)
            {
                ErrorHandler.Throw(e);
                return false;
            }
            return true;
        }

        private static bool SendNextPacket()
        {
            try
            {
                if (CommHandler.SendQueue.Count > 0)
                {
                    CommHandler.SendQueue.Dequeue().Send();
                }
            }
            catch (Exception e)
            {
                ErrorHandler.Throw(e);
                return false;
            }
            return true;
        }

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
                catch (Exception e)
                {
                    ErrorHandler.Throw(e);
                }
                
            }
        }

    }
}
