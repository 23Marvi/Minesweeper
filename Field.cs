using System;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper {
    public enum CellType {
        Mine,
        Cell,
        FCell,
        Flag,
    }

    internal class Field {
        public Field(Control Parent) {
            this.Parent = Parent;
        }
        #region Static
        private static Random RND = new Random();
        #endregion
        #region Cell Click
        /// <summary>
        /// Click for long responses
        /// </summary>
        /// If you Leftclick on a mine, you lose
        /// If the game hasn't started yet, place all mines
        /// Else, reveal cells
        private void Cell_Click(object sender, EventArgs e) {
            MouseEventArgs me = (MouseEventArgs)e;
            Label Cell = (Label)sender;

            if (me.Button == MouseButtons.Left && Cell.BackColor != MineC) {
                int X = Cell.Left / Cell.Width;
                int Y = Cell.Top / Cell.Height;

                if (!Started) { SetMines(new Point(X, Y)); Started = true; }

                if (Cell.Name == CellType.Mine.ToString()) {
                    DialogResult DR = MessageBox.Show("Would you like to try again?",
                                    "You Lost!",
                                    MessageBoxButtons.YesNo);

                    if (DR == DialogResult.Yes) Create(Size.Width, Size.Height);
                    else foreach (Label C in Cells) C.Enabled = false;
                }
                else FloodReveal(X, Y);

                if (Won()) MessageBox.Show("YOU WON");
                
            }
        }

        /// <summary>
        /// Click for quick responses
        /// </summary>
        private void Cell_MouseDown(object sender, MouseEventArgs e) {
            Label Cell = (Label)sender;
            if (e.Button == MouseButtons.Right && Cell.Name != CellType.FCell.ToString()) {
                Cell.BackColor = (Cell.BackColor == CellC) ? MineC : CellC;
            }
        }
        #endregion

        private Control Parent;
        private Cell[,] Cells;

        private bool Started;

        private int Mines;
        private Size Size;

        public Color FCellC = Color.LightGray;
        public Color CellC = Color.Gray;
        public Color MineC = Color.Red;
        
        /// <summary>
        /// Creates the field
        /// </summary>
        /// Create a new field with given size
        internal void Create(int X = 9, int Y = 9) {
            #region Initialize
            Parent.Controls.Clear();
            

            this.Started = false;
            this.Size = new Size(X, Y);
            this.Mines = X * Y / 9;

            Cells = new Cell[X, Y];
            #endregion

            for (int y = 0; y < Y; y++) {
                for (int x = 0; x < X; x++) {
                    Cells[x, y] = new Cell();
                    Cells[x, y].Size = new Size(Parent.ClientSize.Width / X, Parent.ClientSize.Height / Y);
                    Cells[x, y].Location = new Point(x * Cells[x, y].Width, y * Cells[x, y].Height);
                    Cells[x, y].Font = new Font("Arial", Cells[x, y].Width / 2);
                    Cells[x, y].BorderStyle = BorderStyle.FixedSingle;
                    Cells[x, y].TextAlign = ContentAlignment.MiddleCenter;
                    Cells[x, y].BackColor = CellC;
                    Cells[x, y].Name = CellType.Cell.ToString();
                    Cells[x, y].Click += Cell_Click;
                    Cells[x, y].MouseDown += Cell_MouseDown;
                    Parent.Controls.Add(Cells[x, y]);
                }
            }
        }

        internal void Resize() {
            for (int y = 0; y < Size.Height; y++) {
                for (int x = 0; x < Size.Width; x++) {
                    Cells[x, y] = new Cell();
                    Cells[x, y].Size = new Size(Parent.ClientSize.Width / Size.Width, Parent.ClientSize.Height / Size.Height);
                    Cells[x, y].Location = new Point(x * Cells[x, y].Width, y * Cells[x, y].Height);
                }
            }
        }

        /// <summary>
        /// Places mines over field
        /// </summary>
        /// While I've got remaining mines
        /// Generate random Coordinates
        /// If there's not already a mine at coordinate
        /// Make it a mine
        /// Mine it's minecount = 0
        private void SetMines(Point ClickC) {
            int _Mines = Mines;
            while (_Mines != 0) {
                int x = RND.Next(Size.Width);
                int y = RND.Next(Size.Height);

                if (!CFunctions.AroundClick(ClickC, new Point(x, y))) {
                    if (Cells[x, y].Name != CellType.Mine.ToString()) {
                        Cells[x, y].Name = CellType.Mine.ToString();
                        Cells[x, y].MineCount = 0;
                        SetNumbers(x, y);

                        _Mines--;
                    }
                }
            }
        }

        /// <summary>
        /// Increases the minecount of surrounding cells
        /// if it's not a mine
        /// </summary>
        /// Go through all 9 positions centered around current mine
        /// If it's not a mine, add to it
        private void SetNumbers(int x, int y) {
            for (int Y = y - 1; Y < y + 2; Y++) {
                for (int X = x - 1; X < x + 2; X++) {
                    try {
                        if (Cells[X, Y].Name != CellType.Mine.ToString()) Cells[X, Y].MineCount++;
                    }
                    catch (Exception) { }
                }
            }
        }

        /// <summary>
        /// Reveal the numbers around clicked cell
        /// </summary>
        /// By making use of floodfill:
        /// If X or Y is on border, return
        /// Else if it's a Cell, reveal it
        /// If it's MineC is not zero, display it's number
        /// Else, keep floodfilling
        private void FloodReveal(int x, int y) {
            if ((x < 0) || (x >= Size.Width)) return;
            else if ((y < 0) || (y >= Size.Height)) return;
            else if (Cells[x, y].Name == CellType.Cell.ToString() && Cells[x, y].BackColor != MineC) {
                Cells[x, y].BackColor = FCellC;
                Cells[x, y].BorderStyle = BorderStyle.None;
                Cells[x, y].Name = CellType.FCell.ToString();

                if (Cells[x, y].MineCount != 0) {
                    int I = Cells[x, y].MineCount;
                    Cells[x, y].Text = I.ToString();
                }
                else {
                    FloodReveal(x, y + 1);
                    FloodReveal(x, y - 1);
                    FloodReveal(x + 1, y);
                    FloodReveal(x - 1, y);
                    FloodReveal(x + 1, y + 1);
                    FloodReveal(x + 1, y - 1);
                    FloodReveal(x - 1, y + 1);
                    FloodReveal(x - 1, y - 1);
                }
            }
        }

        /// <summary>
        /// Checks whether the game has been won or not
        /// </summary>
        /// Search for as long the amount of mines found does not
        /// exceed mines placed
        /// 
        private bool Won() {
            int MCount = 0;
            foreach (Label Cell in Cells) {
                if (MCount <= Mines) {
                    if (Cell.BackColor == MineC) MCount++;
                    else if (Cell.Name == CellType.Cell.ToString()) return false;
                } else return false;
            }

            return true;
        }
    }
}