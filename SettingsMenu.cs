using System;
using System.Windows.Forms;

namespace Minesweeper {
    public partial class SettingsMenu : UserControl {
        public SettingsMenu() {
            InitializeComponent();
            SLabel = Label = Easy;
        }

        public string Difficulty = "Easy";
        private string SDifficulty = "Easy";
        private Label Label;
        private Label SLabel;

        /// <summary>
        /// Sets Difficulty depending on which button is pressed
        /// Changes the visual state
        /// </summary>
        private void SetDifficulty(object sender, EventArgs e) {
            Label Sender = (Label)sender;

            foreach (Label L in BParent.Controls) L.Enabled = true;

            Difficulty = Sender.Name;
            Label = Sender;

            Sender.Enabled = false;
        }
        /// <summary>
        /// Creates a new Field with current Difficulty
        /// </summary>
        private void Accept_Click(object sender, EventArgs e) {
            SDifficulty = Difficulty;
            SLabel = Label;


            Visible = false;

            Form1 F = (Form1)Parent;
            F.CreateField();
        }
        /// <summary>
        /// Closes the SettingsMenu
        /// Reverts Difficulty state to what it was
        /// </summary>
        private void Cancel_Click(object sender, EventArgs e) {
            Visible = false;
        }

        /// <summary>
        /// Every time the SettingsMenu it is centered
        /// </summary>
        private void SettingsMenu_VisibleChanged(object sender, EventArgs e) {
            Difficulty = SDifficulty;

            foreach (Label L in BParent.Controls) L.Enabled = true;
            SLabel.Enabled = false;
        }
    } 
}
