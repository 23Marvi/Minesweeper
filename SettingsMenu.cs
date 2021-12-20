using System;
using System.Drawing;
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

            foreach (Label L in LParent.Controls) L.Enabled = true;

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

            foreach (Label L in LParent.Controls) L.Enabled = true;
            SLabel.Enabled = false;

            if (!Visible && Parent != null) {
                Form1 F = (Form1)Parent;
                F.Field.Redraw(Bounds);
            }
        }

        // Ability to toggle the visibility instead of setting it
        public void ToggleVisible(int v = 1) {
            Center();

            for (int i = 0; i < v; i++) {
                Visible = !Visible;
            }
        }

        // Centers the Control compared to Parent
        public void Center() {
            Resize();

            if (Parent != null) {
                Location = new Point((Parent.ClientSize.Width - Width) / 2,
                                 (Parent.ClientSize.Height - Height) / 2);

            }
        }

        /// <summary>
        /// On resize
        /// </summary>
        /// Determine new Width, which is the smallest side divided by three 3
        /// Height is that + '50%
        public void Resize() {
            Width = Math.Min(Parent.ClientSize.Width, Parent.ClientSize.Height) / 3;
            Height = Width + Width / 2; ;

            float S = Width / 15;
            LParent.Font = BParent.Font = new Font("Consolas", S, FontStyle.Bold);


            foreach (Control c in LParent.Controls) {
                c.Height = (Height - Height / 4) / 4;
            }
            foreach (Control c in BParent.Controls) {
                c.Height = (Height - LParent.Height) / 2;
            }
        }
    } 
}
