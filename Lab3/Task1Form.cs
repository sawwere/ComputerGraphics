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
        Color Color_Zalivka=Color.White;
        Bitmap kartinka;

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

            flag2 = false;
            button3.Text = "Заливка картинкой";
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

            flag = false;
            button2.Text = "Заливка цветом";
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if ((flag) && (!flag2))
            {
                clickX = Convert.ToInt32(e.X);
                clickY = Convert.ToInt32(e.Y);
                task_a(Convert.ToInt32(e.X), Convert.ToInt32(e.Y));
            }
            if ((!flag) && (flag2))
            {
                clickX = Convert.ToInt32(e.X);
                clickY = Convert.ToInt32(e.Y);
                Color cvet = drawArea.GetPixel(e.X, e.Y);

                task_b(Convert.ToInt32(e.X), Convert.ToInt32(e.Y),cvet);
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

            if (x_left == x_right)//4 часа      ┗|｀O′|┛      (┬┬﹏┬┬)     .·´¯`(>▂<)´¯`·.         (╯°□°）╯︵ ┻━┻
                drawArea.SetPixel(x_left, y, Color_Zalivka);
            else
            g.DrawLine(p, x_left, y, x_right, y);

            for (x = x_left; x <= x_right; x++)//рисуем линии сверху 
            {
                help_variable = drawArea.GetPixel(x, y + 1);
                if ((help_variable.ToArgb() != Color.Black.ToArgb()) && (help_variable.ToArgb() != Color_Zalivka.ToArgb()))
                {
                    task_a(x, (y + 1));
                }
            }
            for (x = x_left; x <= x_right; x++)//рисуем линии сверху 
            {
                help_variable = drawArea.GetPixel(x, y - 1);
                if ((help_variable.ToArgb() != Color.Black.ToArgb()) && (help_variable.ToArgb() != Color_Zalivka.ToArgb()))
                {
                    task_a(x, (y - 1));
                }
            }
            pictureBox1.Image = drawArea;
        }

        private void task_b(int x, int y, Color cvet)
        {
            Color help_variable;


            //var c = drawArea.GetPixel(x,)
            if (drawArea.GetPixel(x, y).ToArgb() != cvet.ToArgb())
                return;
            int startX = x;
            while (drawArea.GetPixel(x, y).ToArgb() == cvet.ToArgb())
            {
                drawArea.SetPixel(x, y, kartinka.GetPixel
                (Math.Abs((x - clickX + kartinka.Width)) % kartinka.Width, Math.Abs((y - clickY + kartinka.Height-1)) % kartinka.Height));
                x++;
                if (x >= drawArea.Width)
                {
                    x--;
                    break;
                }
            }
            int rightX = x;
            x = startX - 1;
            if (x < 0)
                x++;
            while (drawArea.GetPixel(x, y).ToArgb() == cvet.ToArgb())
            {
                drawArea.SetPixel(x, y, kartinka.GetPixel
                (Math.Abs((x - clickX + kartinka.Width)) % kartinka.Width, Math.Abs((y - clickY + kartinka.Height+1)) % kartinka.Height));
                x--;
                if (x < 0)
                {
                    x++;
                    break;
                }
            }
            for (int i = x + 1; i < rightX; i++)
            {
                task_b(i, y - 1,cvet);
                task_b(i, y + 1,cvet);
            }
            pictureBox1.Image = drawArea;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog1.FileName;
            pictureBox2.Image= new Bitmap(filename);
            kartinka = new Bitmap(filename);
        }


        //private void FillPicture(int x, int y, Color oldColor)
        //{
        //    if (x < 0 || y < 0 || x >= map.Width || y >= map.Height)
        //        return;
        //    if (map.GetPixel(x, y) != oldColor)
        //        return;
        //    int startX = x;
        //    while (map.GetPixel(x, y) == oldColor)
        //    {
        //        map.SetPixel(x, y, pictureMap.GetPixel
        //        (x % pictureMap.Width, y % pictureMap.Height));
        //        x++;
        //        if (x >= map.Width)
        //        {
        //            x--;
        //            break;
        //        }
        //    }
        //    int rightX = x;
        //    x = startX - 1;
        //    if (x < 0)
        //        x++;
        //    while (map.GetPixel(x, y) == oldColor)
        //    {
        //        map.SetPixel(x, y, pictureMap.GetPixel
        //        (x % pictureMap.Width, y % pictureMap.Height));
        //        x--;
        //        if (x < 0)
        //        {
        //            x++;
        //            break;
        //        }
        //    }
        //    for (int i = x + 1; i < rightX; i++)
        //    {
        //        FillPicture(i, y - 1, oldColor);
        //        FillPicture(i, y + 1, oldColor);
        //    }
        //}
    }
}
