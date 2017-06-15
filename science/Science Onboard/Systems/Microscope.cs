using System;
using System.Diagnostics;
using RoboticsLibrary.Sensors;

namespace Science.Systems
{
    public class Microscope : Camera
    {
        public Microscope() : base("/dev/video0")
        {
            
        }

        public void EventTriggered(object Sender, EventArgs Event)
        {
            PictureTaken CreatedEvent = new PictureTaken();
            CreatedEvent.Image = new System.IO.FileInfo(GetFilename(false));
            base.OnPicture(CreatedEvent);
        }

        public override bool Test()
        {
            // TODO: Test camera.
            return true;
        }

        /// <summary>
        /// Takes a pciture.
        /// </summary>
        public override void UpdateState()
        {
            Process ImgCap = new Process();
            ImgCap.StartInfo.FileName = "fswebcam";
            ImgCap.StartInfo.Arguments = "-r 1600x1200 " + GetFilename(true);
            ImgCap.Start();
            ImgCap.Exited += this.EventTriggered;
        }

        private string GetFilename(bool Advance)
        { // TODO: Return a filename.
            return null;
        }
    }
}
