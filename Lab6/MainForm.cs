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
using Tools.Scene;

namespace Lab6
{
    

    public partial class MainForm : Form
    {
        private FormRotationFigure rotationFigure = new FormRotationFigure();

        Graphics g;
        Projection projection = Projection.ORTHOGR_X;
        Edge3D axisLineX;
        Edge3D axisLineY;
        Edge3D axisLineZ;
        Edge3D line_1;

        private float startAxisValue = 0;
        private float deltaAxis = 0;
        private enum DeltaAxis { X, Y, Z, None };
        private DeltaAxis curDeltaAxis = DeltaAxis.None;

        private Scene scene;
        private Camera camera;

        SceneObject figure = null;

        private void radioButtonScene_Click(object sender, EventArgs e)
        {
            figure = scene.GetObject((Guid)((RadioButton)sender).Tag);
            inspector.GetUpdate(figure);
        }

        private void AddToHierarchy(string name, Guid id)
        {
            RadioButton rb = new RadioButton();
            rb.Tag = id;
            rb.Name = name;
            rb.Checked = false;
            rb.Text = rb.Name;
            rb.Width = panelSceneHierarchy.Width - 40;
            rb.Height = 30;
            rb.Location = new Point(15, panelSceneHierarchy.Controls.Count * (rb.Height));
            rb.Click += radioButtonScene_Click;
            panelSceneHierarchy.Controls.Add(rb);
        }

        public void UpdateHierarchy()
        {
            var selected = 0;
            if (scene.Count() <= panelSceneHierarchy.Controls.Count)
            {
                int i = 0;
                foreach (RadioButton r in panelSceneHierarchy.Controls)
                {
                    if (r.Checked)
                    {
                        selected = i;
                        break;
                    }
                    i++;
                }
            }
            else
            {
                selected = panelSceneHierarchy.Controls.Count;
            }
            panelSceneHierarchy.Controls.Clear();
            foreach (var pair in scene.GetAllSceneObjects())
            {
                AddToHierarchy(pair.Value.Name, pair.Value.Id);
            }
            (panelSceneHierarchy.Controls[selected] as RadioButton).Checked = true;
        }

        public MainForm()
        {
            InitializeComponent();
            AddOwnedForm(rotationFigure);

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.TranslateTransform(pictureBox1.ClientSize.Width / 2, pictureBox1.ClientSize.Height / 2);
            g.ScaleTransform(1, -1);
            scene = new Scene();

            camera = new Camera(new Point3D(0, 0, -1000), new Point3D(0, 0, 0));
            scene.camera = camera;

            figure = new SceneObject();
            figure.Local = Tools.Meshes.MeshBuilder.LoadFromFile(@"..//..//..//Tools//Meshes//Гексаэдр.stl");
            figure.Name = "Гексаэдр";
            scene.AddObject(figure);
            UpdateHierarchy();



            axisLineX = new Edge3D(new Point3D(0, 0, 0), new Point3D(200, 0, 0), Color.Red);
            axisLineY = new Edge3D(new Point3D(0, 0, 0), new Point3D(0, 200, 0), Color.Green);
            axisLineZ = new Edge3D(new Point3D(0, 0, 0), new Point3D(0, 0, 200), Color.Blue);
            comboBoxProjection.SelectedIndex = 0;
        }

