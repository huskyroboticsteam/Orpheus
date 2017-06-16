using System;
using System.IO;

namespace RoboticsLibrary.Components
{
    public interface ICamera
    {
        /// <summary>
        /// Takes a picture.
        /// </summary>
        void TakePicture();

        /// <summary>
        /// Checks to make sure the camera is functioning correctly.
        /// </summary>
        bool Test();
    }

    public class PictureTaken : EventArgs
    {
        public FileInfo Image { get; set; }
    }
}
