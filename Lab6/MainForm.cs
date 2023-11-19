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
using Tools.Meshes;

namespace Lab6
{
    

    public partial class MainForm : Form
    {
        private FormRotationFigure rotationFigure = new FormRotationFigure();
        private FormFloatingHorizon floatingHorizon = new FormFloatingHorizon();

        Graphics g;
        Projection projection = Projection.ORTHOGR_X;
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
            inspector.GetUpdate(figure, scene.Camera);
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
            AddOwnedForm(floatingHorizon);

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.TranslateTransform(pictureBox1.ClientSize.Width / 2, pictureBox1.ClientSize.Height / 2);
            g.ScaleTransform(1, -1);
            camera = new Camera(pictureBox1.Width, pictureBox1.Height, new Point3D(0, 0, -3),
                new Point3D(0, 0, 0), new Point3D(0, 0, 1));
            scene = new Scene(camera);

            var polygons = new List<Triangle3D>();
            polygons.Add(new Triangle3D(new Point3D(1, 1, 1), new Point3D(1, 1, -1), new Point3D(-1, 1, -1)));

            polygons.Add(new Triangle3D(new Point3D(1, 1, 1), new Point3D(-1, 1, -1), new Point3D(-1, 1, 1)));
            var mesh = new Mesh(polygons);
            mesh = MeshBuilder.make_hexahedron();
            figure = new SceneObject(mesh);
            figure.Name = "Гексаэдр";
            scene.AddObject(figure);
            var light = new SceneObject(new Light(new Point3D(0, 0, 0), Color.White), "Light");
            light.Transform.Translate(new Point3D(0, 0, 50));
            scene.AddObject(light);

            UpdateHierarchy();

            comboBoxProjection.SelectedIndex = 0;
            comboBoxRenderMode.SelectedIndex = 0;
            buttonApplyTransform.Select();
            Render();
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
                        figure = new SceneObject(Tools.Meshes.MeshBuilder.LoadFromFile(filename));

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

        public void Render()
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            Color backgroundColor = Color.LightGray;
            g.Clear(backgroundColor);
            switch  (comboBoxRenderMode.SelectedIndex)
            {
                case 0:
                    scene.Render(g, projection);
                    break;
                case 1:
                    pictureBox1.Image = scene.RasterizedRender(projection);
                    break;
                case 2:
                    pictureBox1.Image = scene.GourodRender(projection);
                    break;
            }

            pictureBox1.Refresh();
            stopWatch.Stop();
            inspector.GetUpdate(figure, scene.Camera);

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
            figure.Transform.Translate(new Point3D(dx, dy, dz));

            //SCALE
            float kx = (float)numericUpDown4.Value;
            float ky = (float)numericUpDown5.Value;
            float kz = (float)numericUpDown6.Value;
            Point3D oldPos = figure.Transform.position;
            figure.Transform.Translate(-1 * figure.Transform.position);
            figure.Transform.Scale(new Point3D(kx, ky, kz));
            figure.Transform.Translate(oldPos);

            ////ROTATE
            figure.Transform.Translate(-1 * figure.Transform.position);

            float rX = float.Parse(textBoxRX.Text);
            float rY = float.Parse(textBoxRY.Text);
            float rZ = float.Parse(textBoxRZ.Text);
            figure.Transform.Rotate(new Point3D(rX, rY, rZ));

            figure.Transform.Translate(oldPos);

            Render();
        }

        private void buttonReflectZ_Click(object sender, EventArgs e)     
        {
            figure.Transform.reflectY();
            Render();
        }

        private void buttonReflectX_Click(object sender, EventArgs e)
        {
            figure.Transform.reflectZ();
            Render();
        }

