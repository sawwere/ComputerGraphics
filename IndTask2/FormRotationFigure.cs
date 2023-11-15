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

namespace IndTask2
{
    public partial class FormRotationFigure : Form
    {
        private Graphics g;
        private Polygon polygon;
        public Axis RotaionAxis { get { return (Axis)(int)comboBoxAxis.SelectedIndex; } }
        public int Steps { get { return (int)numericUpDown4.Value; } }

        public FormRotationFigure()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            comboBoxAxis.SelectedIndex = 1;
            pictureBox1.Refresh();
            polygon = new Polygon();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            polygon.AddNextPoint(new Point2D(e.X, e.Y));
            g.Clear(Color.Gray);
            polygon.Draw(g);
            pictureBox1.Refresh();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            g.Clear(Color.Gray);
            pictureBox1.Refresh();
            polygon = new Polygon();
        }

        public List<Point2D> GetPoints()
        {
            float min_x = float.MaxValue;
            float min_y = float.MaxValue;
            List<Point2D> res = new List<Point2D>();
            for (int i = 0; i < polygon.Count; i++)
                res.Add(polygon[i]);

            for (int i = 0; i < res.Count; i++)
            {
                if (res[i].X < min_x)
                {
                    min_x = res[i].X;
                }
                if (res[i].Y < min_y)
                {
                    min_y = res[i].Y;
                }
            }
            for (int i = 0; i < res.Count; i++)
            {
                res[i].X = res[i].X - min_x;
                res[i].Y = res[i].Y - min_y;
            }

            return res;
        }
    }
}
