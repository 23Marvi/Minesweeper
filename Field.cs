using System;
using System.Collections.Generic;
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
        private void Field_Click(object sender, EventArgs e) {
            MouseEventArgs E = (MouseEventArgs)e;
            int X = E.Location.X / (Cells[0, 0].Size.Width + 1);
            int Y = E.Location.Y / (Cells[0, 0].Size.Height + 1);
            Cell Cell = Cells[X, Y];


            if (E.Button == MouseButtons.Left && Cell.BackColor != MineC) {
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
        private void Field_MouseDown(object sender, MouseEventArgs e) {
            MouseEventArgs E = (MouseEventArgs)e;
            int X = E.Location.X / (Cells[0, 0].Size.Width + 1);
            int Y = E.Location.Y / (Cells[0, 0].Size.Height + 1);
            Cell Cell = Cells[X, Y];

            if (e.Button == MouseButtons.Right && Cell.Name != CellType.FCell.ToString()) {
                Cell.BackColor = (Cell.BackColor == CellC) ? MineC : CellC;
                Cell.Update();
            }
        }
        
        #endregion
        #region Constructors
        internal void Create(int mines = 0, int x = 9, int y = 9) {
            Initialize(mines, x, y);


            int Z = Math.Min(Parent.ClientSize.Width / FSize.Width, Parent.ClientSize.Height / FSize.Height);
            Size = new Size(FSize.Width * Z + 10, FSize.Height * Z + 10);
            Location = new Point((Parent.ClientSize.Width - Width) / 2,
                                 (Parent.ClientSize.Height - Height) / 2);


            for (int Y = 0; Y < y; Y++) {
                for (int X = 0; X < x; X++) {
                    Cells[X, Y] = new Cell();
                    Cells[X, Y].Parent = this;
                    Cells[X, Y].Border = true;
                    Cells[X, Y].BackColor = Color.Gray;
                    Cells[X, Y].ForeColor = Color.DarkGreen;
                    Cells[X, Y].Name = CellType.Cell.ToString();
                }
            }

            Resize();
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
            if (mines > X * Y - 9 || mines == 0) { // Prevention of making more mines than cells
                MessageBox.Show("So we did some magic and decided for you :)", "You tried to place too many mines!");
                Mines = X * Y / 8;
            } else Mines = mines;

            FSize = new Size(X, Y);
            Cells = new Cell[X, Y];

            Click += Field_Click;
            MouseDown += Field_MouseDown;

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
            Graphics G = CreateGraphics();
            G.Clear(Parent.BackColor);

            int Z = Math.Min(Parent.ClientSize.Width / FSize.Width, Parent.ClientSize.Height / FSize.Height);
            Size = new Size(FSize.Width * Z, FSize.Height * Z);
            Location = new Point((Parent.ClientSize.Width - Width) / 2,
                                 (Parent.ClientSize.Height - Height) / 2);

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
                    Cells[X, Y].Update();
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
            List<Point> Placed = new List<Point>();
            int _Mines = Mines;
            while (_Mines != 0) {
                int x = RND.Next(FSize.Width);
                int y = RND.Next(FSize.Height);

                if (!Placed.Contains(new Point(x, y))) {
                    if (!CFunctions.AroundClick(ClickC, new Point(x, y))) {
                        if (Cells[x, y].Name != CellType.Mine.ToString()) {
                            Cells[x, y].Name = CellType.Mine.ToString();
                            Cells[x, y].SMine = 0;
                            SetNumbers(x, y);

                            _Mines--;
                            Placed.Add(new Point(x, y));
                        }
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
                Cells[x, y].Border = false;
                Cells[x, y].Name = CellType.FCell.ToString();
                Cells[x, y].ShowNum = true;
                Cells[x, y].Update();

                if (Cells[x, y].SMine == 0) {
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

            Click -= Field_Click;
            MouseDown -= Field_MouseDown;

            foreach (Cell C in Cells) {
                int[] clr = new int[] { C.ForeColor.R - 45,
                                C.ForeColor.G - 45,
                                C.ForeColor.B - 45
                    };
                for (int i = 0; i < clr.Length; i++) if (clr[i] < 0) clr[i] = 0;
                C.ForeColor = Color.FromArgb(clr[0], clr[1], clr[2]);
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

                if (_Pause == true) {
                    Click -= Field_Click;
                    MouseDown -= Field_MouseDown;

                    foreach (Cell C in Cells) {
                        if (C.BackColor == FCellC) {
                            int[] clr = new int[] { C.ForeColor.R - 45,
                                C.ForeColor.G - 45,
                                C.ForeColor.B - 45
                                };
                            for (int i = 0; i < clr.Length; i++) if (clr[i] < 0) clr[i] = 0;
                            C.ForeColor = Color.FromArgb(clr[0], clr[1], clr[2]);

                            C.Update();
                        }
                    }
                }
                else {
                    Click += Field_Click;
                    MouseDown += Field_MouseDown;

                    foreach (Cell C in Cells) {
                        if (C.BackColor == FCellC) {
                            int[] clr = new int[] {
                                C.ForeColor.R + 45,
                                C.ForeColor.G + 45,
                                C.ForeColor.B + 45
                            };
                            for (int i = 0; i < clr.Length; i++) if (clr[i] > 255) clr[i] = 255;
                            C.ForeColor = Color.FromArgb(clr[0], clr[1], clr[2]);

                            C.Update();
                        }
                    }
                }
            }
        }
    }
}