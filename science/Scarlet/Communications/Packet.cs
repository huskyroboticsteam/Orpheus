using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Scarlet.Utilities;

namespace Scarlet.Communications
{
    /// <summary>
    /// Handles packet architecture.
    /// </summary>
    public class Packet : ICloneable
    {
        public Message Data { get; private set; }
        public IPEndPoint Endpoint { get; private set; }

        /// <summary>
        /// Meant for received packets.
        /// </summary>
        /// <param name="Message">The packet data</param>
        /// <param name="Endpoint">The endpoint where this packet was received from</param>
        public Packet(Message Message, IPEndPoint Endpoint)
        {
            this.Data = Message;
            this.Endpoint = Endpoint;
        }

        /// <summary>
        /// Meant for sent packets.
        /// </summary>
        /// <param name="ID">The packet ID, determining what action will be taken upon receipt</param>
        /// <param name="Target">The destination where this packet will be sent</param>
        public Packet(byte ID, IPEndPoint Target = null)
        {
            this.Endpoint = Target ?? CommHandler.DefaultTarget;
            this.Data = new Message(ID);
        }

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
        /// Gets the current time as a byte array for use in packets.
        /// </summary>
        public static byte[] GetCurrentTime()
        {
            int UnixTime = (int)DateTimeOffset.Now.ToUnixTimeSeconds();
            byte[] TimeArray = UtilData.ToBytes(UnixTime);
            return TimeArray;
        }

        /// <summary>
        /// Formats the Packet's contents to be human-readable.
        /// </summary>
        public override string ToString() { return this.Data.ToString(); }

        public object Clone()
        {
            Packet Clone = (Packet)this.MemberwiseClone(); // This leaves reference objects as references.
            Clone.Data = this.Data != null ? (Message)this.Data.Clone() : null;
            Clone.Endpoint = this.Endpoint != null ? new IPEndPoint(IPAddress.Parse(string.Copy(this.Endpoint.Address.ToString())), this.Endpoint.Port) : null;
            return Clone;
        }
    }
}
