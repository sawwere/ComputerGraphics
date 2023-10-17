using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab5
{
    public partial class Task3Form : Form
    {
        private Graphics g;
        private Bitmap drawArea;
        public List<PointF> points = new List<PointF>();
        public List<PointF> points_help = new List<PointF>();

        int count =-1;
        int count_help = -1;
        public PointF[] result = new PointF[101];
        public PointF[] result2 = new PointF[101];
        public Task3Form()
        {
            InitializeComponent();
            g = this.CreateGraphics();
            g.Clear(Color.White);
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            drawArea = new Bitmap(pictureBox1.Image);
            g = Graphics.FromImage(drawArea);

        }

        private void Task3Form_Load(object sender, EventArgs e)
        {

        }



        private void button2_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            drawArea = new Bitmap(pictureBox1.Image);
            g = Graphics.FromImage(drawArea);
            points.Clear();
            points_help.Clear();
            count = -1;
            count_help = -1;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            g.FillEllipse(new SolidBrush(Color.Black), e.X-5, e.Y-5, 10, 10);           
            pictureBox1.Image = drawArea;
            points.Add(e.Location);
            points_help.Add(e.Location);


            count_help++;
            count++;
            if (count == 3)
            {
                first();
            }
            else if (count > 3)
            {
                Bezier();
            }

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

        }
        
        void first()
        {
            int j = 0;
            float step = 0.01f;
            for (float t = 0; t <= 1; t += step)
            {
                float bx = points[0].X*(1-t)*(1-t)*(1-t) + 3* points[1].X * (1 - t) * (1 - t)*t+3* points[2].X * (1 - t)*t*t+ points[3].X *t*t*t;
                float by = points[0].Y * (1 - t) * (1 - t) * (1 - t) + 3 * points[1].Y * (1 - t) * (1 - t) * t + 3 * points[2].Y * (1 - t) * t * t + points[3].Y * t * t * t;
                result2[j] = new PointF(bx, by);
                j++;
            }
            g.DrawLines(new Pen(Color.Blue), result2);
            pictureBox1.Image = drawArea;
        }

        
        void Bezier()
        {

            g.Clear(Color.White);
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            drawArea = new Bitmap(pictureBox1.Image);
            g = Graphics.FromImage(drawArea);
            int j = 0;
            float step = 0.01f;
            g.DrawLines(new Pen(Color.White), result2);// плохо
            g.DrawLines(new Pen(Color.White), result);//плохо
            


            result = result2;
            PointF a, b, c, d, e;

            b = points[count - 2];//бесполезно по сути
            d = points[count - 1];//бесполезно по сути

            float x =(points_help[count_help - 1].X+ points_help[count_help - 2].X)/2;//центр между n-2 и n-1
            float y = (points_help[count_help - 1].Y + points_help[count_help - 2].Y) / 2;//центр между n-2 и n-1
            b.X = x; b.Y = y;
            float x2 = (points_help[count_help ].X + points_help[count_help-1].X) / 2;//центр между n-1 и n
            float y2 = (points_help[count_help ].Y + points_help[count_help-1].Y) / 2;//центр между n-1 и n
            d.X = x2; d.Y = y2;


            a = points[count - 2];
            c = points[count - 1];
            e = points[count];


            points.RemoveAt(count);
            points.RemoveAt(count-1);
            points.RemoveAt(count-2);

            points.Add(a);
            points.Add(b);
            points.Add(c);
            points.Add(d);
            points.Add(e);
            count = count + 2;

            for (float t = 0; t <= 1; t += step)
            {
                float bx = points[count-6].X * (1 - t) * (1 - t) * (1 - t) + 3 * points[count-5].X * (1 - t) * (1 - t) * t + 3 * points[count-4].X * (1 - t) * t * t + points[count-3].X * t * t * t;
                float by = points[count-6].Y * (1 - t) * (1 - t) * (1 - t) + 3 * points[count - 5].Y * (1 - t) * (1 - t) * t + 3 * points[count - 4].Y * (1 - t) * t * t + points[count - 3].Y * t * t * t;
                result[j] = new PointF(bx, by);
                j++;
            }
            g.DrawLines(new Pen(Color.Blue), result);
            j = 0;
            for (float t = 0; t <= 1; t += step)
            {
                float bx = points[count - 3].X * (1 - t) * (1 - t) * (1 - t) + 3 * points[count - 2].X * (1 - t) * (1 - t) * t + 3 * points[count - 1].X * (1 - t) * t * t + points[count].X * t * t * t;
                float by = points[count-3].Y * (1 - t) * (1 - t) * (1 - t) + 3 * points[count-2].Y * (1 - t) * (1 - t) * t + 3 * points[count-1].Y * (1 - t) * t * t + points[count].Y * t * t * t;
                result2[j] = new PointF(bx, by);
                j++;
            }
            
            g.DrawLines(new Pen(Color.Blue), result2);
            pictureBox1.Image = drawArea;
        }

    }
}