        private void AddMeshToScene()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog1.Filter = "Stl files (*.stl)|*.stl|All files (*.*)|*.*";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    try
                    {
                        var filename = openFileDialog1.FileName;
                        figure = new SceneObject();
                        figure.Local = Tools.Meshes.MeshBuilder.LoadFromFile(filename);

                        StringBuilder figureName = new StringBuilder();
                        figureName.Append(filename.Split('\\').Last().Split('.').First());
                        figure.Name = figureName.ToString();
                        scene.AddObject(figure);
                        UpdateHierarchy();

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

        private void DrawAxesLines()
        {
            axisLineX.Draw(g, projection);
            axisLineY.Draw(g, projection);
            axisLineZ.Draw(g, projection);
            float x_1 = (float)numericUpDown12.Value;
            float y_1 = (float)numericUpDown11.Value;
            float z_1 = (float)numericUpDown10.Value;

            float x_2 = (float)numericUpDown15.Value;
            float y_2 = (float)numericUpDown14.Value;
            float z_2 = (float)numericUpDown13.Value;


            line_1 = new Edge3D(new Point3D(x_1, y_1, z_1), new Point3D(x_2, y_2, z_2), Color.Purple);
            line_1.Draw(g, projection);
        }

        public void Render()
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            Color backgroundColor = Color.Gray;
            g.Clear(backgroundColor);
            foreach (var kv in scene.GetAllSceneObjects())
            {
                kv.Value.Tranformed.Draw(g, projection);
            }

            DrawAxesLines();
            pictureBox1.Refresh();
            stopWatch.Stop();
            inspector.GetUpdate(figure);

            labelFPS.Text = $"FPS: {(1000.0f / stopWatch.ElapsedMilliseconds)}";
        }

        private void comboBoxProjection_SelectedIndexChanged(object sender, EventArgs e)
        {
            projection = (Projection)comboBoxProjection.SelectedIndex;
            Render();
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
            float o_x = figure.Tranformed.Center.X;
            float o_y = figure.Tranformed.Center.Y;
            float o_z = figure.Tranformed.Center.Z;
            figure.Translate(-o_x, -o_y, -o_z);
            figure.Scale(kx, ky, kz);
            figure.Translate(o_x, o_y, o_z);

            //ROTATE
            float old_x = figure.Tranformed.Center.X;
            float old_y = figure.Tranformed.Center.Y;
            float old_z = figure.Tranformed.Center.Z;
            figure.Translate(-old_x, -old_y, -old_z);

            float rX = float.Parse(textBoxRX.Text);
            float rY = float.Parse(textBoxRY.Text);
            float rZ = float.Parse(textBoxRZ.Text);
            figure.Rotate(rX, rY, rZ);

            figure.Translate(old_x, old_y, old_z);

            Render();
        }

        private void buttonReflectZ_Click(object sender, EventArgs e)     
        {
            figure.Tranformed.reflectY();
            Render();
        }

        private void buttonReflectX_Click(object sender, EventArgs e)
        {
            figure.Tranformed.reflectZ();
            Render();
        }

        private void buttonReflectY_Click(object sender, EventArgs e)
        {
            figure.Tranformed.reflectX();
            Render();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (figure == null)
            {
                return;
            }
            var p = new Point2D(e.X - pictureBox1.Width / 2, -(e.Y - pictureBox1.Height / 2));
            var prx = axisLineX.ProjectedEdge(projection);
            if (prx.Length > 0.1f && Math.Abs(p.CompareToEdge2(prx)) < 2000)
            {
                curDeltaAxis = DeltaAxis.X;
                startAxisValue = p.X;
                return;
            }
            prx = axisLineY.ProjectedEdge(projection);
            if (prx.Length > 0.1f && Math.Abs(p.CompareToEdge2(prx)) < 2000)
            {
                curDeltaAxis = DeltaAxis.Y;
                startAxisValue = -p.Y;
                return;
            }
            prx = axisLineZ.ProjectedEdge(projection);
            if (prx.Length > 0.1f && Math.Abs(p.CompareToEdge2(prx)) < 2000)
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
            if (figure == null)
            {
                return;
            }
            switch (curDeltaAxis)
            {
                case DeltaAxis.X:
                    {
                        var p = new Point2D(e.X - pictureBox1.Width / 2, e.Y - pictureBox1.Height / 2);
                        deltaAxis = p.X - startAxisValue;
                        startAxisValue = p.X;
                        figure.Translate(deltaAxis, 0, 0);
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
        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Width <= 0 || pictureBox1.Height <= 0)
                return;
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.TranslateTransform(pictureBox1.ClientSize.Width / 2, pictureBox1.ClientSize.Height / 2);
            g.ScaleTransform(1, -1);
            Render();
        }        

        private void textBoxDX_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text == "")
                tb.Text = "0";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetHierarchy();
            AddMeshToScene();
        }

