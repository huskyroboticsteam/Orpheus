using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Scarlet.Components;
using Scarlet.Components.Motors;
using Scarlet.Filters;
using Scarlet.IO;
using Scarlet.Utilities;
using static Scarlet.Components.Outputs.PCA9685;

namespace Science.Systems
{
    public class Microscope : ISubsystem
    {
        public bool TraceLogging { get; set; } = true;

        private readonly string Folder, FolderAF;
        private readonly Servo Servo;
        private readonly Average<int> ServoFilter;
        private int ServoPos;

        private const string KeyFolder = "LastFolder";
        private const string KeyFile = "LastFile";

        public Microscope(IPWMOutput ServoOut)
        {
            this.Folder = Path.Combine(Environment.CurrentDirectory, "Images" + Path.DirectorySeparatorChar);
            this.FolderAF = Path.Combine(this.Folder, "AutofocusSets" + Path.DirectorySeparatorChar);
            Log.Output(Log.Severity.INFO, Log.Source.GUI, "Looking in folder " + this.Folder);

            if (!StateStore.Started) { StateStore.Start("RoverScience"); }
            StateStore.GetOrCreate(KeyFolder, "0");
            StateStore.GetOrCreate(KeyFile, "0");

            this.ServoFilter = new Average<int>(4);
            ((PWMOutputPCA9685)ServoOut).SetPolarity(true);
            this.Servo = new Servo(ServoOut, 100, 2.8F, 0.55F);
            this.Servo.SetEnabled(true);
        }

        public void Initialize() { }

        public void EmergencyStop() { }

        public void UpdateState()
        {
            this.ServoFilter.Feed(this.ServoPos);
            this.Servo.SetPosition(this.ServoFilter.GetOutput());
            if (this.TraceLogging) { Log.Trace(this, "Servo going to " + this.ServoFilter.GetOutput()); }
        }

        public void Exit() { }

        public void TakePictureHalting(object DoAutofocusObj)
        {
            bool DoAutofocus = (bool)DoAutofocusObj;
            if (!DoAutofocus)
            {
                int FileNum = int.Parse(StateStore.Get(KeyFile)) + 1;
                StateStore.Set(KeyFile, FileNum.ToString());
                StateStore.Save();
                TakePicture(this.Folder, FileNum.ToString("D5") + ".jpg");
            }
            else
            {
                int FolderNum = int.Parse(StateStore.Get(KeyFolder)) + 1;
                StateStore.Set(KeyFolder, FolderNum.ToString());
                StateStore.Save();
                string CurrentFolder = Path.Combine(this.FolderAF, FolderNum.ToString("D5") + Path.DirectorySeparatorChar);
                Directory.CreateDirectory(CurrentFolder);
                AutofocusMain(CurrentFolder);
            }
        }

        public void TakePicture(bool DoAutofocus)
        {
            Thread PictureThread = new Thread(new ParameterizedThreadStart(TakePictureHalting));
            PictureThread.Start(DoAutofocus);
        }

        public void MoveServo(int Position)
        {
            this.ServoPos = Position;
        }

        private void AutofocusMain(string Folder)
        {
            if (this.TraceLogging) { Log.Trace(this, "Taking initial picture."); }
            TakePicture(Folder, "A001.jpg");
            int PictureNum = 2;
            int Sharpness = GetSharpness(Path.Combine(Folder, "A001.jpg"));
            if (this.TraceLogging) { Log.Trace(this, "Initial picture sharpness: " + Sharpness); }
            while(LimitMoveAndContinue(Cycle(Sharpness)))
            {
                string FileName = AFPicName(PictureNum);
                PictureNum++;
                TakePicture(Folder, FileName);
                Sharpness = GetSharpness(Path.Combine(Folder, FileName));
                if (this.TraceLogging) { Log.Trace(this, "Took picture " + FileName + ", sharpness " + Sharpness); }
            }
            int BestPictureInd = 0;
            for (int i = 0; i < PicFoci.Count; i++)
            {
                if (PicFoci[i] > PicFoci[BestPictureInd]) { BestPictureInd = i; }
            }
            Log.Output(Log.Severity.INFO, Log.Source.CAMERAS, "Best AF picture was #" + BestPictureInd + 1);

            PicFoci.Clear();
            ServoPositions.Clear();
            FocusIsNear = false;
            FocusWasVeryGood = false;
            MovementQty = 7F;
            
        }

