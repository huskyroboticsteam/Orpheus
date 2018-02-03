using System;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;

namespace UseArm
{
    public class ArmVisualizer : Form
    {
        Arm a;

        private static (float, float) InverseTransformPoint(float X, float Y)
        {
            const float SCALE = 10.0f;
            const float CX = 100.0f;
            const float CY = 100.0f;
            return ((X - CX) / SCALE, (300 - CY - Y) / SCALE);
        }

        //Only used for drawing. Scales and flips the point in order to 
        //provide a more human-friendly orientation. 
        private static (float, float) TransformPoint(float X, float Y)
        {
            const float SCALE = 10.0f;
            const float CX = 100.0f;
            const float CY = 100.0f;
            return (X * SCALE + CX, 300 - (Y * SCALE + CY));
        }

        //Draw is 2D and uses only Pitch. Takes a Graphics object to draw to. 
        public static void Draw(Arm a, Graphics g)
        {
            float PrevX = 0;
            float PrevY = 0;
            float X = 0;
            float Y = 0;
            float AngleSum = 0.0f;
            for (int i = 0; i < a.CurrentPitches.Length; i++)
            {
                ArmPart[] Params = typeof(Arm).GetField("Params", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(a) as ArmPart[];
                AngleSum += a.CurrentPitches[i];
                X += (float)(Params[i].Length * Math.Cos(AngleSum));
                Y += (float)(Params[i].Length * Math.Sin(AngleSum));
                var Trans1 = TransformPoint(PrevX, PrevY);
                g.FillEllipse(Brushes.DarkRed, Trans1.Item1 - 3, Trans1.Item2 - 3, 6, 6);
                var Trans2 = TransformPoint(X, Y);
                g.DrawLine(Pens.Red, Trans1.Item1, Trans1.Item2, Trans2.Item1, Trans2.Item2);
                PrevX = X;
                PrevY = Y;
            }
            var Trans = TransformPoint(PrevX, PrevY);
            g.FillEllipse(Brushes.DarkRed, Trans.Item1 - 3, Trans.Item2 - 3, 6, 6);
        }
        public ArmVisualizer(Arm Arm)
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
            var (X, Y) = InverseTransformPoint(e.X, e.Y);
            a.MoveTo(X, Y, 0);
            this.Invalidate();
        }
    }
}
