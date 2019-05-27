using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Scarlet.Utilities;

namespace Science_Base
{
    public class Microscope
    {
        public bool TraceLogging { get; set; } = true;

        public Microscope()
        {
            string Folder = Path.Combine(Environment.CurrentDirectory, "Images" + Path.DirectorySeparatorChar);
            Log.Output(Log.Severity.INFO, Log.Source.GUI, "Looking in folder " + Folder);
            string FileName = Path.Combine(Folder, "test004.jpg");

            Stopwatch Time = new Stopwatch();
            Time.Restart();
            Bitmap Img = new Bitmap(FileName);
            
            BitmapData ImgData = Img.LockBits(new Rectangle(Img.Width / 3, Img.Height / 3, Img.Width / 3, Img.Height / 3), ImageLockMode.ReadOnly, Img.PixelFormat);
            int Width = ImgData.Width;
            int Height = ImgData.Height;
            int ArrSize = ImgData.Stride * ImgData.Height; // Takes into account the number of bytes per pixel

            byte[] Data = new byte[ArrSize];
            Marshal.Copy(ImgData.Scan0, Data, 0, ArrSize);
            Img.UnlockBits(ImgData);

            int Sharp = GetShaprness(Data, Width, Height);
            Time.Stop();
            if (this.TraceLogging) { Log.Trace(this, "Calculated sharpness of " + Sharp + " in " + Time.ElapsedMilliseconds + "ms."); }
            Img.Dispose();
        }

        private void TakePicture(string Path, string Filename)
        {
            Process Capture = new Process();
            Capture.StartInfo.FileName = "fswebcam";
            Capture.StartInfo.WorkingDirectory = Path;
            Capture.StartInfo.UseShellExecute = false;
            Capture.StartInfo.Arguments = "-r 1600x1200 " + Filename;
            Capture.Start();
            Capture.WaitForExit();
            if (this.TraceLogging) { Log.Trace(this, "Took picture \"" + Filename + "\"."); }
        }

        private int GetShaprness(byte[] Img, int Width, int Height)
        {
            int Sum = 0;
            for (int x = 0; x < Width - 1; x++)
            {
                for (int y = 0; y < Height - 1; y++)
                {
                    // Compare to pixel to the right
                    Sum += IntPow((Img[((y * Width) + (x + 1)) * 3 + 0] - Img[((y * Width) + x) * 3 + 0]), 2);
                    Sum += IntPow((Img[((y * Width) + (x + 1)) * 3 + 1] - Img[((y * Width) + x) * 3 + 1]), 2);
                    Sum += IntPow((Img[((y * Width) + (x + 1)) * 3 + 2] - Img[((y * Width) + x) * 3 + 2]), 2);

                    // Compare to pixel below
                    Sum += IntPow((Img[(((y + 1) * Width) + x) * 3 + 0] - Img[(((y + 1) * Width) + x) * 3 + 0]), 2);
                    Sum += IntPow((Img[(((y + 1) * Width) + x) * 3 + 1] - Img[(((y + 1) * Width) + x) * 3 + 1]), 2);
                    Sum += IntPow((Img[(((y + 1) * Width) + x) * 3 + 2] - Img[(((y + 1) * Width) + x) * 3 + 2]), 2);
                }
            }
            return Sum;
        }

        private int IntPow(int x, uint Exp)
        {
            int Out = 1;
            while (Exp != 0)
            {
                if ((Exp & 1) == 1) { Out *= x; }
                x *= x;
                Exp >>= 1;
            }
            return Out;
        }

    }
}
