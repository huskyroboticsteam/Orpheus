using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Science_Base
{
    public partial class RailDisplay : Control
    {
        /// <summary> The location of the drill. </summary>
        /// 100 = Top
        /// 0 = Ground
        /// -20 = Fully in ground
        public int DrillLocation
        {
            get => this.P_DrillLocaton;
            set
            {
                this.P_DrillLocaton = value;
                this.Invalidate();
            }
        }
        private int P_DrillLocaton;

        public bool ShowDistanceTop { get; set; }
        public bool ShowDistanceBottom { get; set; }

        public RailDisplay()
        {
            this.ForeColor = Color.FromArgb(220, 220, 220);
            this.DrillLocation = 75;
            InitializeComponent();
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            pe.Graphics.FillRectangle(new SolidBrush(Color.Black), pe.ClipRectangle);

            SolidBrush Fore = new SolidBrush(this.ForeColor);

            Rectangle DiagramArea = ShrinkRect(pe.ClipRectangle, 0, 0, 10, 0); // Leaves space for the height indicators

            Rectangle TopBar = new Rectangle(DiagramArea.Location, new Size(DiagramArea.Width, 5));
            pe.Graphics.FillRectangle(Fore, TopBar);

            Rectangle VerticalBar = new Rectangle(MovePt(DiagramArea.Location, (int)(DiagramArea.Width * 0.2F), 3), new Size(2, DiagramArea.Height - 3));
            pe.Graphics.FillRectangle(Fore, VerticalBar);

            int GroundHeight = (int)(DiagramArea.Height * 0.75F); // Top of the ground line
            Rectangle GroundLine = new Rectangle(MovePt(DiagramArea.Location, 0, GroundHeight), new Size(DiagramArea.Width, 2));
            pe.Graphics.FillRectangle(new SolidBrush(Color.SandyBrown), GroundLine);

            Rectangle DrillTop = new Rectangle(MovePt(VerticalBar.Location, 2, (int)((GroundHeight - (TopBar.Location.Y + TopBar.Height) - 2) * (100 - this.DrillLocation) / 100.0F) + 2), new Size(DiagramArea.Width / 2, 2));
            pe.Graphics.FillRectangle(Fore, DrillTop);
        }

        private Rectangle ShrinkRect(Rectangle Input, int Left, int Top, int Right, int Bottom)
        {
            Point Loc = new Point(Input.Location.X + Left, Input.Location.Y + Top);
            Size Size = new Size(Input.Size.Width - (Left + Right), Input.Size.Height - (Top + Bottom));
            Rectangle Rect = new Rectangle(Loc, Size);
            return Rect;
        }

        private Point MovePt(Point Point, int X, int Y)
        {
            Point.Offset(X, Y);
            return Point;
        }
    }
}
