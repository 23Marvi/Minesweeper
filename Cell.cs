using System;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper {
    class Cell {
        public Size Size;
        public Point Location;
        public Color BackColor;
        public Color ForeColor;
        public string Name;
        public Control Parent;
        public Font Font;

        public int SMine;

        public bool ShowNum;
        public bool Border;

        public void Update() {
            Graphics G = Parent.CreateGraphics();
            SolidBrush RB = new SolidBrush(BackColor);
            SolidBrush SB = new SolidBrush(ForeColor);

            G.FillRectangle(RB, new Rectangle(Location, Size));

            if (Border) {
                Pen PPen = new Pen(Parent.BackColor);

                Point NL = new Point(Location.X + 1, Location.Y + 1);
                Size NS = new Size(Size.Width - 1, Size.Height - 1);

                G.DrawRectangle(PPen, new Rectangle(NL, NS));
            }

            StringFormat SF = new StringFormat();
            SF.LineAlignment = StringAlignment.Center;
            SF.Alignment = StringAlignment.Center;
            if (ShowNum) {
                if (SMine > 0) 
                    G.DrawString(SMine.ToString(), Font, SB, new Rectangle(Location, Size), SF);
            }
        }
    }
}