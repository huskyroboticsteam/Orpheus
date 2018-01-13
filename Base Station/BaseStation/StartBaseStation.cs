using System;
using System.Net;
using HuskyRobotics.BaseStation.Server;

namespace BaseStation {
    public static class StartBaseStation {
        /// <summary>
        /// The entry point of the base station system.
        /// </summary>
        /// <param name="args">command-line arguments</param>
        public static void Main(String[] args) {
            BaseServer server = new BaseServer(new IPEndPoint(IPAddress.Any, 0));
        }
    }
}
