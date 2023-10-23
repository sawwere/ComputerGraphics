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
        public struct MeshInfo
        {
            public string name;
            public Mesh mesh;
            public float dx;
            public float dy;
            public float dz;

            public float rx;
            public float ry;
            public float rz;

            public float sx;
            public float sy;
            public float sz;

            public MeshInfo(string name, Mesh mesh)
            {
                this.name = name;
                this.mesh = mesh;
                this.dx = 0;
                this.dy = 0;
                this.dz = 0;
                this.rx = 0;
                this.ry = 0;
                this.rz = 0;
                this.sx = 100;
                this.sy = 100;
                this.sz = 100;
            }

            public MeshInfo(string name, Mesh mesh, float dx, float dy, float dz, float rx, float ry, float rz, float sx, float sy, float sz)
            {
                this.name = name;
                this.mesh = mesh;
                this.dx = dx;
                this.dy = dy;
                this.dz = dz;
                this.rx = rx;
                this.ry = ry;
                this.rz = rz;
                this.sx = sx;
                this.sy = sy;
                this.sz = sz;
            }
        }

        private FormRotationFigure rotationFigure = new FormRotationFigure();

        Graphics g;
        Projection projection = Projection.ORTHOGR_X;
        Edge3D axisLineX;
        Edge3D axisLineY;
        Edge3D axisLineZ;

        private float startAxisValue = 0;
        private float deltaAxis = 0;
        private enum DeltaAxis { X, Y, Z, None };
        private DeltaAxis curDeltaAxis = DeltaAxis.None;

        private Dictionary<string, MeshInfo> sceneObjects;

        Mesh figure = null;

        private void radioButtonScene_CheckedChanged(object sender, EventArgs e)
        {
            figure = sceneObjects[((RadioButton)sender).Name].mesh;
            UpdateInspector();
        }

        private void AddToHierarchy(string name)
        {
            RadioButton rb = new RadioButton();
            rb.Name = name;
            rb.Checked = false;
            rb.Text = rb.Name;
            rb.Width = panelSceneHierarchy.Width - 40;
            rb.Height = 30;
            rb.Location = new Point(15, sceneObjects.Count * (rb.Height));
            rb.CheckedChanged += radioButtonScene_CheckedChanged;
            foreach (Control r in panelSceneHierarchy.Controls)
            {
                if ( r is RadioButton)
                    ((RadioButton)r).Checked = false;
            }
            rb.Checked = true;
            panelSceneHierarchy.Controls.Add(rb);
        }

        public MainForm()
        {
            InitializeComponent();
            AddOwnedForm(rotationFigure);

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.TranslateTransform(pictureBox1.ClientSize.Width / 2, pictureBox1.ClientSize.Height / 2);
            g.ScaleTransform(1, -1);

            Mesh def = Tools.Meshes.MeshBuilder.LoadFromFile(@"..//..//..//Tools//Meshes//Гексаэдр.stl");
            sceneObjects = new Dictionary<string, MeshInfo>();
            sceneObjects["Гексаэдр"] = new MeshInfo("Гексаэдр", def);
            figure = sceneObjects.First().Value.mesh;
            AddToHierarchy("Гексаэдр");
            


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
                        figure = Tools.Meshes.MeshBuilder.LoadFromFile(filename);

                        StringBuilder figureName = new StringBuilder();
                        figureName.Append( filename.Split('\\').Last().Split('.').First());
                        int i = 1;
                        while (sceneObjects.ContainsKey(figureName.ToString()))
                        {
                            figureName.Append(i);
                            i++;
                        }

                        sceneObjects[figureName.ToString()] = new MeshInfo(figureName.ToString(), figure);
                        AddToHierarchy(figureName.ToString());

                        groupBoxInspector.Text = $"Объект: {figureName}";
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
            foreach (var kv in sceneObjects)
            {
                kv.Value.mesh.Draw(g, projection);
            }
            
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
            AddMeshToScene();
            
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
            float o_x = figure.Center.X;
            float o_y = figure.Center.Y;
            float o_z = figure.Center.Z;
            figure.Translate(-o_x, -o_y, -o_z);
            figure.Scale(kx, ky, kz);
            figure.Translate(o_x, o_y, o_z);

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
        figure.reflectZ();
        Render();
        }

        private void buttonReflectX_Click(object sender, EventArgs e)
        {
        figure.reflectX();
        Render();
        }

        private void buttonReflectY_Click(object sender, EventArgs e)
        {
        figure.reflectY();
        Render();
        }

            private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            
            var p = new Point2D(e.X - pictureBox1.Width / 2, -(e.Y - pictureBox1.Height / 2));
            var prx = axisLineX.ProjectedEdge(projection);
            Console.WriteLine($"{prx.Point1.X} {prx.Point1.Y} {prx.Point2.X} {prx.Point2.Y}");
            if (prx.Length > 0.1f  && Math.Abs(p.CompareToEdge2(prx)) < 2000)
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

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetHierarchy();
            AddMeshToScene();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddMeshToScene();
        }

        private void buttonRotateFigure_Click(object sender, EventArgs e)
        {
            if (rotationFigure.ShowDialog() == DialogResult.OK)
            {
                var list = rotationFigure.GetPoints();
                foreach (var obj in list)
                    Console.WriteLine(obj.X);
            }
        }

        private void ResetHierarchy()
        {
            sceneObjects = new Dictionary<string, MeshInfo>();
            figure = new Mesh();
            panelSceneHierarchy.Controls.Clear();
            var lab = new Label();
            lab.Text = "Сцена";
            panelSceneHierarchy.Controls.Add(lab);
        }

        private void hexahedronToolStripMenuItem_Click(object sender, EventArgs e)
        {
            figure = new Mesh();
            figure.make_hexahedron();
            StringBuilder figureName = new StringBuilder("Гексаэдр");
            int i = 1;
            while (sceneObjects.ContainsKey(figureName.ToString()))
            {
                figureName.Append(i);
                i++;
            }
            sceneObjects[figureName.ToString()] = new MeshInfo(figureName.ToString(), figure);
            AddToHierarchy(figureName.ToString());

            groupBoxInspector.Text = $"Объект: {figureName}";

            Render();
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
    }
}