        private void buttonRotateFigure_Click(object sender, EventArgs e)
        {
            if (rotationFigure.ShowDialog() == DialogResult.OK)
            {
                var list = rotationFigure.GetPoints();
                Axis axis = rotationFigure.RotaionAxis;
                int steps = rotationFigure.Steps;
                //TODO
                //figure.Local = "создание фигуры вращения"
            }
        }

        private void buttonFunction_Click(object sender, EventArgs e)
        {
            //TODO
            //figure.Local = "создание фигуры графика"
        }

        private void ResetHierarchy()
        {
            scene.Clear();
            figure = null;
            panelSceneHierarchy.Controls.Clear();
        }

        private void sceneAddFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddMeshToScene();
        }

        private void buttonSceneClear_Click(object sender, EventArgs e)
        {
            ResetHierarchy();
            Render();
        }

        private void buttonRotateAround_Click(object sender, EventArgs e)
        {
            float x_1 = (float)numericUpDown12.Value;
            float y_1 = (float)numericUpDown11.Value;
            float z_1 = (float)numericUpDown10.Value;

            float x_2 = (float)numericUpDown15.Value;
            float y_2 = (float)numericUpDown14.Value;
            float z_2 = (float)numericUpDown13.Value;

            
            line_1 = new Edge3D(new Point3D(x_1, y_1, z_1), new Point3D(x_2, y_2, z_2), Color.Purple);                   

            float angle = (float)numericUpDown16.Value;
            figure.RotateAroundAxis(angle, Axis.CUSTOM, line_1);


            Render();
        }

        private void hexahedronToolStripMenuItem_Click(object sender, EventArgs e)
        {
            figure = new SceneObject();
            figure.Local.make_hexahedron();
            figure.Name = "Гексаэдр";
            scene.AddObject(figure);
            UpdateHierarchy();

            Render();
        }

        private void tetrahedronToolStripMenuItem_Click(object sender, EventArgs e)
        {
            figure = new SceneObject();
            figure.Local.make_tetrahedron();
            figure.Name = "Тетраэдр";
            scene.AddObject(figure);
            UpdateHierarchy();

            Render();
        }

        private void octahedronToolStripMenuItem_Click(object sender, EventArgs e)
        {
            figure = new SceneObject();
            figure.Local.make_octahedron();
            figure.Name = "Октаэдр";
            scene.AddObject(figure);
            UpdateHierarchy();

            Render();
        }

        private void icosahedronToolStripMenuItem_Click(object sender, EventArgs e)
        {
            figure = new SceneObject();
            figure.Local.make_icosahedron();
            figure.Name = "Икосаэдр";
            scene.AddObject(figure);
            UpdateHierarchy();

            Render();
        }

        private void dodecahedronToolStripMenuItem_Click(object sender, EventArgs e)
        {
            figure = new SceneObject();
            figure.Local.make_dodecahedron();
            figure.Name = "Додекаэдр";
            scene.AddObject(figure);
            UpdateHierarchy();

            Render();
        }

        //
        // CAMERA
        //
        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((this.ActiveControl as TextBox) != null)
            {
                return;
            }
            switch (e.KeyChar)
            {
                case 'a':
                    {
                        camera.position.Translate(1, 0, 0);
                        break;
                    }
                case 'd':
                    {
                        camera.position.Translate(-1, 0, 0);
                        break;
                    }
                case 'w':
                    {
                        camera.position.Translate(0, 0, 1);
                        break;
                    }
                case 's':
                    {
                        camera.position.Translate(0, 0, -1);
                        break;
                    }
                case 'z':
                    {
                        camera.position.Translate(0, -1, 0);
                        break;
                    }
                case 'x':
                    {
                        camera.position.Translate(0, 1, 0);
                        break;
                    }

            }
            Render();
        }
    }
}
