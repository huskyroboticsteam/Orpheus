using System;
using System.Windows.Forms;

namespace UseArm
{
    public class ArmVisualizer : Form
    {
        Arm a;
        public ArmVisualizer(Arm Arm)
        {
            this.a = Arm;
            this.Visible = true;
            this.Paint += (object sender, PaintEventArgs e) =>
            {
                a.Draw(e.Graphics);
            };

            this.MouseDown += (object sender, MouseEventArgs e) =>
            {
                a.MoveTo((e.X - 100) / 10, (200 - e.Y) / 10, 0);
                this.Invalidate();
            };

            this.MouseMove += (object sender, MouseEventArgs e) =>
            {
                a.MoveTo((e.X - 100) / 10, (200 - e.Y) / 10, 0);
                this.Invalidate();
            };
        }
    }
}
