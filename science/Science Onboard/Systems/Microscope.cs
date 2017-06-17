using System;
using System.Diagnostics;
using Scarlet.Components;

namespace Science.Systems
{
    public class Microscope : ICamera
    {
        public event EventHandler<PictureTaken> PictureNotify;

        public Microscope()
        {
            
        }

        public void EventTriggered(object Sender, EventArgs Event)
        {
            PictureTaken CreatedEvent = new PictureTaken();
            CreatedEvent.Image = new System.IO.FileInfo(GetFilename(false));
            this.OnPicture(CreatedEvent);
        }

        public void TakePicture()
        {
            Process ImgCap = new Process();
            ImgCap.StartInfo.FileName = "fswebcam";
            ImgCap.StartInfo.Arguments = "-r 1600x1200 " + GetFilename(true);
            ImgCap.EnableRaisingEvents = true;
            ImgCap.Start();
            ImgCap.Exited += this.EventTriggered;
        }

        public bool Test()
        {
            // TODO: Test camera.
            return true;
        }

        private string GetFilename(bool Advance)
        { // TODO: Return a filename.
            return null;
        }

        protected virtual void OnPicture(PictureTaken Event)
        {
            PictureNotify?.Invoke(this, Event);
        }
    }
}
