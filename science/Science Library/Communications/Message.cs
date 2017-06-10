using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using RoboticsLibrary.Utilities;

namespace RoboticsLibrary.Communications
{
    public class Message
    {
        public int Timesamp, Id;
        public byte[] Data;
        public IPEndPoint From;

        public Message(byte[] IncomingData, IPEndPoint From)
        {
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
