
namespace Minesweeper {
    partial class Form1 {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.FParent = new System.Windows.Forms.Panel();
            this.SettingsMenu = new Minesweeper.SettingsMenu();
            this.SuspendLayout();
            // 
            // FParent
            // 
            this.FParent.Location = new System.Drawing.Point(0, 0);
            this.FParent.Margin = new System.Windows.Forms.Padding(1);
            this.FParent.Name = "FParent";
            this.FParent.Size = new System.Drawing.Size(483, 396);
            this.FParent.TabIndex = 0;
            // 
            // SettingsMenu
            // 
            this.SettingsMenu.BackColor = System.Drawing.SystemColors.Control;
            this.SettingsMenu.Location = new System.Drawing.Point(643, 12);
            this.SettingsMenu.Name = "SettingsMenu";
            this.SettingsMenu.Size = new System.Drawing.Size(420, 600);
            this.SettingsMenu.TabIndex = 1;
            this.SettingsMenu.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.ClientSize = new System.Drawing.Size(1075, 882);
            this.Controls.Add(this.SettingsMenu);
            this.Controls.Add(this.FParent);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Form1_PreviewKeyDown);
            this.ResumeLayout(false);

        }
        #endregion
        private SettingsMenu SettingsMenu;
        private System.Windows.Forms.Panel FParent;
    }
}

