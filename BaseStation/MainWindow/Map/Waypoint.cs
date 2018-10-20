using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuskyRobotics.UI
{
    public class Waypoint
    {
        public double Lat { get; private set; }
        public double Long { get; private set; }
        public string Name { get; private set; } = "";

        public Waypoint() : this(0,0)
        {
        }

        public Waypoint(double lat, double long_)
        {
            Lat = lat;
            Long = long_;
        }

        public Waypoint(double lat, double long_, string name)
        {
            Lat = lat;
            Long = long_;
            Name = name;
        }
    }
}
