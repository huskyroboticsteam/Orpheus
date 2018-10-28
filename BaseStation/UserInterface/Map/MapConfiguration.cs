using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuskyRobotics.UI
{
    public class MapConfiguration
    {
        private const double MinLatitude = -85.05112878;
        private const double MaxLatitude = 85.05112878;
        private const double MinLongitude = -180;
        private const double MaxLongitude = 180;
        public const int LOGO_BLEED = 60;

        private int _scale;
        private int _zoom;
        private string _mapType;
        private double _latitude;
        private double _longitude;
        private int _imgWidth;
        private int _imgheight;
        private int _tilingWidth;
        private int _tilingHeight;

        // Scale must be either 1 or 2
        public int Scale
        {
            get { return _scale; }
            set
            {
                if (value == 1 || value == 2) _scale = value;
                else throw new ArgumentOutOfRangeException("value", "Scale must be 1 or 2");
            }
        }
        // Zoom must be between 0 and 21 inclusive
        public int Zoom
        {
            get { return _zoom; }
            set
            {
                if (value >= 0 && value <= 21) _zoom = value;
                else throw new ArgumentOutOfRangeException("value", "Zoom must be between 0 and 21 inclusive");
            }
        }
        // Map Type must be one of: roadmap, satellite, terrain, hybrid
        public string MapType
        {
            get { return _mapType; }
            set
            {
                value = value.ToLower();
                if (value.Equals("roadmap") || value.Equals("satellite") ||
                  value.Equals("terrain") || value.Equals("hybrid")) _mapType = value;
                else throw new ArgumentOutOfRangeException("value", "Map Type must be one of: roadmap, satellite, terrain, hybrid");
            }
        }
        // Lattitude must be between -85.05112878 and 85.05112878 inclusive
        public double Latitude
        {
            get { return _latitude; }
            set
            {
                if (value >= MinLatitude && value <= MaxLatitude) _latitude = value;
                else throw new ArgumentOutOfRangeException("value", "Lattitude must be between "
                    + MinLatitude + " and " + MaxLatitude + " inclusive");
            }
        }
        // Longitude must be between -180 and 180 inclusive
        public double Longitude
        {
            get { return _longitude; }
            set
            {
                if (value >= MinLongitude && value <= MaxLongitude) _longitude = value;
                else throw new ArgumentOutOfRangeException("value", "Longitude must be between "
                    + MinLongitude + " and " + MaxLongitude + " inclusive");
            }
        }
        // Image width must be greater than 0
        public int ImgWidth
        {
            get { return _imgWidth; }
            set
            {
                if (value > 0) _imgWidth = value;
                else throw new ArgumentOutOfRangeException("value", "Image width must be greater than 0");
            }
        }
        // Image height must be greater than 0
        public int ImgHeight
        {
            get { return _imgheight; }
            set
            {
                if (value > 0) _imgheight = value;
                else throw new ArgumentOutOfRangeException("value", "Image height must be greater than 0");
            }
        }
        // Tiling width must be greater than 0
        public int TilingWidth
        {
            get { return _tilingWidth; }
            set
            {
                if (value > 0) _tilingWidth = value;
                else throw new ArgumentOutOfRangeException("value", "Tiling width must be greater than 0");
            }
        }
        // Tiling height must be greater than 0
        public int TilingHeight
        {
            get { return _tilingHeight; }
            set
            {
                if (value > 0) _tilingHeight = value;
                else throw new ArgumentOutOfRangeException("value", "Tiling height must be greaterthan 0");
            }
        }

        public string MapSetName { get; set; }

        // Constructor with everything set to a default value
        public MapConfiguration()
        {
            Latitude = 47.653799;
            Longitude = -122.307808;
            ImgWidth = 300;
            ImgHeight = 300;
            Scale = 2;
            Zoom = 17;
            MapType = "satellite";
            MapSetName = "New Map";
            TilingWidth = 5;
            TilingHeight = 5;
        }

        // returns the string representation of the configuration
        public string URLParams()
        {
            return "center=" + Latitude + "," + Longitude + "&size=" + ImgWidth + "x"
                + (ImgHeight + LOGO_BLEED) + "&scale=" + Scale + "&zoom=" + Zoom + "&maptype=" + MapType;
        }
    }
}
