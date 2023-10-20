using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Tools.FastBitmap;
using Tools.Primitives;
using Tools;


namespace Lab6
{
    public partial class MainForm : Form
    {
        Graphics g;
        Projection projection = Projection.ORTHOGR_X;
        Edge3D axisLineX;
        Edge3D axisLineY;
        Edge3D axisLineZ;

        private float startAxisValue = 0;
        private float deltaAxis = 0;
        private enum DeltaAxis { X, Y, Z, None };
        private DeltaAxis curDeltaAxis = DeltaAxis.None;

        Mesh figure = null;

        public MainForm()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            //g = pictureBox1.CreateGraphics();
            g.TranslateTransform(pictureBox1.ClientSize.Width / 2, pictureBox1.ClientSize.Height / 2);
            g.ScaleTransform(1, -1);
            figure = Tools.Meshes.MeshBuilder.LoadFromFile(@"..//..//..//Tools\Meshes\Гексаэдр.stl");
            axisLineX = new Edge3D(new Point3D(0, 0, 0), new Point3D(200, 0, 0), Color.Red);
            axisLineY = new Edge3D(new Point3D(0, 0, 0), new Point3D(0, 200, 0), Color.Green);
            axisLineZ = new Edge3D(new Point3D(0, 0, 0), new Point3D(0, 0, 200), Color.Blue);
            comboBoxProjection.SelectedIndex = 0;

            groupBoxInspector.Text = $"Объект: Гексаэдр.stl";
        }

        private void DrawAxesLines()
        {
            axisLineX.Draw(g, projection);
            axisLineY.Draw(g, projection);
            axisLineZ.Draw(g, projection);
        }

        private void UpdateInspector()
        {
            textBoxPosX.Text = figure.Center.X.ToString("0.000");
            textBoxPosY.Text = figure.Center.Y.ToString("0.000");
            textBoxPosZ.Text = figure.Center.Z.ToString("0.000");
        }

        private void Render()
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            g.Clear(Color.Gray);
            figure.Draw(g, projection);
            DrawAxesLines();
            pictureBox1.Refresh();
            stopWatch.Stop();

            labelFPS.Text = $"FPS: {(1000.0f / stopWatch.ElapsedMilliseconds)}";
            
            UpdateInspector();
        }

        private void comboBoxProjection_SelectedIndexChanged(object sender, EventArgs e)
        {
            projection = (Projection)comboBoxProjection.SelectedIndex;
            Render();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog1.Filter = "Stl files (*.stl)|*.stl|All files (*.*)|*.*";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    try
                    {
                        var filename = openFileDialog1.FileName;
                        figure = Tools.Meshes.MeshBuilder.LoadFromFile(filename);
                        groupBoxInspector.Text = $"Объект: {filename.Split('\\').Last()}";
                        Render();
                    }
                    catch
                    {
                        DialogResult result = MessageBox.Show("Could not open file",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            
        }

        private void buttonApplyTransform_Click(object sender, EventArgs e)
        {
            //TRANSLATE
            float dx = float.Parse(textBoxDX.Text);
            float dy = float.Parse(textBoxDY.Text);
            float dz = float.Parse(textBoxDZ.Text);
            figure.Translate(dx, dy, dz);

            //SCALE
            float kx = (float)numericUpDown4.Value;
            float ky = (float)numericUpDown5.Value;
            float kz = (float)numericUpDown6.Value;
            figure.Scale(kx, ky, kz);

            //ROTATE
            float old_x = figure.Center.X;
            float old_y = figure.Center.Y;
            float old_z = figure.Center.Z;
            figure.Translate(-old_x, -old_y, -old_z);

            float rX = float.Parse(textBoxRX.Text);
            figure.Rotate(rX, Axis.AXIS_X);
            float rY = float.Parse(textBoxRY.Text);
            figure.Rotate(rY, Axis.AXIS_Y);
            float rZ = float.Parse(textBoxRZ.Text);
            figure.Rotate(rZ, Axis.AXIS_Z);

            figure.Translate(old_x, old_y, old_z);

            Render();
        }

        private void buttonReflectZ_Click(object sender, EventArgs e)
        {

        }

        private void buttonReflectX_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonReflectY_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            
            var p = new Point2D(e.X - pictureBox1.Width / 2, -(e.Y - pictureBox1.Height / 2));
            var prx = axisLineX.ProjectedEdge(projection);
            if (Math.Abs(p.CompareToEdge2(prx)) < 1000)
            {
                curDeltaAxis = DeltaAxis.X;
                startAxisValue = p.X;
                return;
            }
            prx = axisLineY.ProjectedEdge(projection);
            if (Math.Abs(p.CompareToEdge2(prx)) < 1000)
            {
                curDeltaAxis = DeltaAxis.Y;
                startAxisValue = -p.Y;
                return;
            }
            prx = axisLineZ.ProjectedEdge(projection);
            if (Math.Abs(p.CompareToEdge2(prx)) < 1000)
            {
                curDeltaAxis = DeltaAxis.Z;
                startAxisValue = p.X;
                return;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            startAxisValue = 0;
            deltaAxis = 0;
            curDeltaAxis = DeltaAxis.None;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            switch (curDeltaAxis)
            {
                case DeltaAxis.X:
                    {
                        var p = new Point2D(e.X - pictureBox1.Width / 2, e.Y - pictureBox1.Height / 2);
                        deltaAxis = p.X - startAxisValue;
                        startAxisValue = p.X;
                        figure.Translate(deltaAxis , 0, 0);
                        break;
                    }
                case DeltaAxis.Y:
                    {
                        var p = new Point2D(e.X - pictureBox1.Width / 2, e.Y - pictureBox1.Height / 2);
                        deltaAxis = p.Y - startAxisValue;
                        startAxisValue = p.Y;
                        figure.Translate(0, -deltaAxis, 0);
                        break;
                    }
                case DeltaAxis.Z:
                    {
                        var p = new Point2D(e.X - pictureBox1.Width / 2, e.Y - pictureBox1.Height / 2);
                        deltaAxis = p.X - startAxisValue;
                        startAxisValue = p.X;
                        figure.Translate(0, 0, deltaAxis);
                        break;
                    }
                default:
                    return;
            }
            Render();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.TranslateTransform(pictureBox1.ClientSize.Width / 2, pictureBox1.ClientSize.Height / 2);
            g.ScaleTransform(1, -1);
            Render();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void buttonToDefault_Click(object sender, EventArgs e)
        {
            //TRANSLATE
            var dx = figure.Center.X;
            var dy = figure.Center.Y;
            var dz = figure.Center.Z;
            figure.Translate(-dx, -dy, -dz);

            Render();
        }

        private void textBoxDX_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text == "")
                tb.Text = "0";
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            
        }
    }
}
