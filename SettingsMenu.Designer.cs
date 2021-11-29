
namespace Minesweeper {
    partial class SettingsMenu {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.Easy = new System.Windows.Forms.Label();
            this.Medium = new System.Windows.Forms.Label();
            this.Hard = new System.Windows.Forms.Label();
            this.Extreme = new System.Windows.Forms.Label();
            this.Cancel = new System.Windows.Forms.Label();
            this.Accept = new System.Windows.Forms.Label();
            this.BParent = new System.Windows.Forms.Panel();
            this.BParent.SuspendLayout();
            this.SuspendLayout();
            // 
            // Easy
            // 
            this.Easy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.Easy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Easy.Dock = System.Windows.Forms.DockStyle.Top;
            this.Easy.Enabled = false;
            this.Easy.Font = new System.Drawing.Font("Consolas", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Easy.ForeColor = System.Drawing.Color.Silver;
            this.Easy.Location = new System.Drawing.Point(0, 0);
            this.Easy.Name = "Easy";
            this.Easy.Size = new System.Drawing.Size(420, 122);
            this.Easy.TabIndex = 0;
            this.Easy.Text = "Easy\r\n9x9 10 mines";
            this.Easy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Easy.Click += new System.EventHandler(this.SetDifficulty);
            // 
            // Medium
            // 
            this.Medium.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.Medium.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Medium.Dock = System.Windows.Forms.DockStyle.Top;
            this.Medium.Font = new System.Drawing.Font("Consolas", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Medium.ForeColor = System.Drawing.Color.Silver;
            this.Medium.Location = new System.Drawing.Point(0, 122);
            this.Medium.Name = "Medium";
            this.Medium.Size = new System.Drawing.Size(420, 112);
            this.Medium.TabIndex = 1;
            this.Medium.Text = "Medium\r\n16x16 40 mines";
            this.Medium.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Medium.Click += new System.EventHandler(this.SetDifficulty);
            // 
            // Hard
            // 
            this.Hard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.Hard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Hard.Dock = System.Windows.Forms.DockStyle.Top;
            this.Hard.Font = new System.Drawing.Font("Consolas", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Hard.ForeColor = System.Drawing.Color.Silver;
            this.Hard.Location = new System.Drawing.Point(0, 234);
            this.Hard.Name = "Hard";
            this.Hard.Size = new System.Drawing.Size(420, 112);
            this.Hard.TabIndex = 2;
            this.Hard.Text = "Hard\r\n30x16 99 mines";
            this.Hard.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Hard.Click += new System.EventHandler(this.SetDifficulty);
            // 
            // Extreme
            // 
            this.Extreme.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.Extreme.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Extreme.Dock = System.Windows.Forms.DockStyle.Top;
            this.Extreme.Font = new System.Drawing.Font("Consolas", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Extreme.ForeColor = System.Drawing.Color.Silver;
            this.Extreme.Location = new System.Drawing.Point(0, 346);
            this.Extreme.Name = "Extreme";
            this.Extreme.Size = new System.Drawing.Size(420, 112);
            this.Extreme.TabIndex = 3;
            this.Extreme.Text = "Extreme\r\n30x24 180 mines";
            this.Extreme.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Extreme.Click += new System.EventHandler(this.SetDifficulty);
            // 
            // Cancel
            // 
            this.Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.Cancel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Cancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.Cancel.Font = new System.Drawing.Font("Consolas", 36F, System.Drawing.FontStyle.Bold);
            this.Cancel.ForeColor = System.Drawing.Color.Silver;
            this.Cancel.Location = new System.Drawing.Point(0, 458);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(210, 142);
            this.Cancel.TabIndex = 4;
            this.Cancel.Text = "Cancel";
            this.Cancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Accept
            // 
            this.Accept.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.Accept.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Accept.Dock = System.Windows.Forms.DockStyle.Right;
            this.Accept.Font = new System.Drawing.Font("Consolas", 36F, System.Drawing.FontStyle.Bold);
            this.Accept.ForeColor = System.Drawing.Color.Silver;
            this.Accept.Location = new System.Drawing.Point(210, 458);
            this.Accept.Name = "Accept";
            this.Accept.Size = new System.Drawing.Size(210, 142);
            this.Accept.TabIndex = 5;
            this.Accept.Text = "Accept";
            this.Accept.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Accept.Click += new System.EventHandler(this.Accept_Click);
            // 
            // BParent
            // 
            this.BParent.AutoSize = true;
            this.BParent.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BParent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.BParent.Controls.Add(this.Extreme);
            this.BParent.Controls.Add(this.Hard);
            this.BParent.Controls.Add(this.Medium);
            this.BParent.Controls.Add(this.Easy);
            this.BParent.Dock = System.Windows.Forms.DockStyle.Top;
            this.BParent.ForeColor = System.Drawing.Color.Silver;
            this.BParent.Location = new System.Drawing.Point(0, 0);
            this.BParent.Name = "BParent";
            this.BParent.Size = new System.Drawing.Size(420, 458);
            this.BParent.TabIndex = 6;
            // 
            // SettingsMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.Accept);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.BParent);
            this.Name = "SettingsMenu";
            this.Size = new System.Drawing.Size(420, 600);
            this.VisibleChanged += new System.EventHandler(this.SettingsMenu_VisibleChanged);
            this.BParent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Easy;
        private System.Windows.Forms.Label Medium;
        private System.Windows.Forms.Label Hard;
        private System.Windows.Forms.Label Extreme;
        private System.Windows.Forms.Label Cancel;
        private System.Windows.Forms.Label Accept;
        private System.Windows.Forms.Panel BParent;
    }
}
