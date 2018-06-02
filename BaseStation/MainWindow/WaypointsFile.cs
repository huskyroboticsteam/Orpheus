using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuskyRobotics.UI
{
    public class WaypointsFile
    {
        public String FileName { get;  private set; }
        public ObservableCollection<Waypoint> Waypoints { get; private set; } = new ObservableCollection<Waypoint>();

        public WaypointsFile(String waypointsFile)
        {
            FileName = waypointsFile;
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Images\" + FileName))
            {
                using (StreamReader file = new StreamReader(Directory.GetCurrentDirectory() + @"\Images\" + FileName))
                {
                    String line;
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        double Lat = 47.653799;
                        double Long = -122.307808;
                        if (!Double.TryParse(parts[1], out Lat))
                            throw new ArithmeticException("Could not parse waypoint Lat");
                        if (!Double.TryParse(parts[2], out Long))
                            throw new ArithmeticException("Could not parse waypoint Long");
                        Waypoints.Add(new Waypoint(Lat, Long, parts[0]));
                    }
                }
            }
            Waypoints.CollectionChanged += Save;
        }

        private void Save(object sender, NotifyCollectionChangedEventArgs e)
        {
            String fileLocation = Directory.GetCurrentDirectory().ToString() + @"\Images\" + FileName;
            using (StreamWriter file = new StreamWriter(fileLocation))
            {
                foreach (Waypoint waypoint in Waypoints)
                {
                    file.WriteLine(waypoint.Name + "|" + waypoint.Lat + "|" + waypoint.Long);
                }
            }
        }
    }
}
