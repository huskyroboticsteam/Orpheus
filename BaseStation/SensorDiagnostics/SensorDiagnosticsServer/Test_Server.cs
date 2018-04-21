using Scarlet;
using Scarlet.Components;
using Scarlet.Components.Sensors;
using Scarlet.IO;
using Scarlet.IO.BeagleBone;
using Scarlet.Communications;
using Scarlet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Scarlet.Utilities;
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
            string[] sysSplit = data.Split('|');
            string[] temp = sysSplit[1].Split(',');
            string[] temp2;
            DataUnit tempUnit = new DataUnit("temp");
            foreach (string s in temp)
            {
                temp2 = s.Split('=');
                tempUnit.Add<double>(temp2[0],Convert.ToDouble(temp2[1]));
            }
            tempUnit.System = sysSplit[0];
            info.Add(tempUnit);
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
