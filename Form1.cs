﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        Field Field = new Field();

        /// <summary>
        /// On startup
        /// Show message and
        /// draw first field
        /// </summary>
        bool _Ini = false;
        private void Form1_Paint(object sender, PaintEventArgs e) {
            if (_Ini == false) {
                #region StartMessage
                StringBuilder SB = new StringBuilder();
                SB.Append("Press 'H' for a explanation on how Minesweeper works.");
                SB.AppendLine();
                SB.Append("Press 'P' to pause current game.");
                SB.AppendLine();
                SB.Append("Press 'R' to start a new game with same difficulty");
                SB.AppendLine();
                SB.Append("Press 'Esc' to choose a different difficulty.");
                SB.AppendLine();
                MessageBox.Show(SB.ToString(), "Welcome to Minesweeper!");
                #endregion

                Controls.Add(Field);
                Field.Create(0, 80, 80);
                
                //CreateField();
                _Ini = true;
            }
        }

        /// <summary>
        /// Open function to Create a field from Difficulty
        /// </summary>
        public void CreateField() {
            if (Enum.TryParse(SettingsMenu.Difficulty, out Difficulty D)) Field.Create(D);
            else Field.Create();
        }

        /// <summary>
        /// Resize the Field once the form has ended resizing
        /// </summary>
        private void Form1_ResizeEnd(object sender, EventArgs e) {
            Field.Resize();
        }

        /// <summary>
        /// Hotkeys for ingame settings
        /// </summary>
        /// R to Restart game
        /// Esc for Settings
        /// H for help
        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.R) CreateField();
            if (e.KeyCode == Keys.Escape) {
                SettingsMenu.Visible = (SettingsMenu.Visible) ? false : true;
                SettingsMenu.Location = new Point((ClientSize.Width - SettingsMenu.Width) / 2,
                                                  (ClientSize.Height - SettingsMenu.Height) / 2);
            }
            if (e.KeyCode == Keys.H) {
                StringBuilder SB = new StringBuilder();
                SB.AppendLine("Click any cell to start the game, it will display numbers and blank cells.");
                SB.AppendLine();
                SB.AppendLine("The blank cells mean there are no Mines nearby, phew! " +
                          "BUT, about the cells with the numbers in them.. The number represents the amount of mines within it's 8 cell radius");
                SB.AppendLine();
                SB.AppendLine("When you think you know where a mine is located, right click to flag it. Right click again to unflag it. " +
                              "Flagging prevents you from setting a mine off!");
                SB.AppendLine();
                SB.AppendLine("You win by discovering each cell that is not a mine. And ofcourse you lose by clicking a mine.");
                SB.AppendLine();
                SB.AppendLine("Good luck!");
                MessageBox.Show(SB.ToString(), "How does Minesweeper work?");
            }
            if (e.KeyCode == Keys.P) Field.Pause();
        }
    }
}
