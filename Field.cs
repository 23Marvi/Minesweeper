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

    public class Field : Panel {
        public Field() {
            MouseWheel += Field_MouseWheel;
        }

        int XOff = 0;
        int YOff = 0;

        private void Field_MouseWheel(object sender, MouseEventArgs e) {
            if (!_Pause) {
                int PSize = Cells[0, 0].Size.Width;
                if (e.Delta >= 0) PSize++;
                if (e.Delta <= 0) {
                    if (CArrSize.X * PSize != Width) PSize--;
                    else return;
                }

                Graphics G = CreateGraphics();
                G.Clear(BackColor);

                foreach (Cell c in Cells) c.Size = new Size(PSize, PSize);

                if (e.Delta >= 0) {
                    if (e.X <= Size.Width / 2) {
                        if (e.Y > Size.Height / 2) YOff -= CArrSize.Y;
                    }
                    if (e.X > Size.Width / 2) {
                        if (e.Y > Size.Height / 2) YOff -= CArrSize.Y;
                        XOff -= CArrSize.X;
                    }
                }
                if (e.Delta <= 0) {
                    if (XOff != 0) XOff += CArrSize.X;
                    if (YOff != 0) YOff += CArrSize.Y;
                }

                for (int y = 0; y < CArrSize.Y; y++) {
                    for (int x = 0; x < CArrSize.X; x++) {
                        Cell C = Cells[x, y];

                        C.Location = new Point(C.Size.Width * x + XOff, C.Size.Height * y + YOff);

                        if (C.Location.X < Width && C.Location.X + C.Size.Width > 0) {
                            if (C.Location.Y < Height && C.Location.Y + C.Size.Height > 0) {
                                Cells[x, y].Update();
                            }
                        }
                    }
                }
            }
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
        private void Field_Click(object sender, EventArgs e) {
            MouseEventArgs E = (MouseEventArgs)e;
            Point P = PointToClient(Cursor.Position);
            int X = (P.X - Cells[0, 0].Location.X) / Cells[0, 0].Size.Width;
            int Y = (P.Y - Cells[0, 0].Location.Y) / Cells[0, 0].Size.Height;
            Cell Cell = Cells[X, Y];

            if (E.Button == MouseButtons.Left && Cell.BackColor != MineC) {
                if (!Playing) { SetMines(new Point(X, Y)); Playing = true; }

                if (Cell.Type == CellType.Mine) {
                    Disable();
                    Playing = false;
                    DialogResult DR = MessageBox.Show("Would you like to try again?",
                                                        "You lost!",
                                                        MessageBoxButtons.YesNo);
                    if (DR == DialogResult.Yes) Create(Mines, CArrSize.X, CArrSize.Y);
                    return;
                }
                else {
                    FloodReveal(X, Y);
                    Won();
                }
            }
        }

        /// Fast click for flagging ///
        private void Field_MouseDown(object sender, MouseEventArgs e) {
            Point P = PointToClient(Cursor.Position);
            int X = (P.X - Cells[0, 0].Location.X) / Cells[0, 0].Size.Width;
            int Y = (P.Y - Cells[0, 0].Location.Y) / Cells[0, 0].Size.Height;
            Cell Cell = Cells[X, Y];

            if (e.Button == MouseButtons.Right) {
                if (Cell.Type != CellType.FCell) {
                    Cell.BackColor = (Cell.BackColor == CellC) ? MineC : CellC;
                    Cell.Update();
                }
            }
        }

        #endregion
        #region Constructors
        /// Creates a Field with the properties it already has ///
        internal void Create() {
            Create(Mines, CArrSize.X, CArrSize.Y);
        }

        /// <summary>
        /// Creates a new field with a size and Mines
        /// </summary>
        /// First look if all the data is the same, if so, reuse old field
        /// Otherwise create a whole new field
        internal void Create(int mines, int x, int y) {
            Initialize();

            if (mines == Mines && x == CArrSize.X && y == CArrSize.Y) {
                for (int Y = 0; Y < y; Y++) {
                    for (int X = 0; X < x; X++) {
                        bool Change = false;
                        if (Cells[X, Y].BackColor != CellC) {
                            Cells[X, Y].BackColor = Parent.BackColor;
                            Cells[X, Y].ForeColor = Parent.BackColor;
                            Cells[X, Y].Update();

                            Change = true;
                        }

                        Cells[X, Y].BackColor = CellC;
                        Cells[X, Y].Type = CellType.Cell;
                        Cells[X, Y].SMine = 0;
                        Cells[X, Y].ShowNum = false;
                        Cells[X, Y].Border = true;

                        if (Change) Cells[X, Y].Update();
                    }
                }
            }
            else {
                UpdateProps(mines, x, y);
                for (int Y = 0; Y < y; Y++) {
                    for (int X = 0; X < x; X++) {
                        Cells[X, Y] = new Cell();
                        Cells[X, Y].Parent = this;
                        Cells[X, Y].BackColor = CellC;
                        Cells[X, Y].Type = CellType.Cell;
                    }
                }

                int Z = Math.Min(Parent.ClientSize.Width / CArrSize.X, Parent.ClientSize.Height / CArrSize.Y);
                Size = new Size(CArrSize.X * Z, CArrSize.Y * Z);
                Location = new Point((Parent.ClientSize.Width - Width) / 2,
                                     (Parent.ClientSize.Height - Height) / 2);
                Resize();
            }
        }

        /// <summary>
        /// Resets all parameters of game
        /// Also sets mines if wrong amount of mines is selected
        /// </summary>
        private void UpdateProps(int mines, int X, int Y) {
            if (mines > X * Y - 9) {
                MessageBox.Show("So we did some magic and decided for you :)", "You tried to place too many mines!");
                Mines = X * Y / 8;
            }
            else if (mines < X * Y / 100 * 10) {
                MessageBox.Show("So we did some magic and decided for you :)", "You tried to place underneath the minimum amount of mines!");
                Mines = X * Y / 8;
            } else Mines = mines;

            CArrSize = new Point(X, Y);
            Cells = new Cell[X, Y];
        }

        /// <summary>
        /// Resets all variables used in the game
        /// </summary>
        private void Initialize() {
            if (Playing) {
                DialogResult DR = MessageBox.Show("You will lose all progress in this game!",
                                              "You've already started a game!",
                                               MessageBoxButtons.YesNo);
                if (DR == DialogResult.No) return;
            }

            Click -= Field_Click;
            MouseDown -= Field_MouseDown;
            Click += Field_Click;
            MouseDown += Field_MouseDown;

            Playing = false;
            _PRight = true;
            _Pause = false;

            XOff = YOff = 0;
        }

        /// <summary>
        /// Calls the real create function from a difficulty
        /// </summary>
        internal void Create(Difficulty D, bool Flipped = false) {
            if (D == Difficulty.Easy) Create(10, 9, 9);
            else if (D == Difficulty.Medium) Create(40, 16, 16);
            else if (D == Difficulty.Hard) {
                if (!Flipped) Create(99, 30, 16);
                else Create(99, 16, 30);
            }
            else if (D == Difficulty.Extreme) {
                if (!Flipped) Create(180, 30, 24);
                else Create(180, 24, 30);
            }
        }
        #endregion

        public Cell[,] Cells;
        private bool Playing;

        private int Mines;
        public Point CArrSize;

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

            int Z = Math.Min(Parent.ClientSize.Width / CArrSize.X, Parent.ClientSize.Height / CArrSize.Y);
            Size = new Size(CArrSize.X * Z, CArrSize.Y * Z);
            Location = new Point((Parent.ClientSize.Width - Width) / 2,
                                 (Parent.ClientSize.Height - Height) / 2);

            for (int Y = 0; Y < CArrSize.Y; Y++) {
                for (int X = 0; X < CArrSize.X; X++) {
                    Cells[X, Y].Size = new Size(Z, Z);
                    Cells[X, Y].Location = new Point(X * Cells[X, Y].Size.Width, Y * Cells[X, Y].Size.Height);
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
            int _Mines = Mines;
            while (_Mines != 0) {
                int x = RND.Next(CArrSize.X);
                int y = RND.Next(CArrSize.Y);

                if (!CFunctions.AroundClick(ClickC, new Point(x, y))) {
                    if (Cells[x, y].Type != CellType.Mine) {

                        Cells[x, y].Type = CellType.Mine;
                        Cells[x, y].SMine = 0;
                        SetNumbers(x, y);
                        _Mines--;
                    }
                }
            }

            SetColors();
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
                    if (Y >= 0 && Y <= CArrSize.Y - 1) {
                        if (X >= 0 && X <= CArrSize.X - 1) {
                            if (Cells[X, Y].Type != CellType.Mine) Cells[X, Y].SMine++;
                        }
                    }
                }
            }
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
            if (x < 0 || x >= CArrSize.X || y < 0 || y >= CArrSize.Y) return;

            if (Cells[x, y].Type == CellType.Cell && Cells[x, y].BackColor != MineC) {
                Cells[x, y].BackColor = FCellC;
                Cells[x, y].Border = false;
                Cells[x, y].Type = CellType.FCell;
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
                    else if (Cell.Type == CellType.Cell) return false;
                } else return false;
            }

            Disable();

            Playing = false;
            DialogResult DR = MessageBox.Show("Would you like to try again?",
                                    "You won!",
                                    MessageBoxButtons.YesNo);

            if (DR == DialogResult.Yes) Create();
            return true;
        }

        /// <summary>
        /// Disables the entire field and displays it by 
        /// setting the colors faded
        /// also revokes all eventhandlers
        /// </summary>
        private void Disable() {
            Pause();
            _PRight = false;
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

        /// <summary>
        /// Redraws part of the Cells whenever something "deleted" it
        /// by being on top of it
        /// </summary>
        /// Picks the cells of which their Location is within the Location
        /// of other control
        public void Redraw(Rectangle R) {
            if (Cells != null) {
                for (int y = 0; y < CArrSize.Y; y++) {
                    for (int x = 0; x < CArrSize.X; x++) {
                        Cell i = Cells[x, y];
                        Rectangle C = new Rectangle(new Point(Location.X + i.Location.X, Location.Y + i.Location.Y), i.Size);
                        if (CFunctions.WithinBounds(C, R)) i.Update();
                    }
                }
            }
        }
    }
}