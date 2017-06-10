using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using RoboticsLibrary.Errors;

namespace RoboticsLibrary.Communications
{
    public class Packet
    {

        private static IPEndPoint DefaultEndpoint = new IPEndPoint(IPAddress.Parse("192.168.0.1"), 600);

        private byte Id;
        private byte[] Timestamp;
        private byte[] Data;
        private IPEndPoint PacketEndpoint;
        private TcpClient Client;

        public Packet(int Id, IPEndPoint PacketEndpoint = null)
        {
            this.PacketEndpoint = PacketEndpoint ?? DefaultEndpoint;
            this.Client = new TcpClient(this.PacketEndpoint);
            this.Id = (byte)Id;
            this.Timestamp = new byte[4];
            this.Data = new byte[1]; // Temporary
        }

        public void AppendData(byte[] Data)
        {
            List<byte> TempList = new List<byte>(this.Data);
            TempList.AddRange(Data);
            this.Data = TempList.ToArray();
        }

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

        public static void SetDefaultEndpoint(IPEndPoint Endpoint)
        {
            Packet.DefaultEndpoint = Endpoint;
        }


    }

}
