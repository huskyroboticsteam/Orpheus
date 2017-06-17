using System;
using System.Net;
using System.Net.Sockets;
using Scarlet.Utilities;

namespace Scarlet.Communications
{
    /// <summary>
    /// Handles packet architecture.
    /// </summary>
    public class Packet
    {

        // Default packet endpoint
        public static IPEndPoint DefaultEndpoint = new IPEndPoint(IPAddress.Parse("192.168.0.1"), 600);

        private Message PacketMessage; // Stored Packet message
        private TcpClient Client;      // Packet Tcp client

        /// <summary>
        /// Constructs new packet of given ID.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="PacketEndpoint">
        /// Endpoint for the packet. If null, defaults to given
        /// <c>DefaultEndpoint</c></param>
        public Packet(int ID, IPEndPoint PacketEndpoint = null)
        {
            IPEndPoint EndPoint = PacketEndpoint ?? DefaultEndpoint;
            try
            {
                this.Client = new TcpClient(new IPEndPoint(IPAddress.Parse("0.0.0.0"), 8000));
            }
            catch (SocketException Exception)
            {
                Log.Output(Log.Severity.ERROR, Log.Source.NETWORK, "Failed to connect to remote IP " + EndPoint);
                Log.Exception(Log.Source.NETWORK, Exception);
            }
            this.PacketMessage = new Message((byte)ID, EndPoint);
        }

        /// <summary>
        /// Appends data to packet.
        /// </summary>
        /// <param name="Data">Data to append to packet.</param>
        public void AppendData(byte[] Data)
        {
            this.PacketMessage.AppendData(Data);
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
            return SendWithTimestamp(GetTimestamp());
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
                this.PacketMessage.SetTime(Timestamp);
                // Pre-pend Timestamp and ID
                byte[] SendData = this.PacketMessage.GetRawData();
                // Send Data to Endpoint through TCP
                this.Client.Connect(this.PacketMessage.Endpoint);
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
        public static byte[] GetTimestamp()
        {
            int UnixTime = (int)DateTimeOffset.Now.ToUnixTimeSeconds();
            byte[] TimeArray = BitConverter.GetBytes(UnixTime);
            if (TimeArray.Length != 4)
            {
                Log.Output(Log.Severity.WARNING, Log.Source.NETWORK, "Timestamp was of incorrect length.");
                TimeArray = new byte[] { 0x00, 0x00, 0x00, 0x00 };
            }
            return TimeArray;
        }

        /// <summary>
        /// Formats the Packet's contents to be human-readable.
        /// </summary>
        public override string ToString()
        {
            return this.PacketMessage.ToString();
        }

    }

}
