using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace IndTask2
{
    public partial class MainForm : Form
    {
        public enum Axis { AXIS_X, AXIS_Y, AXIS_Z };
        public enum MaterialType { WALL, MIRROR, TRANSPARENT };

        public List<Mesh> scene = new List<Mesh>();

        public List<Light> lights = new List<Light>();   // список источников света
        public const float ROOM_SIZE = 1f;
        public Random rand = new Random();

        public MainForm()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Render();
        }

        public void Clear()
        {
            scene.Clear();
            lights.Clear();
        }

        public void Render()
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            var cameraPos = new Vector3(0, 0, 1);
            stopWatch.Start();

            var bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            Color[] buffer = new Color[pictureBox1.Width * pictureBox1.Height];
            {
                Parallel.For(0, pictureBox1.Height, y =>
                {
                    for (int x = 0; x < pictureBox1.Width; x++)
                    {
                        var xx = Interpolate(0, -1, pictureBox1.Width, 1, x);
                        var yy = Interpolate(0, 1, pictureBox1.Height, -1, y);
                        var pixel = new Vector3(xx, yy, 0);

                        Ray r = new Ray(cameraPos.Clone(), pixel);
                        Vector3 clr = BackwardRayTrace(r, 4);
                        if (clr.X > 1.0f || clr.Y > 1.0f || clr.Z > 1.0f)
                        {
                            clr = clr.Normalize();
                        }
                        buffer[x + y * pictureBox1.Width] = Color.FromArgb((int)(255 * clr.X), (int)(255 * clr.Y), (int)(255 * clr.Z));
                    }
                });
            }

            using (var fb = new FastBitmap.FastBitmap(bitmap))
            {

                for (int y = 0; y < pictureBox1.Height; y++)
                {
                    for (int x = 0; x < pictureBox1.Width; x++)
                    {
                        fb[x, y] = buffer[x + y * pictureBox1.Width];
                    }
                }
            }
            pictureBox1.Image = bitmap;
            pictureBox1.Refresh();


            labelFPS.Text = $"FPS: {(1000.0f / stopWatch.ElapsedMilliseconds)}";
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Width <= 0 || pictureBox1.Height <= 0)
                return;
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Render();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clear();
            BuildScene();
            Render();
        }

        public void BuildScene()
        {
            Light l1 = new Light(new Vector3(0f, 0.98f, -0.0f), new Vector3(1f, 1f, 1f));
            lights.Add(l1);
            if (checkBoxLight.Checked)
            {
                float x = ((float)numericUpDownX.Value / 100);
                float y = ((float)numericUpDownY.Value / 100);
                float z = ((float)numericUpDownZ.Value / 100);
                Light l2 = new Light(new Vector3(x, y, z), new Vector3(1f, 1f, 1f));
                lights.Add(l2);
            }


            var sb = new SceneBuilder(ROOM_SIZE);
            sb.AddForwardWall(Color.Pink, checkedListBox1.CheckedIndices.Contains(0));

            sb.AddBackWall(Color.Wheat, checkedListBox1.CheckedIndices.Contains(1));

            sb.AddLeftWall(Color.Red, checkedListBox1.CheckedIndices.Contains(2));

            sb.AddRightWall(Color.Aqua, checkedListBox1.CheckedIndices.Contains(3));

            sb.AddUpperWall(Color.White, checkedListBox1.CheckedIndices.Contains(4));

            sb.AddBottomWall(Color.Yellow, checkedListBox1.CheckedIndices.Contains(5));



            sb.AddCube(ROOM_SIZE / 2.6f, Color.Blue,
                cubeMirror.Checked ? MaterialType.MIRROR : MaterialType.WALL,
                new Vector3(ROOM_SIZE / 2, -ROOM_SIZE / 1.5f, -ROOM_SIZE / 1.5f),
                new Vector3(0, 36, 0),
                new Vector3(1, 1, 1)
                );

            sb.AddCube(0.2f, Color.Orange,
                cubeTransparent.Checked ? MaterialType.TRANSPARENT : MaterialType.WALL,
                new Vector3(-ROOM_SIZE / 2f, -ROOM_SIZE / 1.8f, -ROOM_SIZE / 2f),
                new Vector3(0, 0, 0),
                new Vector3(1.43f, 3, 1f)
                );

            sb.AddSphere(new Vector3(-ROOM_SIZE / 2f, ROOM_SIZE / 2f, -ROOM_SIZE / 2f),
                0.4f,
                Color.RosyBrown,
                sphereMirror.Checked ? MaterialType.MIRROR : MaterialType.WALL
                );

            sb.AddSphere(new Vector3(ROOM_SIZE / 2f, ROOM_SIZE / 2f, -ROOM_SIZE / 2f),
                0.4f,
                Color.DarkOrange,
                sphereTransparent.Checked ? MaterialType.TRANSPARENT : MaterialType.WALL
                );

            foreach (var m in sb.Get())
                scene.Add(m);
        }

        // Видима ли точка пересечения луча с фигурой из источника света
        public bool isVisible(Vector3 light_point, Vector3 hit_point)
        {
            float max_t = (light_point - hit_point).Length;
            Ray r = new Ray(hit_point, light_point);

            foreach (var fig in scene)
                if (fig.Intersection(r, out float t, out Vector3 n))
                    if (t < max_t && t > 1e-4f)
                        return false;
            return true;
        }

        float Interpolate(float x0, float y0, float x1, float y1, float i)
        {
            if (Math.Abs(x0 - x1) < 1e-8)
                return (y0 + y1) / 2;
            return y0 + ((y1 - y0) * (i - x0)) / (x1 - x0);
        }

        public Vector3 BackwardRayTrace(Ray r, int iter)
        {
            float t = 0;
            bool refract_out_of_figure = false;
            Vector3 normal = null;
            //вообще говоря материал изменится, если что-то найдем
            Material m = Material.Mirror();
            Vector3 res = new Vector3(0, 0, 0);
            //ищем ближайшую по направлению луча фигуру и ее материал
            foreach (var mesh in scene)
            {
                if (mesh.Intersection(r, out float intersect, out Vector3 n))
                {
                    if (intersect < t || t == 0)
                    {
                        t = intersect;
                        normal = n;
                        m = mesh.material.Clone();
                    }
                }
            }

            if (t == 0 || iter < 0)
            {
                return new Vector3(0.0f, 0.0f, 0.0f);
            }

            if (r.dest.DotProduct(normal) > 0)
            {
                normal *= -1;
                refract_out_of_figure = true;
            }
            //непосредственно точка пересечения луча и фигуры
            Vector3 hit_point = r.origin + r.dest * t;

            foreach (Light l in lights)
            {
                Vector3 amb = l.color * m.ambient;
                amb.X *= m.color.X;
                amb.Y *= m.color.Y;
                amb.Z *= m.color.Z;
                res += amb;

                if (isVisible(l.position, hit_point))
                    res += l.Shading(hit_point, normal, m.color, m.diffuse);
            }

            if (m.reflection > 0)
            {
                Vector3 reflectDir = r.dest - 2 * normal * r.dest.DotProduct(normal);
                var reflected = new Ray(hit_point, hit_point + reflectDir);
                // UGA BUGA??
                Vector3 reflectDir2 = r.dest - (2 + 0.1f * ((float)rand.NextDouble() - 0.5f)) * normal * r.dest.DotProduct(normal);
                var reflected2 = reflected = new Ray(hit_point, hit_point + reflectDir2);
                //reflected2.dest = new Vector3(
                //    reflected2.dest.X + (float)(rand.NextDouble() - 0.5) * 1e-6f,
                //    reflected2.dest.Y + (float)(rand.NextDouble() - 0.5) * 1e-6f,
                //    reflected2.dest.Z + (float)(rand.NextDouble() - 0.5) * 1e-6f);
                res += m.reflection * BackwardRayTrace(reflected, iter - 1);
                res += m.reflection * BackwardRayTrace(reflected2, iter-1) * 1e-1f;
            }

            if (m.refraction > 0)
            {
                float eta;
                if (refract_out_of_figure)
                    eta = m.environment;
                else
                    eta = 1 / m.environment;
                hit_point = r.origin + r.dest * (t + 1e-4f);
                Ray refracted_ray = r.Refract(hit_point, normal, eta);
                if (refracted_ray != null)
                    res += m.refraction * BackwardRayTrace(refracted_ray, iter - 1);
            }

            return res;
        }
    }

}
