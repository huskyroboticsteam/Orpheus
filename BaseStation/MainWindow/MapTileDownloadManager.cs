using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using HuskyRobotics.Utilities;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Drawing;
using ImageProcessor.Imaging.Formats;
using ImageProcessor;

// make sure there is a folder named Images in the working directory
// https://msdn.microsoft.com/en-us/library/bb259689.aspx#Map map system (its bing but google
// uses the same system)
// https://google-developers.appspot.com/maps/documentation/static-maps/intro#Zoomlevels google
// tile download documentation
// Image factory documentation: http://imageprocessor.org/imageprocessor/imagefactory/#about

namespace HuskyRobotics.UI
{
    public static class MapTileDownloadManager
    {
        public static void DownloadNewTileSet(MapConfiguration config)
        {
            DownloadNewTileSet(config, null);
        }

        // gets the tile set of maps with the given coords of the center, width and height of tiling
        // and configuration for the center tile
        public static void DownloadNewTileSet(MapConfiguration config, BackgroundWorker worker)
        {
            String fileName = Directory.GetCurrentDirectory().ToString() + @"\Images\" + config.MapSetName + ".map";
            using (StreamWriter file = new StreamWriter(fileName))
            {
                Tuple<int, int> centerPoint = MapConversion.LatLongToPixelXY(config.Latitude,
                    config.Longitude, config.Zoom);

                file.WriteLine(config.ImgWidth + "x" + config.ImgHeight + "|" + config.Latitude + ","
                    + config.Longitude + "|" + config.Zoom + "|" + config.Scale + "|" + config.MapType);

                // center of the tiling is 0,0
                int startx = -config.TilingWidth / 2;
                int starty = -config.TilingHeight / 2;
                if (config.TilingWidth % 2 == 0) startx++;
                if (config.TilingHeight % 2 == 0) starty++;
                int numberDownloaded = 0;

                for (int i = startx; i <= config.TilingWidth / 2; i++)
                {
                    for (int j = starty; j <= config.TilingHeight / 2; j++)
                    {
                        if (worker != null)
                        {
                            worker.ReportProgress(numberDownloaded);
                        }

                        Tuple<double, double> newCoords = MapConversion.PixelXYToLatLong
                            (centerPoint.Item1 + (i * config.ImgWidth), centerPoint.Item2
                            + (j * config.ImgHeight), config.Zoom);
                        config.Latitude = newCoords.Item1;
                        config.Longitude = newCoords.Item2;
                        string imageName = Fetch(config);
                        if (imageName != null)
                        {
                            file.WriteLine(i + "," + j + "|" + imageName);
                        }

                        numberDownloaded++;
                    }
                }
            }
        }

        // pulls a single map tile from google maps returns the hash code used for the file name.
        // Will return null if the image was not downloaded
        public static String Fetch(MapConfiguration config)
        {
            String requestUrl = "https://maps.googleapis.com/maps/api/staticmap?" + config.URLParams();

            HttpWebRequest request = WebRequest.CreateHttp(requestUrl);
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            } catch (WebException ex)
            {
                Console.WriteLine(ex.Status + " " + ex.Message);
                return null;
            }

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

                if (File.Exists(fileName))
                {
                    return bufferHash; // Already saved the file
                }
                
                buffer.Position = 0;
                CropImage(fileName, config, buffer);
                return bufferHash;
            }
        }
        
        private static void CropImage(string file, MapConfiguration config, MemoryStream inStream)
        {
            // this was done all to the documentation
            ISupportedImageFormat format = new PngFormat();
            Size size = new Size(config.ImgWidth * config.Scale, config.ImgHeight * config.Scale);
            Rectangle crop = new Rectangle(new Point(0, 0), size);
            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
            {
                imageFactory.Load(inStream)
                            .Crop(crop)
                            .Resize(size)
                            .Format(format)
                            .Save(file);
            }
        }
    }
}
