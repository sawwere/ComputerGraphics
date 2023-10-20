using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace Tools.Primitives
{
    public class Mesh
    {
        private List<Triangle3D> polygons;

        public Point3D Center
        {
            get;
            private set;
        }


        public Mesh(List<Triangle3D> list = null)
        {
            polygons = new List<Triangle3D>();
            Center = new Point3D(0, 0, 0);
            if (list != null)
            {
                polygons.AddRange(list);
            }
        }

        private void UpdateCenter()
        {
            Center.X = 0;
            Center.Y = 0;
            Center.Z = 0;
            foreach (Triangle3D f in polygons)
            {
                Center.X += f.Center.X;
                Center.Y += f.Center.Y;
                Center.Z += f.Center.Z;
            }
            Center.X /= polygons.Count;
            Center.Y /= polygons.Count;
            Center.Z /= polygons.Count;
        }

        public void Translate(float x, float y, float z)
        {
            foreach (Triangle3D f in polygons)
                f.Translate(x, y, z);
            UpdateCenter();
        }

        public void Rotate(double angle, Axis a, Edge3D line = null)
        {
            foreach (Triangle3D f in polygons)
                f.Rotate(angle, a, line);
            UpdateCenter();
        }

        public void Scale(float kx, float ky, float kz)
        {
            foreach (Triangle3D f in polygons)
                f.Scale(kx, ky, kz);
            UpdateCenter();
        }

        public void make_hexahedron(float size = 50)
        {
            polygons = new List<Triangle3D> {
                new Triangle3D( new Point3D(size, -size, size), new Point3D(size, -size, -size), new Point3D(size, size, -size)),
                new Triangle3D( new Point3D(size, -size, size), new Point3D(size, size, -size), new Point3D(size, size, size)),

                new Triangle3D( new Point3D(-size, size, size), new Point3D(-size, size, -size), new Point3D(-size, -size, -size)),
                new Triangle3D( new Point3D(-size, size, size), new Point3D(-size, -size, -size), new Point3D(-size, -size, size)),

                new Triangle3D( new Point3D(size, size, size), new Point3D(-size, size, size), new Point3D(-size, -size, size)),
                new Triangle3D( new Point3D(size, size, size), new Point3D(-size, -size, size), new Point3D(size, -size, size)),

                new Triangle3D( new Point3D(size, size, size), new Point3D(size, size, size), new Point3D(size, size, -size)),
                new Triangle3D( new Point3D(size, size, size), new Point3D(-size, size, -size), new Point3D(-size, size, size)),

                new Triangle3D( new Point3D(size, -size, -size), new Point3D(-size, -size, -size), new Point3D(-size, size, -size)),
                new Triangle3D( new Point3D(size, -size, -size), new Point3D(-size, size, -size), new Point3D(size, size, -size)),

                new Triangle3D( new Point3D(-size, -size, size), new Point3D(-size, -size, -size), new Point3D(size, -size, -size)),
                new Triangle3D( new Point3D(-size, -size, size), new Point3D(size, -size, -size), new Point3D(size, -size, size))
            };
        }

        public void Draw(Graphics g, Projection pr = 0, Pen pen = null)
        {
            foreach (Triangle3D t in polygons)
            {
                if (t.isVisible)
                    t.Draw(g, pr, pen);
            }
        }
    }
}
