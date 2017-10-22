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

        public static string Name;

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
            
        }

        #region Receive

        #endregion

        #region Send

        /// <summary>
        /// Sends a packet. Handles both UDP and TCP.
        /// </summary>
        /// <param name="SendPacket">Packet to send</param>
        /// <returns>Success of packet sending</returns>
        public static bool Send(Packet SendPacket)
        {

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
            return false;
        }
        #endregion
        
    }
}
