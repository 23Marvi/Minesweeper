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

        public bool ShowNum { get; set; }

        public void Update() {
            Graphics G = Parent.CreateGraphics();
            SolidBrush RB = new SolidBrush(BackColor);
            SolidBrush SB = new SolidBrush(ForeColor);

            G.FillRectangle(RB, new Rectangle(Location, Size));

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