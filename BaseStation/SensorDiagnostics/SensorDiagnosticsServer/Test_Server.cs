using Scarlet.Communications;
using Scarlet.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace HuskyRobotics.UI
{
    /// <summary>
    /// This Class is just used as a test server in order to recieve data from the BBB and send it to the real time graph.
    /// </summary>
    class Test_Server
    {
        public static SensorRealTimeGraph sensorRealTimeGraph = new SensorRealTimeGraph(); //The Graph
        [STAThread]
        static void Main(string[] args)
        {
            new Thread(StartServer).Start();
            Application app = new Application();
            app.Exit += (sd, ev) => Scarlet.Communications.Server.Stop();
            Window host = new Window();
            host.Content = sensorRealTimeGraph;
            app.Run(host);
        }

        /// <summary>
        /// This method parses the packet of sensor data into a Dataunit that can be used by the graph.
        /// </summary>
        /// <param name="packet">This packet should be formatted: {system}|{valueName}={value},{valueName}={value};repeat</param>
        public static void ParseSensorPacket(Packet packet)
        {
            string data = UtilData.ToString(packet.Data.Payload);
            List<DataUnit> info = new List<DataUnit>();
            data = data.Substring(0,data.Length-1);
            string[] sensSplit = data.Split(';');
            foreach (string s in sensSplit)
            {
                string sens = s.Substring(0, s.Length - 1);
                string[] sysSplit = sens.Split('|');
                string[] values = sysSplit[1].Split(',');
                string[] temp2;
                DataUnit tempUnit = new DataUnit("GraphPacket");
                foreach (string v in values)
                {
                    temp2 = v.Split('=');
                    tempUnit.Add<double>(temp2[0], Convert.ToDouble(temp2[1]));
                }
                tempUnit.System = sysSplit[0];
                info.Add(tempUnit);
            }
            sensorRealTimeGraph.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() =>
            {
                sensorRealTimeGraph.updateGraph(info);
            }));
        }
        /// <summary>
        /// This method starts the scarlet server
        /// </summary>
        private static void StartServer()
        {
            Server.Start(1025, 1026, 256);
            Parse.SetParseHandler(0x65, ParseSensorPacket);
        }
    }
}
