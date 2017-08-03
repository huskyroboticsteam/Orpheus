using System;
using System.Net;
using Scarlet.Utilities;

namespace Scarlet.Communications
{
    /// <summary>
    /// Handles packet architecture.
    /// </summary>
    public class Packet : ICloneable
    {
        public Message Data { get; private set; }        // Data to send
        public string Endpoint;  // Endpoint to send or endpoint received on 
        public bool IsUDP; // Either protocol message received on or protocol for sending

        /// <summary>
        /// Meant for received packets.
        /// </summary>
        /// <param name="Data">The packet data</param>
        /// <param name="IsUDP">Defines whether or not packet is a UDP message.</param>
        /// <param name="Endpoint">The endpoint where this packet was received from</param>
        public Packet(Message Data, bool IsUDP, string Endpoint = null)
        {
            this.IsUDP = IsUDP;
            this.Data = Data;
            this.Endpoint = Endpoint;
        }

        /// <summary>
        /// Meant for sent packets.
        /// </summary>
        /// <param name="ID">The packet ID, determining what action will be taken upon receipt</param>
        /// <param name="IsUDP">Defines whether or not packet is a UDP message.</param>
        /// <param name="Endpoint">The destination where this packet will be sent</param>
        public Packet(byte ID, bool IsUDP, string Endpoint = null)
            : this(new Message(ID), IsUDP, Endpoint) { }

        /// <summary>
        /// Appends data to packet.
        /// </summary>
        /// <param name="Data">Data to append to packet.</param>
        public void AppendData(byte[] NewData) { this.Data.AppendData(NewData); }

        /// <summary>
        /// Prepares the packet for sending, then returns the raw data.
        /// </summary>
        /// <returns>The raw data, ready to be sent.</returns>
        public byte[] GetForSend(byte[] Timestamp = null)
        {
            if (Timestamp == null || Timestamp.Length != 4) { this.Data.SetTime(GetCurrentTime()); } // Sets the timestamp to the current time.
            else { this.Data.SetTime(Timestamp); } // Sets the timestamp to the one provided.
            return this.Data.GetRawData();
        }

        /// <summary>
        /// Updates the packet timestamp to the current time
        /// </summary>
        public void UpdateTimestamp()
        {
            Data.SetTime(GetCurrentTime());
        }

        /// <summary>
        /// Gets the current time as a byte array for use in packets.
        /// </summary>
        public static byte[] GetCurrentTime()
        {
            // We can't use this because it requires .NET 4.6, which isn't present on the version of Mono on the BeagleBone.
            // int UnixTime = (int)DateTimeOffset.Now.ToUnixTimeSeconds();
            int UnixTime = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            byte[] TimeArray = UtilData.ToBytes(UnixTime);
            return TimeArray;
        }

        /// <summary>
        /// Formats the Packet's contents to be human-readable.
        /// </summary>
        public override string ToString()
        {
            return (this.IsUDP ? "UDP " : "TCP ") + this.Data.ToString();
        }

        public object Clone()
        {
            Packet Clone = (Packet)this.MemberwiseClone(); // This leaves reference objects as references.
            Clone.Data = this.Data != null ? (Message)this.Data.Clone() : null;
            Clone.Endpoint = string.Copy(this.Endpoint);
            return Clone;
        }
    }
}

