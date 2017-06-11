using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using RoboticsLibrary.Utilities;

namespace RoboticsLibrary.Communications
{
    /// <summary>
    /// This class is intended
    /// to encode incoming data.
    /// </summary>
    public class Message
    {

        public int Timesamp, Id; // Stores message timestamp and id
        public byte[] Data;      // Stored message data (discluding timestamp and id)
        public byte[] RawData;
        public IPEndPoint From;  // Endpoint that the message was received from.

        /// <summary>
        /// Constructs a message given
        /// incoming data.
        /// Data encoded with:
        /// Timestamp data[0] to data[3]
        /// ID at data[4]
        /// Data encoded after data[4], i.e. data[5:]
        /// </summary>
        /// <param name="IncomingData">
        /// Incoming data array</param>
        /// <param name="From">
        /// Given from endpoint</param>
        public Message(byte[] IncomingData, IPEndPoint From)
        {
            this.RawData = IncomingData;
            // Retrieve necessary data for instantiation
            byte[] TimeBytes = UtilMain.SubArray(IncomingData, 0, 4);
            byte IdByte = IncomingData[4];
            int DataLength = IncomingData.Length - 4;
            // Set Instance Variables
            this.Data = UtilMain.SubArray(IncomingData, 5, DataLength - 1);
            this.Id = (int)IdByte;
            this.Timesamp = UtilMain.ByteArrayToInt(TimeBytes);
            this.From = From;
        }

    }
}
