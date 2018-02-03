using System;
using System.Net;
using System.IO;
using System.Collections.Generic;

namespace TileSetDownloader
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            MapGenerator.Configuration config = new MapGenerator.Configuration();

            config.setImageSize(3000, 3000);
            config.setImageScale(2);
            config.setZoomLevel(19);
            config.setLocation(38.4064262, -110.7941097);
            config.setSatelliteImage();

            MapGenerator.Fetch(config);
        }
    }

    class MapGenerator
    {
        public class Configuration
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();

            public void setImageSize(int width, int height)
            {
                parameters.Add("size", width + "x" + height);
            }

            public void setImageScale(int scale)
            {
                parameters.Add("scale", scale.ToString());
            }

            public void setLocation(double longitude, double latitude)
            {
                parameters.Add("center", longitude + "," + latitude);
            }

            public void setZoomLevel(int zoom)
            {
                parameters.Add("zoom", zoom.ToString());
            }

            public void setSatelliteImage()
            {
                parameters.Add("maptype", "satellite");
            }

            public String toString()
            {
                String result = "";
                foreach (String key in parameters.Keys)
                {
                    result += key + "=" + parameters[key] + "&";
                }
                return result;
            }
        }

        public static void Fetch(MapGenerator.Configuration config)
        {
            String googleUrl = "https://maps.googleapis.com/maps/api/staticmap?" + config.toString();
            String filename = "/Users/sydneyzapf/Desktop/map_image.jpg";

            Uri uri = new Uri(googleUrl);

            HttpWebRequest request = WebRequest.CreateHttp(googleUrl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream input = response.GetResponseStream();

            using (Stream file = File.Create(filename))
            {
                MapGenerator.CopyStream(input, file);
            }
        }

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