        private void buttonReflectY_Click(object sender, EventArgs e)
        {
            figure.Transform.reflectX();
            Render();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (figure == null)
            {
                return;
            }
            var p = new Point2D(e.X - pictureBox1.Width / 2, -(e.Y - pictureBox1.Height / 2));
            var prx = (scene.systemObjects["axisLineX"].GetTransformed() as Edge3D).ProjectedEdge(projection, scene.Camera);
            if (prx.Length > 0.1f && Math.Abs(p.CompareToEdge2(prx)) < 2000)
            {
                curDeltaAxis = DeltaAxis.X;
                startAxisValue = p.X;
                return;
            }
            prx = (scene.systemObjects["axisLineY"].GetTransformed() as Edge3D).ProjectedEdge(projection, scene.Camera);
            if (prx.Length > 0.1f && Math.Abs(p.CompareToEdge2(prx)) < 2000)
            {
                curDeltaAxis = DeltaAxis.Y;
                startAxisValue = -p.Y;
                return;
            }
            prx = (scene.systemObjects["axisLineZ"].GetTransformed() as Edge3D).ProjectedEdge(projection, scene.Camera);
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
                        figure.Transform.Translate(new Point3D(deltaAxis / 100, 0, 0));
                        break;
                    }
                case DeltaAxis.Y:
                    {
                        var p = new Point2D(e.X - pictureBox1.Width / 2, e.Y - pictureBox1.Height / 2);
                        deltaAxis = p.Y - startAxisValue;
                        startAxisValue = p.Y;
                        figure.Transform.Translate(new Point3D(0, -deltaAxis / 100, 0));
                        break;
                    }
                case DeltaAxis.Z:
                    {
                        var p = new Point2D(e.X - pictureBox1.Width / 2, e.Y - pictureBox1.Height / 2);
                        deltaAxis = p.X - startAxisValue;
                        startAxisValue = p.X;
                        figure.Transform.Translate(new Point3D(0, 0, deltaAxis / 100));
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

            scene.Camera.Width = pictureBox1.Width;
            scene.Camera.Height = pictureBox1.Height;
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
                Mesh mesh;
                List<Point3D> points = new List<Point3D>();
                foreach (var p in list)
                {
                    points.Add(new Point3D(p.X, p.Y, 0));
                }

                mesh = MeshBuilder.BuildRotationFigure(points, axis, steps);

                figure = new SceneObject(mesh);
                figure.Name = "Rotated figure";
                scene.AddObject(figure);

                UpdateHierarchy();
                Render();
            }
        }

        private void buttonFunction_Click(object sender, EventArgs e)
        {
            Func<float, float, float> func;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    func = (x,y) => (float)(10 * Math.Sin(x) + 10 * Math.Sin(y));
                    break; 
                case 1:
                    func = (x, y) => (float)(x + y);
                    break;
                case 2:
                    func = (x, y) => (float)(x * x) / 100;
                    break;
                case 3:
                    func = (x, y) => (float)(Math.Sign(x) * 10);
                    break;
                default:
                    func = (x, y) => (float)(0);
                    break;
            }
            Mesh mesh;
            Point3D point;
            (mesh, point) = MeshBuilder.BuildFunctionFigure((int)numericUpDown28.Value, (int)numericUpDown29.Value,
                (int)numericUpDown27.Value, (int)numericUpDown26.Value, (int)numericUpDown30.Value, func);
            
            figure = new SceneObject(mesh);
            figure.Name = "x+y";
            figure.Transform.Translate(-1 * point);
            scene.AddObject(figure);

            UpdateHierarchy();
            Render();
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
            //Ничего не делает, выключил от греха подальше
            //figure.RotateAroundAxis(angle, Axis.CUSTOM, line_1);


            Render();
        }

        private void hexahedronToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Mesh mesh = new Mesh();
            mesh = MeshBuilder.make_hexahedron();
            figure = new SceneObject(mesh);
            figure.Name = "Гексаэдр";
            scene.AddObject(figure);
            UpdateHierarchy();

            Render();
        }

        private void tetrahedronToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Mesh mesh = new Mesh();
            mesh.make_tetrahedron();
            figure = new SceneObject(mesh);
            figure.Name = "Тетраэдр";
            scene.AddObject(figure);
            UpdateHierarchy();

            Render();
        }

        private void octahedronToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Mesh mesh = new Mesh();
            mesh.make_octahedron();
            figure = new SceneObject(mesh);
            figure.Name = "Октаэдр";
            scene.AddObject(figure);
            UpdateHierarchy();

            Render();
        }

        private void icosahedronToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Mesh mesh = new Mesh();
            mesh.make_icosahedron();
            figure = new SceneObject(mesh);
            figure.Name = "Икосаэдр";
            scene.AddObject(figure);
            UpdateHierarchy();

            Render();
        }

        private void dodecahedronToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Mesh mesh = new Mesh();
            mesh.make_dodecahedron();
            figure = new SceneObject(mesh);
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
                        scene.MoveCamera(new Point3D(-0.5f, 0, 0));
                        break;
                    }
                case 'd':
                    {
                        scene.MoveCamera(new Point3D(0.5f, 0, 0));
                        break;
                    }
                case 'w':
                    {
                        scene.MoveCamera(new Point3D(0, 0, 0.5f));
                        break;
                    }
                case 's':
                    {
                        scene.MoveCamera(new Point3D(0, 0, -0.5f));
                        break;
                    }
                case 'z':
                    {
                        scene.MoveCamera(new Point3D(0, -0.5f, 0));
                        break;
                    }
                case 'x':
                    {
                        scene.MoveCamera(new Point3D(0, 0.5f, 0));
                        break;
                    }
                case 'l':
                    {
                        scene.RotateCamera(new Point3D(-0.5f, 0, 0));
                        break;
                    }
                case 'o':
                    {
                        scene.RotateCamera(new Point3D(0.5f, 0, 0));
                        break;
                    }
                case ',':
                    {
                        scene.RotateCamera(new Point3D(0, 0, 0.5f));
                        break;
                    }
                case '.':
                    {
                        scene.RotateCamera(new Point3D(0, 0, -0.5f));
                        break;
                    }
                case ';':
                    {
                        scene.RotateCamera(new Point3D(0, -0.5f, 0));
                        break;
                    }
                case 'k':
                    {
                        scene.RotateCamera(new Point3D(0, 0.5f, 0));
                        break;
                    }


            }
            Render();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(figure.Local is Mesh))
                return;
            //TODO
            var path = figure.Name + ".stl";
            

            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Title = "Save as...";
            sfd.CheckPathExists = true;
            sfd.Filter = "STL Files(*.stl)|*.stl";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    MeshBuilder.SaveToFile(sfd.FileName, (Mesh)figure.GetTransformed(), figure.Name);
                }
                catch
                {
                    MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void comboBoxRenderMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            g = Graphics.FromImage(pictureBox1.Image);
            g.TranslateTransform(pictureBox1.ClientSize.Width / 2, pictureBox1.ClientSize.Height / 2);
            g.ScaleTransform(1, -1);
        }

        private void buttonFloatingHorizon_Click(object sender, EventArgs e)
        {
            floatingHorizon.Show();
        }
    }
}
