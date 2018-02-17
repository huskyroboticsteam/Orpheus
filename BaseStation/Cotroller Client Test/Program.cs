using Scarlet.Communications;
using Scarlet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerClient
{
    class Program
    {
        static string ServerIP;
        private enum PacketID : byte {GenericPackets, LXAxis, LYAxis};
        static void Main(string[] args)
        {
            ServerIP = "127.0.0.1";
            int PortTCP = 5287;
            int PortUDP = 5288;
            string ClientName = "Controller Parser";

            Parse.SetParseHandler((byte)PacketID.LXAxis, LXAxisHandle);
            Parse.SetParseHandler((byte)PacketID.LYAxis, LYAxisHandle);

            Log.SetGlobalOutputLevel(Log.Severity.ERROR);
            Client.Start(ServerIP, PortTCP, PortUDP, ClientName);
        }

        // sends a message back to server indicating the clients interpretation of the packet
        // for left y axis packets
        private static void LYAxisHandle(Packet Packet)
        {
            float data = UtilData.ToFloat(Packet.Data.Payload);
            Console.WriteLine(data);
            data = (float)Math.Round(data, 2);
            if (data < -0.02 || data > 0.02)
            {
                String say = "Broken";
                if (data < -0.02)
                {
                    say = "Move Backward";
                }
                else if (data > 0.02)
                {
                    say = "Move Foreward";
                }
                Message returns = new Message((byte)PacketID.GenericPackets, say);
                sendPacket(returns, true, "Server");
            }
        }

        // sends a message back to server indicating the clients interpretation of the packet
        // for left x axis packets
        private static void LXAxisHandle(Packet Packet)
        {
            float data = UtilData.ToFloat(Packet.Data.Payload);
            Console.WriteLine(data);
            data = (float)Math.Round(data, 2);
            if (data < -0.02 || data > 0.02)
            {
                String say = "Broken";
                if (data < -0.02)
                {
                    say = "Straif left";
                }
                else if (data > 0.02)
                {
                    say = "Straif right";
                }
                Message returns = new Message((byte)PacketID.GenericPackets, say);
                sendPacket(returns, true, "Server");
            }
        }

        // sends a packet to the server with the given message, udp setting and endpoint
        private static void sendPacket(Message message, bool isUDP, String endpoint)
        {
            Packet returnMessage = new Packet(message, isUDP, endpoint);
            Client.Send(returnMessage);
        }
    }
}
