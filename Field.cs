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

    internal class Field : Panel {
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
                    Disable();
                    Playing = false;
                    DialogResult DR = MessageBox.Show("Would you like to try again?",
                                                      "You lost!",
                                                      MessageBoxButtons.YesNo);
                    if (DR == DialogResult.Yes) Create(Mines, FSize.Width, FSize.Height);
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
        internal void Create(int mines = 0, int x = 9, int y = 9) {
            Initialize(mines, x, y);
            Graphics G = Parent.CreateGraphics();
            G.Clear(Parent.BackColor);

            int Z = Math.Min(Parent.ClientSize.Width / FSize.Width, Parent.ClientSize.Height / FSize.Height);
            Size = new Size(FSize.Width * Z + 10, FSize.Height * Z + 10);
            Location = new Point((Parent.ClientSize.Width - Width) / 2,
                                 (Parent.ClientSize.Height - Height) / 2);


            for (int Y = 0; Y < y; Y++) {
                for (int X = 0; X < x; X++) {
                    Cells[X, Y] = new Cell();
                    Cells[X, Y].Parent = this;
                    Cells[X, Y].Size = new Size(10, 10);
                    Cells[X, Y].Location = new Point(X * Cells[X, Y].Size.Width + 1 * X, Y * Cells[X, Y].Size.Height + 1 * Y);
                    Cells[X, Y].BackColor = Color.Gray;
                    Cells[X, Y].ForeColor = Color.DarkGreen;
                    Cells[X, Y].Font = new Font("Arial", (int)Math.Ceiling((double)Cells[X, Y].Size.Width / 2));
                    Cells[X, Y].Name = CellType.Cell.ToString();
                    
                    //Cells[X, Y].BorderStyle = BorderStyle.FixedSingle;
                    //Cells[X, Y].Click += Cell_Click;
                    //Cells[X, Y].MouseDown += Cell_MouseDown;
                }
            }

            int FontSize = 1;
            for (int Y = 0; Y < FSize.Height; Y++) {
                for (int X = 0; X < FSize.Width; X++) {
                    Cells[X, Y].Size = new Size(Z, Z);

                    if (X == 0) {
                        FontSize = (int)Math.Ceiling((decimal)Cells[0, 0].Size.Width / 2);
                        if (FontSize == 0) FontSize = 1;
                    }

                    Cells[X, Y].Location = new Point(X * Cells[X, Y].Size.Width, Y * Cells[X, Y].Size.Height);
                    Cells[X, Y].Font = new Font("Arial", FontSize);
                    Cells[X, Y].Draw();
                }
            }
            Console.WriteLine("");
        }

        /// <summary>
        /// Resets all parameters of game
        /// Also sets mines if wrong amount of mines is selected
        /// </summary>
        private void Initialize(int mines, int X, int Y) {
            if (Playing) {
                DialogResult DR = MessageBox.Show("You will lose all progress in this game!",
                                              "You've already started a game!",
                                               MessageBoxButtons.YesNo);
                if (DR == DialogResult.No) return;
            }
            if (Mines > X * Y - 9) { // Prevention of making more mines than cells
                MessageBox.Show("So we did some magic and decided for you :)", "You tried to place too many mines!");
                Mines = X * Y / 8;
            } else Mines = mines;

            Controls.Clear();

            FSize = new Size(X, Y);
            Cells = new Cell[X, Y];

            Playing = false;
            _PRight = true;
        }

        /// <summary>
        /// Calls the real create function from a difficulty
        /// </summary>
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

        public Color FCellC = Color.DarkGray;
        public Color CellC = Color.Gray;
        private Color MineC = Color.IndianRed;

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
                        FontSize = (int)Math.Ceiling((decimal)Cells[0, 0].Size.Width / 2);
                        if (FontSize == 0) FontSize = 1;
                    }

                    Cells[x, y].Location = new Point(x * Cells[x, y].Size.Width, y * Cells[x, y].Size.Height);
                    Cells[x, y].Font = new Font("Arial", FontSize);

                    Cells[x, y].Draw();
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
                        Cells[x, y].SMine = 0;
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
                        if (Cells[X, Y].Name != CellType.Mine.ToString()) Cells[X, Y].SMine++;
                    }
                    catch (Exception) { }
                }
            }

            SetColors();
        }

        Color[] clrs = new Color[] {
            Color.Blue,
            Color.Green,
            Color.Red,
            Color.DarkBlue,
            Color.Brown,
            Color.DarkCyan,
            Color.Black,
            Color.DarkSlateGray
        };

        private void SetColors() {
            foreach (Cell C in Cells) {
                if (C.SMine != 0) C.ForeColor = clrs[C.SMine - 1];
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
                //Cells[x, y].BorderStyle = BorderStyle.None;
                Cells[x, y].Name = CellType.FCell.ToString();

                if (Cells[x, y].SMine != 0) {
                    //int I = Cells[x, y].SMine;
                    //Cells[x, y].Text = I.ToString();
                }
                else {
                    for (int Y = y - 1; Y < y + 2; Y++) {
                        for (int X = x - 1; X < x + 2; X++) {
                            FloodReveal(X, Y);
                        }
                    }
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
            foreach (Cell Cell in Cells) {
                if (MCount <= Mines) {
                    if (Cell.BackColor == MineC) MCount++;
                    else if (Cell.Name == CellType.Cell.ToString()) return false;
                }
                else return false;
            }

            Disable();

            Playing = false;
            DialogResult DR = MessageBox.Show("Would you like to try again?",
                                    "You won!",
                                    MessageBoxButtons.YesNo);

            if (DR == DialogResult.Yes) Create(Mines, FSize.Width, FSize.Height);
            return true;
        }

        /// <summary>
        /// Disables the entire field and displays it by 
        /// setting the colors faded
        /// also revokes all eventhandlers
        /// </summary>
        public void Disable() {
            _PRight = false;

            foreach (Cell C in Cells) {
                int[] clr = new int[] { C.ForeColor.R - 45,
                                C.ForeColor.G - 45,
                                C.ForeColor.B - 45
                    };
                for (int i = 0; i < clr.Length; i++) if (clr[i] < 0) clr[i] = 0;
                C.ForeColor = Color.FromArgb(clr[0], clr[1], clr[2]);

                //C.Click -= Cell_Click;
                //C.MouseDown -= Cell_MouseDown;
            }
        }

        /// <summary>
        /// If you've got the right to pause/unpause
        /// it will toggle between pause and unpaused
        /// displays it by fading colors
        /// </summary>
        bool _PRight = true;
        bool _Pause = false;
        public void Pause() {
            if (_PRight) {
                _Pause = !_Pause;

                foreach (Cell C in Cells) {
                    if (_Pause == true) {
                        int[] clr = new int[] { C.ForeColor.R - 45,
                                C.ForeColor.G - 45,
                                C.ForeColor.B - 45
                        };
                        for (int i = 0; i < clr.Length; i++) if (clr[i] < 0) clr[i] = 0;
                        C.ForeColor = Color.FromArgb(clr[0], clr[1], clr[2]);

                        //C.Click -= Cell_Click;
                        //C.MouseDown -= Cell_MouseDown;
                    }
                    else {
                        int[] clr = new int[] {
                        C.ForeColor.R + 45,
                        C.ForeColor.G + 45,
                        C.ForeColor.B + 45
                        };
                        for (int i = 0; i < clr.Length; i++) if (clr[i] > 255) clr[i] = 255;
                        C.ForeColor = Color.FromArgb(clr[0], clr[1], clr[2]);

                        //C.Click += Cell_Click;
                        //C.MouseDown += Cell_MouseDown;
                    }
                }
            }
        }
    }
}