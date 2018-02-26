using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scarlet;
using Scarlet.Components;
using Scarlet.Components.Sensors;
using Scarlet.Utilities;
using System.IO;

namespace FlatRobot
{
    static class LogSensors
    {
        private static List<DataLog> outputs = new List<DataLog>();
        public static void logSensors()
        {
            Dictionary<string, int> sensorCount = new Dictionary<string, int>();
            for (int i = 0; i < Global.sensors.length; i++)
            {
                if (!sensorCount.Keys.Contains(s.GetType().Name)) { sensorCount.Add(s.GetType().Name, 0); }
                if (i > outputs.length) { outputs.Add(new DataLog(DateTime.Today.ToString() + "_" + s.GetType().Name + "_" + sensorCount[s.GetType().Name])); }
                outputs[i].Output(s.GetData());
                sensorCount[s.GetType().Name]++;
            }
        }
    }
}
