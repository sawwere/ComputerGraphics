using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab5
{
    public partial class Task1Form : Form
    {
        private Graphics g;
        private string axiom;
        private double angle;

        private string filename;
        SortedDictionary<char, string> rules;
        private int iterations;
        public Task1Form()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            rules = new SortedDictionary<char, string>();
            openFileDialog1.InitialDirectory = "/Templates";
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            rules.Clear();
            string[] rule;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    filename = openFileDialog1.FileName;
                    string[] lines = File.ReadAllLines(filename);
                    string[] parameters = lines[0].Split(' ');
                    axiom = parameters[0];
                    angle = Convert.ToDouble(parameters[1]);
                    for (int i = 1; i < lines.Length; i++)
                    {
                        rule = lines[i].Split('=');
                        rules[Convert.ToChar(rule[0])] = rule[1];
                    }
                }
                catch
                {
                    DialogResult result = MessageBox.Show("Could not open file",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonDraw_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);

            var lines = new List<Tuple<double, double, double, double>>();
            var stateStack = new Stack<Tuple<double, double, double, double>>();
            double x = pictureBox1.Width;
            double y = pictureBox1.Height / 2;
            double stepY = 0;
            double stepX = -(pictureBox1.Width / Math.Pow(10, iterations + 1));

            double yMin, yMax;
            double xMin = yMin = double.MaxValue;
            double xMax = yMax = double.MinValue;

            StringBuilder next = new StringBuilder(axiom);
            for (int cur_iter = 0; cur_iter < iterations; cur_iter++)
            {
                string prev = next.ToString();
                next.Clear();
                foreach (char c in prev)
                {
                    if (rules.ContainsKey(c))
                        next.Append( rules[c]);
                    else
                        next.Append(c);
                }
            }
            double tempX, tempY;
            for (int i = 0; i < next.Length; i++)
            {
                switch (next[i])
                {
                    case 'F':
                        lines.Add(new Tuple<double, double, double, double>(x, y, x + stepX, y + stepY));
                        x += stepX;
                        y += stepY;
                        if (x < xMin)
                            xMin = x;
                        if (x > xMax)
                            xMax = x;
                        if (y < yMin)
                            yMin = y;
                        if (y > yMax)
                            yMax = y;
                        break;
                    case '+':
                        tempX = stepX;
                        tempY = stepY;
                        stepX = (tempX * Math.Cos(angle * Math.PI / 180) - tempY * Math.Sin(angle * Math.PI / 180));
                        stepY = (tempX * Math.Sin(angle * Math.PI / 180) + tempY * Math.Cos(angle * Math.PI / 180));
                        break;
                    case '-':
                        tempX = stepX;
                        tempY = stepY;
                        stepX = (tempX * Math.Cos(-angle * Math.PI / 180) - tempY * Math.Sin(-angle * Math.PI / 180));
                        stepY = (tempX * Math.Sin(-angle * Math.PI / 180) + tempY * Math.Cos(-angle * Math.PI / 180));
                        break;
                    case '[':
                        stateStack.Push(new Tuple<double, double, double, double>(x, y, stepX, stepY));
                        break;

                    case ']':
                        Tuple<double, double, double, double> coords = stateStack.Pop();
                        x = coords.Item1;
                        y = coords.Item2;
                        stepX = coords.Item3;
                        stepY = coords.Item4;
                        break;
                }
            }

            double scale = Math.Max(xMax - xMin, yMax - yMin);
            foreach (var p in lines)
                g.DrawLine(Pens.Blue,
                    (float)((xMax - p.Item1) / scale * pictureBox1.Width),
                    (float)((yMax - p.Item2) / scale * pictureBox1.Height),
                    (float)((xMax - p.Item3) / scale * pictureBox1.Width),
                    (float)((yMax - p.Item4) / scale * pictureBox1.Height));

            pictureBox1.Invalidate();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            pictureBox1.Invalidate();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            iterations = (int)numericUpDown1.Value;
        }

        private void buttonBeautiful_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);

            var lines = new List<Tuple<double, double, double, double, int>>();
            var stateStack = new Stack<Tuple<double, double, double, double, int>>();
            double x = pictureBox1.Width / 2;
            double y = 0;
            double stepY = pictureBox1.Height / Math.Pow(10, iterations + 1);
            double stepX = 0;
            double yMin, yMax;
            double xMin = yMin = double.MaxValue;
            double xMax = yMax = double.MinValue;


            StringBuilder next = new StringBuilder(axiom);
            for (int cur_iter = 0; cur_iter < iterations; cur_iter++)
            {
                string prev = next.ToString();
                next.Clear();
                foreach (char c in prev)
                {
                    if (rules.ContainsKey(c))
                        next.Append(rules[c]);
                    else
                        next.Append(c);
                }
            }
            Color brown = Color.FromArgb(255, 71, 39, 0);
            int depth = 0;
            double tempX, tempY;
            for (int i = 0; i < next.Length; i++)
            {
                switch (next[i])
                {
                    case 'F':
                        lines.Add(new Tuple<double, double, double, double, int>(x, y, x + stepX, y + stepY, depth));
                        x += stepX;
                        y += stepY;
                        if (x < xMin)
                            xMin = x;
                        if (x > xMax)
                            xMax = x;
                        if (y < yMin)
                            yMin = y;
                        if (y > yMax)
                            yMax = y;
                        break;
                    case '+':
                        tempX = stepX;
                        tempY = stepY;
                        var randAngle = new Random().NextDouble() * 100 / 10;
                        stepX = (tempX * Math.Cos(randAngle * Math.PI / 180) - tempY * Math.Sin(randAngle * Math.PI / 180));
                        stepY = (tempX * Math.Sin(randAngle * Math.PI / 180) + tempY * Math.Cos(randAngle * Math.PI / 180));
                        break;
                    case '-':
                        tempX = stepX;
                        tempY = stepY;
                        randAngle = new Random().NextDouble() * 100 / 10;
                        stepX = (tempX * Math.Cos(-randAngle * Math.PI / 180) - tempY * Math.Sin(-randAngle * Math.PI / 180));
                        stepY = (tempX * Math.Sin(-randAngle * Math.PI / 180) + tempY * Math.Cos(-randAngle * Math.PI / 180));
                        break;
                    case '[':
                        stateStack.Push(new Tuple<double, double, double, double, int>(x, y, stepX, stepY, depth + 1));
                        depth++;
                        break;

                    case ']':
                        Tuple<double, double, double, double, int> coords = stateStack.Pop();
                        x = coords.Item1;
                        y = coords.Item2;
                        stepX = coords.Item3;
                        stepY = coords.Item4;
                        depth--;
                        break;
                }
            }

            double scale = Math.Max(xMax - xMin, yMax - yMin);
            foreach (var p in lines)
            {
                var colorR = Interpolate(0, brown.R, iterations, 0, p.Item5);
                var colorG = Interpolate(0, brown.G, iterations, 255, p.Item5);
                var colorB = Interpolate(0, brown.B, iterations, 0, p.Item5);
                Pen curPen = new Pen(Color.FromArgb(255, colorR, colorG, colorB));
                curPen.Width = iterations - p.Item5;
                g.DrawLine(curPen,
                    (float)((xMax - p.Item1) / scale * pictureBox1.Width),
                    (float)((yMax - p.Item2) / scale * pictureBox1.Height),
                    (float)((xMax - p.Item3) / scale * pictureBox1.Width),
                    (float)((yMax - p.Item4) / scale * pictureBox1.Height));
            }

            pictureBox1.Invalidate();
        }

        private int Interpolate(int x0, int y0, int x1, int y1, int i)
        {
            if (x0 == x1)
                return y0;
            return y0 + ((y1 - y0) * (i - x0)) / (x1 - x0);
        }
    }
}
