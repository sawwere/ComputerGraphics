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
        int clickX;
        int clickY;
        Color Color_Zalivka=Color.Blue;


        public Task1Form()
        {
            InitializeComponent();
            g = this.CreateGraphics();
            g.Clear(Color.White);           
            drawArea = new Bitmap(pictureBox1.Image);
            g = Graphics.FromImage(drawArea);
            SetBorders();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }
        private void SetBorders()
        {
            Pen blackPen = new Pen(Color.Black, 1);
            Rectangle rect = new Rectangle(0, 0, pictureBox1.Width-1, pictureBox1.Height-1);
            g.DrawRectangle(blackPen, rect);
            pictureBox1.Image = drawArea;
        }//рисуем прямоугольник
        private void button2_Click(object sender, EventArgs e)//кнопка! заливка цветом/рисуем
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
            Color_Zalivka = colorDialog1.Color;
        }

        private void button4_Click(object sender, EventArgs e)//clear
        {
            drawArea = new Bitmap(Lab3.Properties.Resources.grid);
            pictureBox1.Image = drawArea;

            drawArea = new Bitmap(pictureBox1.Image);
            g = Graphics.FromImage(drawArea);
            SetBorders();
            flag = false;
            flag2 = false;
            button2.Text = "Заливка цветом";
            button3.Text = "Заливка картинкой";

        }

        /*начало блока дря рисования*/
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if ((!flag)&&(!flag2))
            {
                Zajata = true;
                currentPoint = e.Location;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Zajata = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((!flag) && (!flag2)) 
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


        private void button3_Click(object sender, EventArgs e)//кнопка! заливка картинкой/рисуем
        {
            flag2 = !flag2;
            if (flag2)
                button3.Text = "Рисовать далее";
            else
                button3.Text = "Заливка картинкой";
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (flag)
            {
                clickX = Convert.ToInt32(e.X);
                clickY = Convert.ToInt32(e.Y);
                task_a(Convert.ToInt32(e.X), Convert.ToInt32(e.Y));
            }
        }//считываем клик

        private void task_a(int x, int y)
        {
            int x_right =x, x_left=x;

            Color help_variable;
            do
            {
                help_variable = drawArea.GetPixel(--x_left, y);
            } while (help_variable.ToArgb() != Color.Black.ToArgb()); //ищем левую границу

            do
            {
                help_variable = drawArea.GetPixel(++x_right, y);
            } while (help_variable.ToArgb() != Color.Black.ToArgb());//ищем правую границу

            x_left++;//!!! 
            x_right--;//!!!

            Pen p = new Pen(Color_Zalivka);
            g.DrawLine(p, x_left, y, x_right, y);
            //pictureBox1.Image = drawArea;

            for (x = x_left; x <= x_right; x++)//рисуем линии сверху 
            {
                help_variable = drawArea.GetPixel(x, y + 1);
                if ((help_variable.ToArgb() != Color.Black.ToArgb()) && (help_variable.ToArgb() != Color_Zalivka.ToArgb()))
                {
                    task_a (x, (y + 1));
                }
            }

            for (x = x_left; x <= x_right; x++)//рисуем линии снизу 
            {
                help_variable = drawArea.GetPixel(x, y - 1);
                if ((help_variable.ToArgb() != Color.Black.ToArgb()) && (help_variable.ToArgb() != Color_Zalivka.ToArgb()))
                {
                    task_a(x, (y - 1));
                }
            }

            pictureBox1.Image = drawArea;
        }

    }
}
