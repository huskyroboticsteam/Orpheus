using System;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using HuskyRobotics.Arm;

namespace HuskyRobotics.Arm.Visualizer
{

    public class ArmVisualizer : Form
    {
        Armature a;
        (float, float) setpoint;
        const float PIXELS_PER_INCH = 5.0f;
        const float INCHES_PER_PIXEL = 1.0f / PIXELS_PER_INCH;

        private (float, float) ScreenToModel(float X, float Y)
        {
            return ((X - Width / 2) * INCHES_PER_PIXEL, (Height / 2 - Y) * INCHES_PER_PIXEL);
        }

        //Only used for drawing. Scales and flips the point in order to 
        //provide a more human-friendly orientation. 
        private (float, float) ModelToScreen(float X, float Y)
        {
            return ((Width / 2 + X * PIXELS_PER_INCH), (Height / 2 - Y * PIXELS_PER_INCH));
        }

        //Draw is 2D and uses only Pitch. Takes a Graphics object to draw to. 
        public void Draw(Armature a, Graphics g)
        {
            float PrevX = 0;
            float PrevY = 0;
            float X = 0;
            float Y = 0;
            float AngleSum = 0.0f;
            for (int i = 0; i < a.CurrentPitches.Length; i++)
            {
                ArmPart[] Params = a.Params;
                AngleSum += a.CurrentPitches[i];
                X += (float)(Params[i].Length * Math.Cos(AngleSum));
                Y += (float)(Params[i].Length * Math.Sin(AngleSum));
                var Trans1 = ModelToScreen(PrevX, PrevY);
                g.FillEllipse(Brushes.DarkRed, Trans1.Item1 - 3, Trans1.Item2 - 3, 6, 6);
                var Trans2 = ModelToScreen(X, Y);
                g.DrawLine(Pens.Red, Trans1.Item1, Trans1.Item2, Trans2.Item1, Trans2.Item2);
                PrevX = X;
                PrevY = Y;
            }
            var Trans = ModelToScreen(PrevX, PrevY);
            g.FillEllipse(Brushes.DarkRed, Trans.Item1 - 3, Trans.Item2 - 3, 6, 6);

            Trans = ModelToScreen(setpoint.Item1, setpoint.Item2);
            g.FillEllipse(Brushes.Black, Trans.Item1 - 2, Trans.Item2 - 2, 4, 4);
        }
        public ArmVisualizer(Armature Arm)
        {
            this.a = Arm;
            this.Visible = true;
            this.Paint += (object sender, PaintEventArgs e) =>
            {
                Draw(a, e.Graphics);
            };

            this.MouseDown += MoveAndRedraw;
            this.MouseMove += MoveAndRedraw;
        }
        private void MoveAndRedraw(object sender, MouseEventArgs e)
        {
            setpoint = ScreenToModel(e.X, e.Y);
            a.MoveTo(setpoint.Item1, setpoint.Item2, 0);
            this.Invalidate();
        }
    }
}
