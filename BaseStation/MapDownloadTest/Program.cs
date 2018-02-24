using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using HuskyRobotics.Utilities;

// google API key: AIzaSyDr7Tuv6bar9jkYbz23b3jv0RlHLnhtzxU
// https://msdn.microsoft.com/en-us/library/bb259689.aspx#Map map system (its bing but google
// uses the same system)
// https://google-developers.appspot.com/maps/documentation/static-maps/intro#Zoomlevels google
// tile download documentation

// To Do:
// Download tile set
// store tile set info in file using hash for map file names
// add on to existing tile set

namespace MapDownloadTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int imagewidth = 100;
            int imageheight = 100;
            int scale = 2;
            int zoom = 19;
            // shift x and y by image height and width
            MapTileDownloadManager.Configuration config1 = new MapTileDownloadManager.Configuration
                (imagewidth, imageheight, scale, zoom, 47.653741, -122.304855);
            Tuple<int, int> point = MapConversion.LatLongToPixelXY(new Tuple<double, double>(47.653741, -122.304855), zoom);
            Tuple<double, double> coords = MapConversion.PixelXYToLatLong(point.Item1, point.Item2 + imageheight, zoom);
            MapTileDownloadManager.Configuration config2 = new MapTileDownloadManager.Configuration
                (imagewidth, imageheight, scale, zoom, coords.Item1, coords.Item2);
            MapTileDownloadManager.Fetch(config1, "test1.jpeg");
            MapTileDownloadManager.Fetch(config2, "test2.jpeg");
        }
    }

    public static class MapTileDownloadManager
    {
        public const String APIKEY = "AIzaSyDr7Tuv6bar9jkYbz23b3jv0RlHLnhtzxU";

        // a class to hold the configuration for the map dowload, formated as specified by google
        public class Configuration
        {

            Dictionary<String, String> parameters;

            public Configuration(int imgWidth, int imgHeight, int scale, int zoom, double longe, double lat)
            {
                parameters = new Dictionary<String, String>();

                addLocation(longe, lat);
                addZoomLevel(zoom);
                addImageSize(imgHeight, imgWidth);
                addImageScale(scale);
                parameters.Add("maptype", "satellite");
            }

            public void addImageSize(int width, int height)
            {
                parameters.Add("size", width + "x" + height);
            }

            public void addImageScale(int scale)
            {
                parameters.Add("scale", scale.ToString());
            }

            public void addLocation(double longitude, double latitude)
            {
                parameters.Add("center", longitude + "," + latitude);
            }

            public void addZoomLevel(int zoom)
            {
                parameters.Add("zoom", zoom.ToString());
            }

            public override String ToString()
            {
                String result = "";
                foreach (String key in parameters.Keys)
                {
                    result += key + "=" + parameters[key] + "&";
                }
                result += "key=" + APIKEY;
                return result;
            }
        }

        // gets the tile set of maps with the given coords of the center, width and height of tiling and
        public static void DownloadTileSet(Tuple<double, double> coordsMid, Tuple<int, int> tilingDim, Configuration config)
        {
            if (tilingDim.Item1 <= 0 || tilingDim.Item2 <= 0)
            {
                throw new ArgumentException("Dimesions may not be less that or equal to zero", "tilingDim");
            }

        }

        // pulls a single map tile from google maps
        public static void Fetch(Configuration config, String fileName)
        {
            String googleUrl = "https://maps.googleapis.com/maps/api/staticmap?" + config.ToString();
            String filename = Directory.GetCurrentDirectory().ToString() + @"\" + fileName;

            Uri uri = new Uri(googleUrl);

            HttpWebRequest request = WebRequest.CreateHttp(googleUrl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream input = response.GetResponseStream();

            MemoryStream buffer = new MemoryStream();

            using (Stream file = File.Create(filename))
            {
                CopyStream(input, file);
            }
        }

        // copies the given input stream to the given output stream
        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int count;
            while ((count = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, count);
            }
        }
    }
}
