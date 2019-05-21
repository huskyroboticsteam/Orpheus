﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scarlet.Utilities;

namespace Science_Base
{
    public partial class TurntableDisplay : Control
    {
        public int Angle
        {
            get => this.P_Angle;
            set
            {
                this.P_Angle = value;
                Invalidate();
            }
        }
        private int P_Angle;

        public byte InitStatus
        {
            get => this.P_InitStatus;
            set
            {
                this.P_InitStatus = value;
                Invalidate();
            }
        }
        private byte P_InitStatus;

        public TurntableDisplay()
        {
            this.ForeColor = Color.FromArgb(220, 220, 220);
            this.Angle = 30;
            InitializeComponent();
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            Rectangle OverallArea = pe.ClipRectangle;
            SolidBrush Fore = new SolidBrush(this.ForeColor);
            pe.Graphics.FillPolygon(Fore, new Point[] // The triangle indicator at the top.
            {
                new Point((OverallArea.Width / 2) - 8, 5),
                new Point((OverallArea.Width / 2) + 8, 5),
                new Point(OverallArea.Width / 2, 15)
            });

            Color TableColour;
            if (this.InitStatus == 1) { TableColour = Color.Yellow; }
            else if (this.InitStatus == 2) { TableColour = Color.Green; }
            else { TableColour = Color.Red; }

            Pen TablePen = new Pen(TableColour, 1);

            Rectangle TableSpace = ShrinkRect(OverallArea, 0, 16, 0, 0);
            int Radius = (Math.Min(TableSpace.Width, TableSpace.Height) / 2) - 4;

            Rectangle Circle = ShrinkRect(TableSpace, (TableSpace.Width - (Radius * 2)) / 2, 4, (TableSpace.Width - (Radius * 2)) / 2, (TableSpace.Height - (Radius * 2)) - 4);
            pe.Graphics.DrawEllipse(TablePen, Circle);
            PointF TableCenter = new PointF((float)(Circle.Left + Circle.Width / 2.0), (float)(Circle.Top + (Circle.Height / 2.0)));

            pe.Graphics.DrawLine(TablePen, TableCenter, Radial(TableCenter, 90 - this.Angle, Radius)); // Home line
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

        private PointF Radial(PointF Center, int Angle, int Radius)
        {
            double ToRad = Math.PI / 180;
            return new PointF((float)(Center.X + (Math.Cos(Angle * ToRad) * Radius)), (float)(Center.Y - (Math.Sin(Angle * ToRad) * Radius)));
        }
    }
}
