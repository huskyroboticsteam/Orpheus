using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticsLibrary.Sensors
{
    public class Sensor : ISensor
    {
        private static List<Sensor> AllSensors = new List<Sensor>();

        public Sensor()
        {
            Sensor.AllSensors.Add(this);
        }

        public virtual void UpdateState() { }
        public virtual bool Test() { return false; }

        public static void UpdateAllStates()
        {
            foreach (Sensor Sensor in Sensor.AllSensors)
            {
                Sensor.UpdateState();
            }
        }
    }
}
