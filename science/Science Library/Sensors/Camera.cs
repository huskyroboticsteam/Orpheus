using System;
using System.IO;

namespace RoboticsLibrary.Sensors
{
    public class Camera : Sensor
    {
        private readonly string DeviceName;
        public event EventHandler<PictureTaken> PictureNotify;

        public Camera(string DeviceName)
        {
            this.DeviceName = DeviceName;
        }

        public override bool GetsRegUpdates() { return false; }

        protected virtual void OnPicture(PictureTaken Event)
        {
            PictureNotify?.Invoke(this, Event);
        }
    }

    public class PictureTaken : EventArgs
    {
        public FileInfo Image { get; set; }
    }
}
