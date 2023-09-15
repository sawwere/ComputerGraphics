using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab2
{
    public partial class Form1 : Form
    {
        public Image _image;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var form1 = new Task1Form();
            this.AddOwnedForm(form1);
            //form1.Owner = this;
            this.Hide();
            form1.Closed += (s, args) => this.Show();
            form1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var form2 = new Task2Form();
            this.AddOwnedForm(form2);
            this.Hide();
            form2.Closed += (s, args) => this.Show();
            form2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var form3 = new Task3Form();
            this.AddOwnedForm(form3);
            this.Hide();
            form3.Closed += (s, args) => this.Show();
            form3.Show();
        }

        private void buttonLoadFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog1.FileName;
            pictureBox1.Image = new Bitmap(filename);
            _image = pictureBox1.Image;
            label1.Text = filename;
        }
    }
}
