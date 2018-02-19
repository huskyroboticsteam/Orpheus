using Scarlet.Communications;
using Scarlet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// this code will go on the rover. Right now it only prints the data to a console

namespace ControllerClient
{
    class Program
    {
        static string ServerIP;
        static void Main(string[] args)
        {
            ServerIP = "127.0.0.1"; // ip to be changed later
            int PortTCP = 5287;
            int PortUDP = 5288;
            string ClientName = "Controller Parser";
            int recieveBufferSize = 128;

            Parse.SetParseHandler(0, PrintPacketData);

            Log.SetGlobalOutputLevel(Log.Severity.ERROR);
            Client.Start(ServerIP, PortTCP, PortUDP, ClientName, recieveBufferSize);
        }

        // prints the packet raw information. In the future the data will be used to control the rover 
        private static void PrintPacketData(Packet Packet)
        {
            // data order: LX, LY, RX, RY, DUp, DDown, DLeft, DRight, RTrig, LTrig, RBump,
            // LBump, A, B, X, Y, RStick, LStick, Back, Start, Big
            // Data parts that are floats: LX, LY, RX, RY, RTrig, LTrig
            // All other data piecees are ints
            if (Packet.Data.Payload.Length == 84)
            // this will make sure that any packets recieved are ones that we sent
            // because we know the length of the packets we send
            {
                byte[][] chunks = Packet.Data.Payload
                        .Select((s, i) => new { Value = s, Index = i })
                        .GroupBy(x => x.Index / 4)
                        .Select(grp => grp.Select(x => x.Value).ToArray())
                        .ToArray();
                foreach (byte[] part in chunks)
                {
                    //NOT ALL VALUES ARE FLOATS this is just a simple test to make sure all data got through
                    Console.WriteLine(UtilData.ToFloat(part));
                }
            }
        }
    }
}
