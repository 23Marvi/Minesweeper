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


        public bool Border = true;

        public void Update() {
            Graphics G = Parent.CreateGraphics();
            SolidBrush RB = new SolidBrush(BackColor);
            SolidBrush SB = new SolidBrush(ForeColor);

            

            Pen PPen = new Pen(Parent.BackColor);
            if (Border) {
                Point BLoc = new Point(Location.X + 1, Location.Y + 1);
                Size BSize = new Size(Size.Width - 2, Size.Height - 2);
                G.FillRectangle(RB, new Rectangle(BLoc, BSize));
            } else G.FillRectangle(RB, new Rectangle(Location, Size));


            int FontSize = Size.Width / 2;
            if (FontSize <= 0) FontSize = 1;
            Font = new Font("Arial", FontSize);



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