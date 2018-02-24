using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using HuskyRobotics.Utilities;

// make sure there is a folder named MapTiles in the working directory
// google API key is stored in environment variables
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
            private Dictionary<String, String> parameters;

            public Configuration(int imgWidth, int imgHeight, int scale, int zoom, Tuple<double, double> coords)
            {
                parameters = new Dictionary<String, String>();

                SetLocation(coords);
                SetZoom(zoom);
                SetImageSize(imgHeight, imgWidth);
                SetImageScale(scale);
                parameters.Add("maptype", "satellite");
            }

            // adds the image dimensions to the current configuration
            public void SetImageSize(int width, int height)
            {
                if (parameters.ContainsKey("size")) parameters.Remove("size");
                parameters.Add("size", width + "x" + height);
            }

            // returns the dimensions of the image in the configuration
            public String GetImageDim()
            {
                String size;
                parameters.TryGetValue("size", out size);
                return size;
            }

            // adds the scale to the current configuration
            public void SetImageScale(int scale)
            {
                if (parameters.ContainsKey("scale")) parameters.Remove("scale");
                parameters.Add("scale", scale.ToString());
            }

            // returns the scale of the image in the configuration
            public String GetScale()
            {
                String scale;
                parameters.TryGetValue("scale", out scale);
                return scale;
            }

            // adds the coords to the current confuguration
            public void SetLocation(Tuple<double, double> coords)
            {
                if (parameters.ContainsKey("center")) parameters.Remove("center");
                parameters.Add("center", coords.Item1 + "," + coords.Item2);
            }

            // returns the location of the center for the current config
            public String GetLocation()
            {
                String center;
                parameters.TryGetValue("center", out center);
                return center;
            }

            // adds the zoom level to the current configuration
            public void SetZoom(int zoom)
            {
                if (parameters.ContainsKey("zoom")) parameters.Remove("zoom");
                parameters.Add("zoom", zoom.ToString());
            }

            // returns the zoom value of the configuration
            public String GetZoom()
            {
                String zoom;
                parameters.TryGetValue("zoom", out zoom);
                return zoom;
            }

            // returns the string representation of the configuration
            public override String ToString()
            {
                String result = "";
                foreach (String key in parameters.Keys)
                {
                    result += key + "=" + parameters[key] + "&";
                }
                result += "key=" + Environment.GetEnvironmentVariable("GOOGLEMAPSAPIKEY");
                return result;
            }
        }

        // gets the tile set of maps with the given coords of the center, width and height of tiling
        // and configuration for the center tile
        public static void DownloadNewTileSet(Tuple<int, int> tilingDim, Configuration config, String mapSetName)
        {
            String fileName = Directory.GetCurrentDirectory().ToString() + @"\MapTiles\" + mapSetName + ".txt";
            using (StreamWriter file = new StreamWriter(fileName))
            {
                int zoom;
                int imgWidth;
                int imgHeight;
                double longe;
                double lat;

                String[] imgDim = config.GetImageDim().Split('x');
                String[] coords = config.GetLocation().Split(',');
                Int32.TryParse(imgDim[0], out imgWidth);
                Int32.TryParse(imgDim[1], out imgHeight);
                Int32.TryParse(config.GetZoom(), out zoom);
                Double.TryParse(coords[0], out lat);
                Double.TryParse(coords[1], out longe);

                Tuple<int, int> centerPoint = MapConversion.LatLongToPixelXY(lat, longe, zoom);

                file.WriteLine(config.GetImageDim() + "|" + zoom + "|" + config.GetScale());

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
                            (centerPoint.Item1 + (i * imgWidth), centerPoint.Item2 + (j * imgHeight), zoom);
                        config.SetLocation(newCoords);
                        file.WriteLine(i + "," + j + "|" + Fetch(config));
                    }
                }
            }
        }

        // pulls a single map tile from google maps returns the hash code used for the file name.
        public static String Fetch(Configuration config)
        {
            String requestTile = "https://maps.googleapis.com/maps/api/staticmap?" + config.ToString();

            HttpWebRequest request = WebRequest.CreateHttp(requestTile);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream input = response.GetResponseStream();

            MemoryStream buffer;
            String bufferHash;
            // writes to the buffer stream
            using (buffer = new MemoryStream())
            {
                input.CopyTo(buffer);
                //CopyStream(input, buffer);
                bufferHash = buffer.GetHashCode().ToString();
                String fileName = Directory.GetCurrentDirectory().ToString() + @"\MapTiles\" + bufferHash + ".jpeg";
                buffer.Position = 0;
                buffer.CopyTo(File.Create(fileName));
            }
            return bufferHash;
        }
    }
}
