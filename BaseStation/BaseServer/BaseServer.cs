using Scarlet.Communications;
using Scarlet.Utilities;
using System.Net;

namespace HuskyRobotics.BaseStation.Server {
    /// <summary>
    /// Contains functionality to communicate with the client/rover.
    /// Acts as an abstraction over Scarlet communications API.
    /// </summary>
    public static class BaseServer {

        private static bool shutdown = false;

        public static void Start()
        {
            Scarlet.Communications.Server.Start(1025, 1026);
        }

        public static void EventLoop()
        {
            while (!shutdown) {
                Packet MyPack = new Packet(0x80, true);
                MyPack.AppendData(UtilData.ToBytes("Homura"));
                Scarlet.Communications.Server.Send(MyPack);
            }
        }

        public static void Shutdown()
        {
            shutdown = true;
            Scarlet.Communications.Server.Stop();
        }
    }
}
