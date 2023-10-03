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
        private bool flag3 = false;//выделить границу
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
            flag3 = false;
            button6.Text = "Выделить границу";
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
            flag3 = false;
            button2.Text = "Заливка цветом";
            button3.Text = "Заливка картинкой";
            button6.Text = "Выделить границу";
        }

        /*начало блока дря рисования*/
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if ((!flag)&&(!flag2) && (!flag3))
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
            if ((!flag) && (!flag2) && (!flag3)) 
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
            flag3 = false;
            button6.Text = "Выделить границу";
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if ((flag) && (!flag2) && (!flag3))
            {
                clickX = Convert.ToInt32(e.X);
                clickY = Convert.ToInt32(e.Y);
                task_a(Convert.ToInt32(e.X), Convert.ToInt32(e.Y));
            }
            if ((!flag) && (flag2) && (!flag3))
            {
                clickX = Convert.ToInt32(e.X);
                clickY = Convert.ToInt32(e.Y);
                Color cvet = drawArea.GetPixel(e.X, e.Y);

                task_b(Convert.ToInt32(e.X), Convert.ToInt32(e.Y),cvet);
            }
            if ((!flag) && (!flag2) && (flag3))
            {
                clickX = Convert.ToInt32(e.X);
                clickY = Convert.ToInt32(e.Y);

                task_c(Convert.ToInt32(e.X), Convert.ToInt32(e.Y));
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

            if (x < 0 || y < 0 || x >= drawArea.Width || y >= drawArea.Height)
                return;
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

        private void task_c(int x, int y)
        {
            int x_left = x;

            Color help_variable;
            do
            {
                help_variable = drawArea.GetPixel(--x_left, y);
            } while (help_variable.ToArgb() != Color.Black.ToArgb()); //ищем левую границу

            Point p = new Point(x_left, y);
            List<Point> border = GetBorderPoints(p);

            border = GetBorderPoints(p).OrderBy(xx => xx.Y).ThenBy(xx => xx.X).ToList();

            for (int i = 0; i < border.Count; i++)
            {
                drawArea.SetPixel(border[i].X, border[i].Y, Color.Red);
            }
            var bb = new HashSet<Point>();
            for (int i = 0; i < border.Count - 1; i++)
            {
                {
                    for (int xx = border[i].X; xx < border[i + 1].X; xx++)
                    {
                        if (drawArea.GetPixel(xx, border[i].Y) != Color.FromArgb(255, 0, 0, 0))
                            continue;// drawArea.SetPixel(xx, border[i].Y, Color.Red);
                        else
                        {
                            bb.UnionWith(GetBorderPoints(new Point(xx, border[i].Y)).ToHashSet());
                            break;
                        }
                    }
                }
            }
            border = bb.ToList().OrderBy(xx => xx.Y).ThenBy(xx => xx.X).ToList();
            for (int i = 0; i < border.Count; i++)
            {
                drawArea.SetPixel(border[i].X, border[i].Y, Color.Red);
            }
            pictureBox1.Image = drawArea;
        }


        private List<Point> GetBorderPoints(Point start)
        {
            List<Point> border = new List<Point>();

            Point cur = start;
            border.Add(cur);
            Point next = cur;
            Color borderColor = drawArea.GetPixel(cur.X, cur.Y);

            //Будем идти против часовой стрелки и ходить внутри области
            int dir = 8;
            do
            {
                dir += dir - 1 < 0 ? 7 : -2; // поворот на 90 градусов
                int t = dir;
                do
                {
                    next = cur;
                    switch (dir)
                    {
                        case 0: next.X++; break;
                        case 1: next.X++; next.Y--; break;
                        case 2: next.Y--; break;
                        case 3: next.X--; next.Y--; break;
                        case 4: next.X--; break;
                        case 5: next.X--; next.Y++; break;
                        case 6: next.Y++; break;
                        case 7: next.X++; next.Y++; break;
                    }
                    //Если не нашли - останавливаемся
                    if (next == start)
                        break;
                    if (drawArea.GetPixel(next.X, next.Y) == borderColor)
                    {
                        //Кладем в список
                        border.Add(next);
                        cur = next;
                        //cur_dir = pred_Dir;
                        break;
                    }
                    dir = (dir + 1) % 8;
                } while (dir != t);
            } while (next != start);

            return border;
        }



        private void button5_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog1.FileName;
            pictureBox2.Image = new Bitmap(filename);
            kartinka = new Bitmap(filename);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            flag3 = !flag3;
            if (flag3)
                button6.Text = "Рисовать далее";
            else
                button6.Text = "Выделить границу";
            /*
            foreach (var x in GetBorderPoints(drawArea))
            drawArea.SetPixel(x.X, x.Y, Color.Red);
            pictureBox1.Refresh();*/

            flag = false;
            button2.Text = "Заливка цветом";
            flag2 = false;
            button3.Text = "Заливка картинкой";


        }
    }
}
