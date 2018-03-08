using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using HuskyRobotics.Utilities;
using System.Security.Cryptography;
using System.ComponentModel;

// make sure there is a folder named MapTiles in the working directory
// https://msdn.microsoft.com/en-us/library/bb259689.aspx#Map map system (its bing but google
// uses the same system)
// https://google-developers.appspot.com/maps/documentation/static-maps/intro#Zoomlevels google
// tile download documentation

namespace HuskyRobotics.UI
{
    public static class MapTileDownloadManager
    {
        // a class to hold the configuration for the map dowload, formated as specified by google
        public class Configuration
        {
            private const double MinLatitude = -85.05112878;
            private const double MaxLatitude = 85.05112878;
            private const double MinLongitude = -180;
            private const double MaxLongitude = 180;

            private int _scale;
            private int _zoom;
            private string _mapType;
            private double _latitude;
            private double _longitude;
            private int _imgWidth;
            private int _imgheight;

            public int Scale
            {
                get { return _scale; }
                set {
                    if (value == 1 || value == 2) _scale = value;
                }
            }
            public int Zoom
            {
                get { return _zoom; }
                set {
                    if (value >= 0 && value <= 21) _zoom = value;
                }
            }
            public string MapType
            {
                get { return _mapType; }
                set
                {
                    value = value.ToLower();
                    if (value.Equals("roadmap") || value.Equals("satellite") ||
                      value.Equals("terrain") || value.Equals("hybrid")) _mapType = value;
                }
            }
            public double Latitude { get { return _latitude; }
                set {
                    if (value >= MinLatitude && value <= MaxLatitude) _latitude = value;
                }
            }
            public double Longitude { get { return _longitude; }
                set {
                    if (value >= MinLongitude && value <= MaxLongitude) _longitude = value;
                }
            }
            public int ImgWidth { get { return _imgWidth; }
                set {
                    if (value > 0) _imgWidth = value;
                }
            }
            public int ImgHeight { get { return _imgheight; }
                set {
                    if (value > 0) _imgheight = value;
                }
            }

            public Configuration(Tuple<double, double> coords, Tuple<int, int> imgDim, int zoom = 1, int scale = 2, string maptype = "satellite")
            {
                Latitude = coords.Item1;
                Longitude = coords.Item2;
                ImgWidth = imgDim.Item1;
                ImgHeight = imgDim.Item2;
                Scale = scale;
                Zoom = zoom;
                MapType = maptype;
            }

            // returns the string representation of the configuration
            public override String ToString()
            {
                return "center=" + Latitude + "," + Longitude + "&size=" + ImgWidth + "x"
                    + ImgHeight + "&scale=" + Scale + "&zoom=" + Zoom + "&maptype=" + MapType;
            }
        }

        // gets the tile set of maps with the given coords of the center, width and height of tiling
        // and configuration for the center tile
        public static void DownloadNewTileSet(Tuple<int, int> tilingDim, Configuration config, String mapSetName)
        {
            String fileName = Directory.GetCurrentDirectory().ToString() + @"\Images\" + mapSetName + ".map";
            using (StreamWriter file = new StreamWriter(fileName))
            {
                Tuple<int, int> centerPoint = MapConversion.LatLongToPixelXY(config.Latitude,
                    config.Longitude, config.Zoom);

                file.WriteLine(config.ImgWidth + "x" + config.ImgHeight + "|" + config.Zoom + "|"
                    + config.Scale + "|" + config.MapType);

                // center of the tiling is 0,0
                int startx = -tilingDim.Item1 / 2;
                int starty = -tilingDim.Item2 / 2;
                if (tilingDim.Item1 % 2 == 0) startx++;
                if (tilingDim.Item2 % 2 == 0) starty++;

                for (int i = startx; i <= tilingDim.Item1 / 2; i++)
                {
                    for (int j = starty; j <= tilingDim.Item2 / 2; j++)
                    {
                        Tuple<double, double> newCoords = MapConversion.PixelXYToLatLong
                            (centerPoint.Item1 + (i * config.ImgWidth), centerPoint.Item2
                            + (j * config.ImgHeight), config.Zoom);
                        config.Latitude = newCoords.Item1;
                        config.Longitude = newCoords.Item2;
                        string imageName = Fetch(config);
                        file.WriteLine(i + "," + j + "|" + imageName);
                    }
                }
            }
        }

        // pulls a single map tile from google maps returns the hash code used for the file name.
        public static String Fetch(Configuration config)
        {
            String requestUrl = "https://maps.googleapis.com/maps/api/staticmap?" + config.ToString();

            HttpWebRequest request = WebRequest.CreateHttp(requestUrl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream input = response.GetResponseStream();
            // writes to the buffer stream
            using (MemoryStream buffer = new MemoryStream())
            {
                input.CopyTo(buffer);
                //hash code generator
                SHA256Managed sha = new SHA256Managed();
                buffer.Position = 0;
                byte[] hash = sha.ComputeHash(buffer);
                var bufferHash = BitConverter.ToString(hash).Replace("-", String.Empty).Substring(0, 16);
                Console.WriteLine(bufferHash);
                String fileName = Directory.GetCurrentDirectory().ToString()+ @"\Images\" + bufferHash + ".jpg";
                buffer.Position = 0;
                using (var file = File.Create(fileName))
                {
                    buffer.CopyTo(file);
                    return bufferHash;
                }
            }
        }
    }
}
