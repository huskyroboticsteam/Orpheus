using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using RoboticsLibrary.Errors;

namespace RoboticsLibrary.Communications
{
    /// <summary>
    /// Handles packet architecture.
    /// </summary>
    public class Packet
    {
        
        // Default packet endpoint
        private static IPEndPoint DefaultEndpoint = new IPEndPoint(IPAddress.Parse("192.168.0.1"), 600);

        private byte Id;                    // Packet ID
        private byte[] Timestamp;           // Packet send timestamp
        private byte[] Data;                // Packet data
        private IPEndPoint PacketEndpoint;  // Packet endpoint
        private TcpClient Client;           // Packet Tcp client

        /// <summary>
        /// Constructs new packet of given ID.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="PacketEndpoint">
        /// Endpoint for the packet. If null, defaults to given
        /// <c>DefaultEndpoint</c></param>
        public Packet(int Id, IPEndPoint PacketEndpoint = null)
        {
            this.PacketEndpoint = PacketEndpoint ?? DefaultEndpoint;
            this.Client = new TcpClient(this.PacketEndpoint);
            this.Id = (byte)Id;             // Setup ID
            this.Timestamp = new byte[4];   // Initialize timestamp
            this.Data = new byte[1];        // Temporary set to bytearray of length 1
                                            // to prevent null pointer exception.
        }

        /// <summary>
        /// Appends data to packet.
        /// </summary>
        /// <param name="Data">Data to append to packet.</param>
        public void AppendData(byte[] Data)
        {
            List<byte> TempList = new List<byte>(this.Data);
            TempList.AddRange(Data);
            this.Data = TempList.ToArray();
        }

        /// <summary>
        /// Sends packet
        /// </summary>
        /// <returns>
        /// true if Send successful,
        /// false is Send unsuccessful.
        /// </returns>
        public bool Send()
        {
            try
            {
                this.SetTime();
                // Pre-pend Timestamp and ID
                List<byte> TempDataList = new List<byte>();
                TempDataList.AddRange(this.Timestamp);
                TempDataList.Add(this.Id);
                TempDataList.AddRange(this.Data);
                byte[] SendData = TempDataList.ToArray();
                // Send Data to Endpoint through TCP
                this.Client.Connect(this.PacketEndpoint);
                NetworkStream Stream = this.Client.GetStream();
                Stream.Write(SendData, 0, SendData.Length);
                this.Client.Close(); // Close TCP Connection
            }
            catch (Exception e)
            {
                ErrorHandler.Throw(e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Sets time for packet.
        /// Internal use only.
        /// </summary>
        private void SetTime()
        {
            int UnixTime = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            byte[] TimeArray = BitConverter.GetBytes(UnixTime);
            if (TimeArray.Length != 4)
            {
                ErrorHandler.Throw(0x00);  // TODO: Error: Timestamp of wrong length.
            }
            this.Timestamp = TimeArray;
        }

        /// <summary>
        /// Sets the packet class's default Endpoint.
        /// </summary>
        /// <param name="Endpoint">
        /// New default endpoint for the packet class.
        /// </param>
        public static void SetDefaultEndpoint(IPEndPoint Endpoint)
        {
            Packet.DefaultEndpoint = Endpoint;
        }


    }

}
