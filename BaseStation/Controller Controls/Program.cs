using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenTK.Input;
using Scarlet.Communications;
using Scarlet.Utilities;

namespace ControllerServer
{
    class Program
    {
        private enum PacketID : byte {GenericPackets, LXAxis, LYAxis};
        static void Main(string[] args)
        {
            int PortTCP = 5287;
            int PortUDP = 5288;

            Parse.SetParseHandler((byte)PacketID.GenericPackets, PrintPacketData);

            Log.SetGlobalOutputLevel(Log.Severity.ERROR);
            Server.Start(PortTCP, PortUDP);
            while (true)
            {
                if (Server.GetClients().Contains("Controller Parser"))
                {
                    GamePadState state = GamePad.GetState(0);
                    // Left X-axis
                    sendPacket(new Message((byte)PacketID.LXAxis, UtilData.ToBytes(state.ThumbSticks.Left.X)),
                        true, "Controller Parser");
                    // Left Y-axis
                    sendPacket(new Message((byte)PacketID.LYAxis, UtilData.ToBytes(state.ThumbSticks.Left.Y)),
                        true, "Controller Parser");
                }
                Thread.Sleep(50);
            }
        }

        // sends a packet with the given message udp setting and endpoint
        private static void sendPacket(Message message, bool isUDP, String endpoint)
        {
            Packet MyPack = new Packet(message, isUDP, endpoint);
            Server.Send(MyPack);
        }

        // package handler prints package contents to console
        private static void PrintPacketData(Packet packet)
        {
            Console.WriteLine(UtilData.ToString(packet.Data.Payload));
        }

    }
}
