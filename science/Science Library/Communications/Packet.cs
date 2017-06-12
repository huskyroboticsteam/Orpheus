using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using RoboticsLibrary.Errors;
using RoboticsLibrary.Utilities;

namespace RoboticsLibrary.Communications
{
    /// <summary>
    /// Handles packet architecture.
    /// </summary>
    public class Packet
    {
        
        // Default packet endpoint
        private static IPEndPoint DefaultEndpoint = new IPEndPoint(IPAddress.Parse("192.168.0.1"), 600);

        private byte ID;                    // Packet ID
        private byte[] Timestamp;           // Packet send timestamp
        private byte[] Data;                // Packet data
        private IPEndPoint PacketEndpoint;  // Packet endpoint
        private TcpClient Client;           // Packet Tcp client

        /// <summary>
        /// Constructs new packet of given ID.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="PacketEndpoint">
        /// Endpoint for the packet. If null, defaults to given
        /// <c>DefaultEndpoint</c></param>
        public Packet(int ID, IPEndPoint PacketEndpoint = null)
        {
            this.PacketEndpoint = PacketEndpoint ?? DefaultEndpoint;
            try
            {
                this.Client = new TcpClient(this.PacketEndpoint);
            }
            catch(SocketException Exception)
            {
                Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Failed to connect to remote IP " + this.PacketEndpoint);
                Log.Exception(Log.Source.NETWORK, Exception);
            }
            this.ID = (byte)ID;             // Setup ID
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
            this.SetTime();
            return SendWithTimestamp(this.Timestamp);
        }

        /// <summary>
        /// Sends packet, using a custom timestamp.
        /// </summary>
        /// <param name="Timestamp">The timestamp to use</param>
        /// <returns>Whether packet sending was successful.</returns>
        public bool SendWithTimestamp(byte[] Timestamp)
        {
            try
            {
                // Pre-pend Timestamp and ID
                List<byte> TempDataList = new List<byte>();
                TempDataList.AddRange(Timestamp);
                TempDataList.Add(this.ID);
                TempDataList.AddRange(this.Data);
                byte[] SendData = TempDataList.ToArray();
                // Send Data to Endpoint through TCP
                this.Client.Connect(this.PacketEndpoint);
                NetworkStream Stream = this.Client.GetStream();
                Stream.Write(SendData, 0, SendData.Length);
                this.Client.Close(); // Close TCP Connection
            }
            catch (Exception Except)
            {
                Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Failed to create packet.");
                Log.Exception(Log.Source.NETWORK, Except);
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
                Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Timestamp was of incorrect length.");
                TimeArray = new byte[] { 0x00, 0x00, 0x00, 0x00 };
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

        /// <summary>
        /// Formats the Packet's contents to be human-readable.
        /// </summary>
        public override string ToString()
        {
            StringBuilder Str = new StringBuilder();
            Str.Append("Packet = Time:(0x");
            Str.Append(BitConverter.ToInt32(this.Timestamp, 0).ToString("X8"));
            Str.Append(") ID:(0x");
            Str.Append(this.ID.ToString("X2"));
            Str.Append(") Data:(0x");
            foreach(byte DataElement in this.Data)
            {
                Str.Append(DataElement.ToString("X2"));
                Str.Append(' ');
            }
            Str.Remove(Str.Length - 1, 1);
            Str.Append(')');
            return Str.ToString();
        }

    }

}
