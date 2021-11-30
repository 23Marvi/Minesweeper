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
    public enum Difficulty { 
        Easy,
        Medium,
        Hard,
        Extreme
    }

    internal class Field : Panel{
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

                if (!Playing) { SetMines(new Point(X, Y)); Playing = true; }

                if (Cell.Name == CellType.Mine.ToString()) {
                    Playing = false;
                    DialogResult DR = MessageBox.Show("Would you like to try again?",
                                                      "You lost!",
                                                      MessageBoxButtons.YesNo);

                    if (DR == DialogResult.Yes) Create(Mines, FSize.Width, FSize.Height);
                    else foreach (Label C in Cells) C.Enabled = false;
                }
                else {
                    FloodReveal(X, Y);
                    Won();
                }
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
        #region Constructors
        internal void Create(int Mines = 0, int X = 9, int Y = 9) {
            #region Initialize
            if (Playing) {
                DialogResult DR = MessageBox.Show("You will lose all progress in this game!",
                                              "You've already started a game!",
                                               MessageBoxButtons.YesNo);
                if (DR == DialogResult.No) return;
            }
            if (Mines > X * Y - 9) { // Prevention of making more mines than cells
                MessageBox.Show("So we did some magic and decided for you :)", "You tried to place too many mines!");
                this.Mines = X * Y / 8;
            } else this.Mines = Mines;

            Controls.Clear();
            FSize = new Size(X, Y);
            Playing = false;

            Cells = new Cell[X, Y];
            #endregion
            int Z = Math.Min(Parent.ClientSize.Width / X, Parent.ClientSize.Height / Y);
            for (int y = 0; y < Y; y++) {
                for (int x = 0; x < X; x++) {
                    Cells[x, y] = new Cell();
                    Cells[x, y].Location = new Point(x * Cells[x, y].Width, y * Cells[x, y].Height);
                    Cells[x, y].Font = new Font("Arial", Cells[x, y].Width / 2);
                    Cells[x, y].BorderStyle = BorderStyle.FixedSingle;
                    Cells[x, y].TextAlign = ContentAlignment.MiddleCenter;
                    Cells[x, y].BackColor = CellC;
                    Cells[x, y].Name = CellType.Cell.ToString();
                    Cells[x, y].Click += Cell_Click;
                    Cells[x, y].MouseDown += Cell_MouseDown;
                    Controls.Add(Cells[x, y]);
                }
            }

            Resize();
        }
        internal void Create(Difficulty D) {
            if (D == Difficulty.Easy) Create(10, 9, 9);
            else if (D == Difficulty.Medium) Create(40, 16, 16);
            else if (D == Difficulty.Hard) Create(99, 30, 16);
            else if (D == Difficulty.Extreme) Create(180, 30, 24);
        }
        #endregion

        public Cell[,] Cells;

        private bool Playing;

        private int Mines;
        private Size FSize;

        public Color FCellC = Color.LightGray;
        public Color CellC = Color.Gray;
        private Color MineC = Color.Red;

        /// <summary>
        /// Resizes the cells 
        /// Then fits the Panel around it
        /// </summary>
        internal new void Resize() {
            int Z = Math.Min(Parent.ClientSize.Width / FSize.Width, Parent.ClientSize.Height / FSize.Height);
            int FontSize = 1;

            for (int y = 0; y < FSize.Height; y++) {
                for (int x = 0; x < FSize.Width; x++) {
                    Cells[x, y].Size = new Size(Z, Z);

                    if (x == 0) {
                        FontSize = (int)Math.Ceiling((decimal)Cells[0, 0].Width / 2);
                        if (FontSize == 0) FontSize = 1;
                    }

                    Cells[x, y].Location = new Point(x * Cells[x, y].Width, y * Cells[x, y].Height);
                    Cells[x, y].Font = new Font("Arial", FontSize);
                }
            }

            Size = new Size(FSize.Width * Z + 10, FSize.Height * Z + 10);
            Location = new Point((Parent.ClientSize.Width - Width) / 2,
                                 (Parent.ClientSize.Height - Height) / 2);
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
                int x = RND.Next(FSize.Width);
                int y = RND.Next(FSize.Height);

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
            if ((x < 0) || (x >= FSize.Width)) return;
            else if ((y < 0) || (y >= FSize.Height)) return;
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
        /// If won, disable the field
        /// Show Messagebox asking to play again
        private bool Won() {
            int MCount = 0;
            foreach (Label Cell in Cells) {
                if (MCount <= Mines) {
                    if (Cell.BackColor == MineC) MCount++;
                    else if (Cell.Name == CellType.Cell.ToString()) return false;
                } else return false;
            }
            foreach (Label C in Cells) C.Enabled = false;
            Playing = false;
            DialogResult DR = MessageBox.Show("Would you like to try again?",
                                    "You won!",
                                    MessageBoxButtons.YesNo);

            if (DR == DialogResult.Yes) Create(Mines, FSize.Width, FSize.Height);
            else foreach (Label C in Cells) C.Enabled = false;
            return true;
        }
    }
}