        private float MovementQty = 7F;
        private List<int> PicFoci = new List<int>(40);
        private List<int> ServoPositions = new List<int>(40);
        private bool FocusIsNear, FocusWasVeryGood;

        private bool LimitMoveAndContinue(float Movement)
        {
            if(Math.Abs(Movement) > 0.2F)
            {
                if ((this.ServoPos + Movement) < 0 || (this.ServoPos + Movement) > 100) // We'd go out of bounds
                {
                    Movement *= 0.5F;
                    MovementQty *= -1;
                }
                this.ServoPos = (int)(this.ServoPos + Movement);
                Thread.Sleep(300);
                return true;
            }
            else
            {
                return false;
            }
        }

        private float Cycle(int CurrFocus)
        {
            PicFoci.Add(CurrFocus);
            ServoPositions.Add(ServoPos);
            float Avg = AvgList(PicFoci);
            if(PicFoci.Count > 40)
            {
                if (this.TraceLogging) { Log.Trace(this, "Took 40 pictures, giving up."); }
                return 0;
            }
            if(CurrFocus > (Avg * 1.12))
            {
                // We are close, slow down and check direction.
                if(PicFoci.Last() > CurrFocus * 1.05)
                {
                    if (this.TraceLogging) { Log.Trace(this, "Close, but was better before. Moving back slowly."); }
                    MovementQty *= -0.6F;
                }
                else
                {
                    if (this.TraceLogging) { Log.Trace(this, "Close, but still improving. Slowing down to avoid overshoot."); }
                    if(FocusIsNear)
                    {
                        if(CurrFocus > (Avg * 5))
                        {
                            if (this.TraceLogging) { Log.Trace(this, "Focus is very good. Slowing down significantly."); }
                            FocusWasVeryGood = true;
                            MovementQty *= 0.5F;
                        }
                        if(Math.Abs(MovementQty) < 3)
                        {
                            if (this.TraceLogging) { Log.Trace(this, "Already moving slow, not stopping yet."); }
                            MovementQty *= (1 / MovementQty);
                        }
                        else
                        {
                            if (FocusWasVeryGood) { MovementQty *= -0.5F; }
                            else { MovementQty *= 0.5F; }
                        }
                    }
                    else
                    { // We just got close, don't slow down as much.
                        MovementQty *= 0.6F;
                    }
                }
                FocusIsNear = true;
            }
            else if(FocusIsNear)
            {
                if (this.TraceLogging) { Log.Trace(this, "Focus was near, is no longer. Need to go back."); }
                MovementQty *= -0.95F;
                FocusIsNear = false;
                return MovementQty;
            }
            else
            {
                if (this.TraceLogging) { Log.Trace(this, "Focus was not near, still is not. Continuing along."); }
                return MovementQty;
            }
            return MovementQty;
        }

        private float AvgList(List<int> List)
        {
            double Total = 0;
            foreach (int Item in List) { Total += Item; }
            return (float)(Total / List.Count);
        }

        private string AFPicName(int Num) => "A" + Num.ToString("D3") + ".jpg";

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

        private int GetSharpness(string FileName)
        {
            if (this.TraceLogging) { Log.Trace(this, "Checking sharpness of image file \"" + FileName + "\""); }
            Bitmap Img = new Bitmap(FileName);

            BitmapData ImgData = Img.LockBits(new Rectangle(Img.Width / 3, Img.Height / 3, Img.Width / 3, Img.Height / 3), ImageLockMode.ReadOnly, Img.PixelFormat);
            int Width = ImgData.Width;
            int Height = ImgData.Height;
            int ArrSize = ImgData.Stride * ImgData.Height; // Takes into account the number of bytes per pixel

            byte[] Data = new byte[ArrSize];
            Marshal.Copy(ImgData.Scan0, Data, 0, ArrSize);
            Img.UnlockBits(ImgData);

            int Sharp = GetShaprness(Data, Width, Height);
            Img.Dispose();
            return Sharp;
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
