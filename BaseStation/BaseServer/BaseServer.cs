using System.Net;

namespace HuskyRobotics.BaseStation.Server {
    /// <summary>
    /// Contains functionality to communicate with the client/rover. 
    /// Acts as an abstraction over Scarlet networking APIs.
    /// </summary>
    public class BaseServer {
        private readonly IPEndPoint address;

        /// <summary>
        /// Construct a BaseServer with the provided bind socket (ip address and port)
        /// </summary>
        /// <param name="bindAddress"></param>
        public BaseServer(IPEndPoint bindAddress) {
            this.address = bindAddress;
        }
    }
}
