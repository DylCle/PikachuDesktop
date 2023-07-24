using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;


namespace PikachuDesktop
{
    public partial class Form2 : Form
    {
        private Point lastCursorPosition;
        private bool isDragging;
        private Point offset;
        private name myName = new name();
        private PictureBox berryDrag;
        public Form2()
        {
            InitializeComponent();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            myName.Name = txtName.Text;
            lblName.Text = myName.Name;

            Properties.Settings.Default.Name = myName.Name;
            Properties.Settings.Default.Save();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            myName.Name = Properties.Settings.Default.Name;
            lblName.Text = myName.Name;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            lastCursorPosition = e.Location;

            berryDrag = new PictureBox
            {
                Image = pictureBox1.Image,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size =  new Size(103, 93),
                Location = pictureBox1.Location,
                Cursor = Cursors.Hand
            };
            this.Controls.Add(berryDrag);
           berryDrag.BringToFront();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                int dx = e.X - lastCursorPosition.X;
                int dy = e.Y - lastCursorPosition.Y;
                this.Location = new Point(this.Left + dx, this.Top + dy);
                berryDrag.Left += dx;
                berryDrag.Top += dy;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if(isDragging)
            {
                isDragging = false;
                this.Controls.Remove(berryDrag);
                berryDrag.Dispose();    
            }  
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (isDragging)
            {
                isDragging = false;
                this.Controls.Remove(berryDrag);
                berryDrag.Dispose();
            }
        }
    }


}
