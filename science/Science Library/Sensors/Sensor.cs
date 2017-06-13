using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticsLibrary.Sensors
{
    public class Sensor : ISensor
    {
        private static List<Sensor> AllSensors = new List<Sensor>(); // Stores all sensors

        public Sensor()
        {
            Sensor.AllSensors.Add(this);
        }

        /// <summary>
        /// To be overridden.
        /// Handles what to on
        /// each iteration to 
        /// determine state change.
        /// </summary>
        public virtual void UpdateState() { }

        /// <summary>
        /// To be overridden to
        /// determine whether or not the
        /// sensor is working.
        /// </summary>
        /// <returns>
        /// Whether or not the sensor is
        /// working.</returns>
        public virtual bool Test() { return false; }


        /// <summary>
        /// Updates the states for all sensors
        /// </summary>
        public static void UpdateAllStates()
        {
            foreach (Sensor Sensor in Sensor.AllSensors)
            {
                Sensor.UpdateState();
            }
        }
        
        /// <summary>
        /// Tests all the sensors,
        /// returns whether or not all
        /// tests passed
        /// </summary>
        /// <returns>
        /// Whether or not all
        /// tests passed
        /// </returns>
        public static bool TestAll()
        {
            bool AllPassed = true;
            foreach(Sensor Sensor in Sensor.AllSensors)
            {
                if (!Sensor.Test())
                {
                    AllPassed = false;
                }
            }
            return AllPassed;
        }
    }
}
