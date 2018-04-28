using Scarlet.Communications;
using Scarlet.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using HuskyRobotics.UI;

namespace HuskyRobotics
{
    class Test_Server
    {
        public static sensorRealTimeGraph uc = new sensorRealTimeGraph();
        [STAThread]
        static void Main(string[] args)
        {
            new Thread(StartServer).Start();
            Application app = new Application();
            app.Exit += (sd, ev) => Scarlet.Communications.Server.Stop();
            Window host = new Window();
            host.Content = uc;
            app.Run(host);

        }
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
            uc.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() =>
            {
                uc.updateGraph(info);
            }));
        }
        private static void StartServer()
        {
            Server.Start(1025, 1026, 256);
            Parse.SetParseHandler(0x65, ParseSensorPacket);
        }
        private static void update()
        {

        }
    }
}
