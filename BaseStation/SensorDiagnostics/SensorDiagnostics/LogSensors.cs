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

namespace SensorDiagnostics
{
    /// <summary>
    /// Logs all sensors in the Global class
    /// Please be sure to add your sensors to the global class otherwise this won't find it
    /// </summary>
    static class LogSensors
    {
        private static List<DataLog> outputs = new List<DataLog>();
        public static void logSensors()
        {
            Dictionary<string, int> sensorCount = new Dictionary<string, int>();
            ISensor s;
            for (int i = 0; i < Globals.sensors.Count; i++)
            {
                s = Globals.sensors[i];
                if (!sensorCount.Keys.Contains(s.GetType().Name)) { sensorCount.Add(s.GetType().Name, 0); }
                if (i > outputs.Count) { outputs.Add(new DataLog(DateTime.Today.ToString() + "_" + s.GetType().Name + "_" + sensorCount[s.GetType().Name])); }
                outputs[i].Output(s.GetData());
                sensorCount[s.GetType().Name]++;
            }
        }
    }
}
