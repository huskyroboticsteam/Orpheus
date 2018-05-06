using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace Science_Base
{
    public class UIHelper
    {
        public static SolidColorBrush ScarletColour = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0x81, 0x14, 0x26));
        public static SolidColorBrush ScarletBackColour = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0x3F, 0x81, 0x14, 0x26));
        public static SolidColorBrush TextColour = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0xCC, 0xCC, 0xCC));
        public static SolidColorBrush GaugeRedColour = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0xD1, 0x26, 0x26));
        public static SolidColorBrush GaugeYellowColour = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0xD1, 0xD1, 0x26));
        public static SolidColorBrush GaugeGreenColour = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0x26, 0xD1, 0x26));
        
        public static void Init()
        {
            ScarletColour.Freeze();
            ScarletBackColour.Freeze();
            TextColour.Freeze();
            GaugeRedColour.Freeze();
            GaugeYellowColour.Freeze();
            GaugeGreenColour.Freeze();
        }

        public static void SetMode(PictureBox Box, Image Original, byte Mode)
        {
            float[][] Good =
            {
                new float[] { 0, 0, 0, 0, 0 },
                new float[] { 0, 0.8F, 0, 0, 0 },
                new float[] { 0, 0, 0, 0, 0 },
                new float[] { 0, 0, 0, 1, 0 },
                new float[] { 0, 0, 0, 0, 0 }
            };

            float[][] Warning =
            {
                new float[] { 0.8F, 0, 0, 0, 0 },
                new float[] { 0, 0.8F, 0, 0, 0 },
                new float[] { 0, 0, 0, 0, 0 },
                new float[] { 0, 0, 0, 1, 0 },
                new float[] { 0, 0, 0, 0, 0 }
            };

            float[][] Error =
            {
                new float[] { 0.8F, 0, 0, 0, 0 },
                new float[] { 0, 0, 0, 0, 0 },
                new float[] { 0, 0, 0, 0, 0 },
                new float[] { 0, 0, 0, 1, 0 },
                new float[] { 0, 0, 0, 0, 0 }
            };

            float[][] Default =
            {
                new float[] { 0.8F, 0, 0, 0, 0 },
                new float[] { 0, 0.8F, 0, 0, 0 },
                new float[] { 0, 0, 0.8F, 0, 0 },
                new float[] { 0, 0, 0, 1, 0 },
                new float[] { 0, 0, 0, 0, 0 }
            };

            ColorMatrix Matrix;
            switch (Mode)
            {
                case 0: Matrix = new ColorMatrix(Good); break;
                case 1: Matrix = new ColorMatrix(Warning); break;
                case 2: Matrix = new ColorMatrix(Error); break;
                default: Matrix = new ColorMatrix(Default); break;
            }
            ImageAttributes Attributes = new ImageAttributes();
            Attributes.SetColorMatrix(Matrix);
            Image Image = (Image)Original.Clone();
            Graphics Graphics = Graphics.FromImage(Image);
            Rectangle Output = new Rectangle(0, 0, Image.Width, Image.Height);
            Graphics.DrawImage(Image, Output, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, Attributes);
            Box.Image = Image;
        }

    }
}
