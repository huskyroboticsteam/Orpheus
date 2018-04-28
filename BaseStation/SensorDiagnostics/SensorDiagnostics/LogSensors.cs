using HuskyRobotics.UI;
using Scarlet;
using Scarlet.Components;
using Scarlet.Components.Sensors;
using Scarlet.Communications;
using Scarlet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HuskyRobotics
{
    /// <summary>
    /// Logs all sensors in the Global class
    /// Please be sure to add your sensors to the global class otherwise this won't find it
    /// </summary>
    static class LogSensors
    {
        private static List<DataLog> outputs = new List<DataLog>();
        private static Dictionary<string, DataUnit> LastReadings = new Dictionary<string,DataUnit>();
        public static Dictionary<string,DataUnit> logSensors()
        {
            Packet sensorInfoPacket = new Packet(0x65, false);
            Dictionary<string, int> sensorCount = new Dictionary<string, int>();
            ISensor s;
            for (int i = 0; i < Globals.sensors.Count; i++)
            {
                s = Globals.sensors[i];
                DataUnit data = s.GetData();
                if (!sensorCount.Keys.Contains(s.GetType().Name)) {
                    sensorCount.Add(s.GetType().Name, 0);
                }
                if (i >= outputs.Count) {
                    outputs.Add(new DataLog(data.System +"_"+ s.GetType().Name + "_" + sensorCount[s.GetType().Name]));
                    LastReadings.Add(s.GetType().Name+sensorCount[s.GetType().Name],null);
                }
                LastReadings[s.GetType().Name+sensorCount[s.GetType().Name]]=data;
                outputs[i].Output(data);
                sensorCount[s.GetType().Name]++;
                sensorInfoPacket.AppendData(UtilData.ToBytes(data.System));
                sensorInfoPacket.AppendData(UtilData.ToBytes("|"));
                foreach (string key in data.Keys)
                {
                    sensorInfoPacket.AppendData(UtilData.ToBytes(key));
                    sensorInfoPacket.AppendData(UtilData.ToBytes("="));
                    sensorInfoPacket.AppendData(UtilData.ToBytes(Convert.ToString(data.GetValue<ValueType>(key))));
                    sensorInfoPacket.AppendData(UtilData.ToBytes(","));
                }
                sensorInfoPacket.AppendData(UtilData.ToBytes(";"));
            }
            Client.Send(sensorInfoPacket);
            return LastReadings;
        }
    }
}
