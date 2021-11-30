using System;
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

        Field Field;

        private void Form1_Load(object sender, EventArgs e) {
            #region StartMessage
            StringBuilder SB = new StringBuilder();
            SB.Append("Press 'H' for a explanation on how Minesweeper works.");
            SB.AppendLine();
            SB.Append("Press 'R' to start a new game with same difficulty");
            SB.AppendLine();
            SB.Append("Press 'Esc' to choose a different difficulty.");
            SB.AppendLine();
            MessageBox.Show(SB.ToString(), "Welcome to Minesweeper!");
            #endregion

            int MaxSize = Math.Min(ClientSize.Width, ClientSize.Height);
            FParent.Size = new Size(MaxSize, MaxSize);
            FParent.Left = (ClientSize.Width - FParent.Width) / 2;
            
            Field = new Field(FParent);
            CreateField();
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
            int MaxSize = Math.Min(ClientSize.Width, ClientSize.Height);
            FParent.Size = new Size(MaxSize, MaxSize);
            FParent.Left = (ClientSize.Width - FParent.Width) / 2;
            FParent.Top = (ClientSize.Height - FParent.Height) / 2;
            Field.FResize();
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
        }
    }
}
