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
        private static List<List<string>>[] fileOutput = new List<List<string>>[7];
        private static StreamWriter SensorLogFile;

        private const string delimiter = ",";
        private const string LogFilesLocation = "Logs";

        public static void logSensors()
        {
            int[] sensorCount = new int[7];
            /*
             * 0 - Encoders
             * 1 - LimitSwtichs
             * 2 - MAX21855s
             * 3 - MPU6050s
             * 4 - Potentiometers
             * 5 - VEML6070s
             */
            foreach (ISensor s in Global.sensors)
            {
                if (s.GetType() == typeof(Scarlet.Components.Sensors.Encoder))
                {
                    Scarlet.Components.Sensors.Encoder temp = (Scarlet.Components.Sensors.Encoder)s;
                    fileOutput[0][sensorCount[0]].Add(temp.Angle.ToString());
                    sensorCount[0]++;
                } else
                if (s.GetType() == typeof(LimitSwitch))
                {
                    LimitSwitch temp = (LimitSwitch)s;
                    temp.SwitchToggle += (object sender, LimitSwitchToggle e) => { fileOutput[1][sensorCount[2]].Add(e.CurrentState.ToString()); };
                    sensorCount[1]++;

                } else
                if (s.GetType() == typeof(MAX31855))
                {
                    MAX31855 temp = (MAX31855)s;
                    fileOutput[2][sensorCount[2]].Add(temp.GetRawData().ToString());
                    sensorCount[2]++;
                } else
                if (s.GetType() == typeof(MPU6050))
                {
                    MPU6050 temp = (MPU6050)s;
                    MPU6050.MPUData Data = temp.GetData();
                    fileOutput[3][sensorCount[3]].Add(String.Format("{0}/{1}/{2}-{3}/{4}/{5}", Data.AccelX, Data.AccelY, Data.AccelZ, Data.GyroX, Data.GyroY, Data.GyroZ));
                    sensorCount[3]++;
                } else
                /*if (s.GetType() == typeof(MTK3339))
                {
                    MTK3339 temp = (MTK3339)s;
                    s.GetCoords().ToString()
                    fileOutput[4][sensorCount[4]].Add(String.Format("{0}/{1}", s.GetCoords.Item1, s.GetCoords.Item2));
                    sensorCount[4]++;
                }else*/
                if (s.GetType() == typeof(Potentiometer))
                {
                    Potentiometer temp = (Potentiometer)s;
                    fileOutput[5][sensorCount[5]].Add(temp.Angle.ToString());
                    sensorCount[5]++;
                } else
                if (s.GetType() == typeof(VEML6070))
                {
                    VEML6070 temp = (VEML6070)s;
                    fileOutput[6][sensorCount[6]].Add(temp.GetRawData().ToString());
                    sensorCount[6]++;
                }
                
            }
            string fileName = "SensorLogs - " + DateTime.Now.ToString("yy-MM-dd-hh-mm-ss-tt") + ".csv";
            string FileLocation = Path.Combine(LogFilesLocation, fileName);
            if (!Directory.Exists(@LogFilesLocation)) { Directory.CreateDirectory(@LogFilesLocation); }
            SensorLogFile = new StreamWriter(@FileLocation);
            int i = 0;
            foreach(List<string> l in fileOutput[0])
            {
                SensorLogFile.WriteLine("Encoder_" + i + delimiter + string.Join(delimiter, l[i]));
                i++;
            }
            i = 0;
            foreach (List<string> l in fileOutput[1])
            {
                
                SensorLogFile.WriteLine("LimitSwitch_" + i + delimiter + string.Join(delimiter, l[i]));
                i++;
            }
            i = 0;
            foreach (List<string> l in fileOutput[2])
            {
                SensorLogFile.WriteLine("MAX31855 _" + i + delimiter + string.Join(delimiter, l[i]));
                i++;
            }
            i = 0;
            foreach (List<string> l in fileOutput[3])
            {
                SensorLogFile.WriteLine("MPU6050_" + i + delimiter + string.Join(delimiter, l[i]));
                i++;
            }
            i = 0;
            foreach (List<string> l in fileOutput[4])
            {
                SensorLogFile.WriteLine("Potentiometer_" + i + delimiter + string.Join(delimiter, l[i]));
                i++;
            }
            i = 0;
            foreach (List<string> l in fileOutput[5])
            {
                SensorLogFile.WriteLine("VEML6070_" + i + delimiter + string.Join(delimiter, l[i]));
                i++;
            }
        }

        public static void StopSensorLogging()
        {
            SensorLogFile.Flush();
            SensorLogFile.Close();
        }

    }
}
