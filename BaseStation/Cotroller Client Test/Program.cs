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
            ServerIP = "127.0.0.1"; // this ip is used as a test
            int PortTCP = 5287;
            int PortUDP = 5288;
            string ClientName = "Controller Parser";
            int recieveBufferSize = 128;

            Parse.SetParseHandler(0, ParsePacket);

            Log.SetGlobalOutputLevel(Log.Severity.ERROR);
            Client.Start(ServerIP, PortTCP, PortUDP, ClientName, recieveBufferSize);
        }

        // Separates each piece of data within the packet
        private static void ParsePacket(Packet Packet)
        {
            // data order: LX, LY, RX, RY, RTrig, LTrig, DUp, DDown, DLeft, DRight, RBump,
            // LBump, A, B, X, Y, RStick, LStick, Back, Start, Big
            // Data parts that are floats: LX, LY, RX, RY, RTrig, LTrig
            // All other data piecees are ints
            if (Packet.Data.Payload.Length == 84)
            // this will make sure that any packets recieved are ones that we sent
            // because we know the length of the packets we send
            {
                // this bit is an example of how the data can be split into its parts knowing that
                // floats and int both have byte arrays of length 4
                byte[][] chunks = Packet.Data.Payload
                        .Select((s, i) => new { Value = s, Index = i })
                        .GroupBy(x => x.Index / 4)
                        .Select(grp => grp.Select(x => x.Value).ToArray())
                        .ToArray();
            }
        }

        // prints the data to the console
        private static void PrintData(byte[][] data)
        {
            foreach (byte[] part in data)
            {
                //NOT ALL VALUES ARE FLOATS this is just a simple test to make sure all data got through
                Console.WriteLine(UtilData.ToFloat(part));
            }
        }
    }
}
