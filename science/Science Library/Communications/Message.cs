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

        public uint Timestamp;      // Stores message timestamp (unsigned to timestamp can use all 32-bits)
        public int ID;              // Stores message id
        public byte[] Data;         // Stored message data (discluding timestamp and id)
        public IPEndPoint Endpoint; // Endpoint that the message was received from or going to.

        /// <summary>
        /// Constructs a message given
        /// data and an IPEndPoint
        /// Data encoded with:
        /// Timestamp data[0] to data[3]
        /// ID at data[4]
        /// Data encoded after data[4], i.e. data[5:]
        /// </summary>
        /// <param name="Data">
        /// Incoming data array</param>
        /// <param name="Endpoint">
        /// Given from endpoint</param>
        public Message(byte[] Data, IPEndPoint Endpoint)
        {
            // Retrieve necessary data for instantiation
            byte[] TimeBytes = UtilMain.SubArray(Data, 0, 4);
            byte IdByte = Data[4];
            int DataLength = Data.Length - 4;
            // Set Instance Variables
            this.Data = UtilMain.SubArray(Data, 5, DataLength - 1);
            this.ID = IdByte;
            this.Timestamp = (uint)UtilMain.ByteArrayToInt(TimeBytes);
            this.Endpoint = Endpoint;
        }

        /// <summary>
        /// Constructs a message given
        /// an ID, timestamp, Endpoint and other data.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Timestamp"></param>
        /// <param name="Endpoint"></param>
        /// <param name="data"></param>
        public Message(byte ID, IPEndPoint Endpoint, byte[] Data = null)
        {
            if (this.Data == null)
            { // Avoid null pointer exception
                this.Data = new byte[1];
            }
            this.Endpoint = Endpoint;
            this.ID = ID;
        }

        public void SetTime(byte[] Time)
        {
            Time = UtilMain.SubArray(Time, 0, 4); // Ensures timestamp of length 4
            this.Timestamp = (uint)BitConverter.ToInt32(Time, 0); // Calculates timestamp from given 
        }

        /// <summary>
        /// Appends data to the end of message.
        /// </summary>
        /// <param name="NewData">
        /// New Data to append to current data.</param>
        public void AppendData(byte[] NewData)
        {
            List<byte> TempList = new List<byte>(this.Data);
            TempList.AddRange(NewData);
            this.Data = TempList.ToArray();
        }

        /// <summary>
        /// Returns the raw data
        /// in structure:
        /// Timestamp data[0] to data[3]
        /// ID at data[4]
        /// Data encoded after data[4], i.e. data[5:]
        /// </summary>
        /// <returns>
        /// Returns all data in message</returns>
        public byte[] GetRawData()
        {
            List<byte> Temp = new List<byte>();
            byte[] TimeBytes = BitConverter.GetBytes(this.Timestamp);
            byte[] ID = BitConverter.GetBytes(this.ID);
            Temp.AddRange(TimeBytes);
            Temp.AddRange(ID);
            Temp.AddRange(this.Data);
            return Temp.ToArray();
        }

        /// <summary>
        /// Formats the Messages's contents to be human-readable.
        /// </summary>
        public override string ToString()
        {
            StringBuilder Str = new StringBuilder();
            Str.Append("Packet = Time:(0x");
            Str.Append(this.Timestamp.ToString("X8"));
            Str.Append(") ID:(0x");
            Str.Append(this.ID.ToString("X2"));
            Str.Append(") Data:(0x");
            Str.Append(UtilMain.BytesToNiceString(this.Data, true));
            Str.Append(')');
            return Str.ToString();
        }

    }
}
