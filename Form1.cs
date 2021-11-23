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
            this.Size = Screen.PrimaryScreen.Bounds.Size;
        }

        Field Field;

        private void Form1_Load(object sender, EventArgs e) {
            int MaxSize = Math.Min(ClientSize.Width, ClientSize.Height);
            FParent.Size = new Size(MaxSize, MaxSize);
            FParent.Left = (ClientSize.Width - FParent.Width) / 2;
            
            Field = new Field(FParent);
            Field.Create(30, 30);
        }

        /// <summary>
        /// Hotkeys for ingame settings
        /// </summary>
        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            if (e.KeyCode == Keys.R) Field.Create(30, 30);
        }

        /// <summary>
        /// Resize the Field once the form has ended resizing
        /// </summary>
        private void Form1_ResizeEnd(object sender, EventArgs e) {
            int MaxSize = Math.Min(ClientSize.Width, ClientSize.Height);
            Parent.Size = new Size(MaxSize, MaxSize);
            Parent.Left = (ClientSize.Width - Parent.Width) / 2;
            Field.Resize();
        }
    }
}
