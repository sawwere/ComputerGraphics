using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Tools;
using Tools.FastBitmap;

namespace Lab3
{
    public partial class Task1Form : Form
    {

        private Graphics g;
        private Bitmap drawArea;
        private bool Zajata=false;//ЛКМ зажата 
        private bool flag = false;// чтобы рисовать далее (поле заливки цветом)
        private bool flag2 = false;//чтобы рисовать далее (поле заливки картинкой) 
        Point currentPoint;
        Point prevPoint;


        public Task1Form()
        {
            InitializeComponent();
            g = this.CreateGraphics();
            g.Clear(Color.White);
            drawArea = new Bitmap(pictureBox1.Image);
            g = Graphics.FromImage(drawArea);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)//заливка цветом
        {
            flag = !flag;
            if (flag)
                button2.Text = "Рисовать далее";
            else
                button2.Text = "Заливка цветом";
        }

        private void button1_Click(object sender, EventArgs e)//выбор цвета
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            (sender as Button).BackColor = colorDialog1.Color;
        }

        private void button4_Click(object sender, EventArgs e)//clear
        {
            drawArea = new Bitmap(Lab3.Properties.Resources.grid);
            pictureBox1.Image = drawArea;

            drawArea = new Bitmap(pictureBox1.Image);
            g = Graphics.FromImage(drawArea);

            flag = false;
            flag2 = false;
            button2.Text = "Заливка цветом";
            button3.Text = "Заливка картинкой";

        }

        /*начало блока дря рисования*/
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            
                Zajata = true;
                currentPoint = e.Location;
            
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Zajata = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!flag)
            {
                if (Zajata)
                {
                    prevPoint = currentPoint;
                    currentPoint = e.Location;
                    paint();
                }
            }
        }

        private void paint()
        {
            Pen p = new Pen(Color.Black, 1);
            g.DrawLine(p, prevPoint, currentPoint);
            pictureBox1.Image = drawArea;
        }
        /*конец блока дря рисования*/


        private void button3_Click(object sender, EventArgs e)//заливка картинкой
        {
            flag2 = !flag2;
            if (flag2)
                button3.Text = "Рисовать далее";
            else
                button3.Text = "Заливка картинкой";
        }
    }
}
