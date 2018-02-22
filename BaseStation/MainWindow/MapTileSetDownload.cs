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
            MapTileDownloadManager.Configuration config = new MapTileDownloadManager.Configuration
                (3000, 3000, 2, 19, 38.4064262, -110.7941097);
            MapTileDownloadManager.Fetch(config);
        }
    }

    class MapTileDownloadManager
    {
        public class Configuration
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();

            public Configuration(int imgWidth, int ingHeight, int scale, int zoom, double longe, double lat)
            {
                setImageSize(3000, 3000);
                setImageScale(2);
                setZoomLevel(19);
                setLocation(38.4064262, -110.7941097);
                parameters.Add("maptype", "satellite");
            }

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

        // pulls a single map tile from google maps
        public static void Fetch(Configuration config)
        {
            String googleUrl = "https://maps.googleapis.com/maps/api/staticmap?" + config.toString();
            String filename = Directory.GetCurrentDirectory().ToString() + "Test.jpg";

            Uri uri = new Uri(googleUrl);

            HttpWebRequest request = WebRequest.CreateHttp(googleUrl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream input = response.GetResponseStream();

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