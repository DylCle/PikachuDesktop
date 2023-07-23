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

        private name myName = new name();
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
    }


}
