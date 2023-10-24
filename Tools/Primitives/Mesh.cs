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
        public void make_tetrahedron(float size = 100)
        {
            // Point3D A11 = new Point3D(0,0,0);//C = new Point3D(0.5*size,(float)System.Math.Sqrt(3)/2,0),
            // Point3D D11 = new Point3D((float)0.5*size, (float)System.Math.Sqrt(3)*size/6, (float)System.Math.Sqrt(2) * size / 6);
            // Point3D B11 = new Point3D(size, 0, 0);
            polygons = new List<Triangle3D>
            {
                new Triangle3D(new Point3D(0,0,0),  new Point3D((float)0.5*size, (float)System.Math.Sqrt(3)*size/6, (float)System.Math.Sqrt(2) * size / 6), new Point3D(size, 0, 0)),
                new Triangle3D(new Point3D(0,0,0),  new Point3D((float)0.5*size, (float)System.Math.Sqrt(3)*size/6, (float)System.Math.Sqrt(2) * size / 6), new Point3D((float)0.5*size,(float)System.Math.Sqrt(3)*size/2,0)),
                new Triangle3D(new Point3D((float)0.5*size,(float)System.Math.Sqrt(3)*size/2,0), new Point3D((float)0.5*size, (float)System.Math.Sqrt(3)*size/6, (float)System.Math.Sqrt(2) * size / 6), new Point3D(size, 0, 0)),
                new Triangle3D(new Point3D(0,0,0), new Point3D((float)0.5*size,(float)System.Math.Sqrt(3)*size/2,0), new Point3D(size, 0, 0)),
            };
        }

        public void make_octahedron(float size = 50)
        {
            //Point3D A=new Point3D(0,size,0),
            //        B=new Point3D(size,0,0),
            //        C=new Point3D(0,-size,0),
            //        D=new Point3D(-size,0,0),
            //        E=new Point3D(0,0,size),
            //        F=new Point3D(0,0,-size);
            polygons = new List<Triangle3D>
            {
                new Triangle3D(new Point3D(0,size,0),new Point3D(0,0,size),new Point3D(size,0,0)),
                new Triangle3D(new Point3D(0,size,0),new Point3D(0,0,size),new Point3D(-size,0,0)),
                new Triangle3D(new Point3D(0,-size,0),new Point3D(0,0,size),new Point3D(-size,0,0)),
                new Triangle3D(new Point3D(0,-size,0),new Point3D(0,0,size),new Point3D(size,0,0)),
                new Triangle3D(new Point3D(0,size,0),new Point3D(0,0,-size),new Point3D(size,0,0)),
                new Triangle3D(new Point3D(0,size,0),new Point3D(0,0,-size),new Point3D(-size,0,0)),
                new Triangle3D(new Point3D(0,-size,0),new Point3D(0,0,-size),new Point3D(-size,0,0)),
                new Triangle3D(new Point3D(0,-size,0),new Point3D(0,0,-size),new Point3D(size,0,0))
            };
        }
        public void make_icosahedron(float size = 50)
        {
            float p =(float) (1 + System.Math.Sqrt(5)) / 2;//phi
            polygons = new List<Triangle3D>
            {
                new Triangle3D(new Point3D(-p*size,0,size),new Point3D(0,-size,p*size),new Point3D(0,size,p*size)),//1
                new Triangle3D(new Point3D(0,-size,p*size),new Point3D(0,size,p*size),new Point3D(p*size,0,size)),//2
                new Triangle3D(new Point3D(0,-size,p*size),new Point3D(-size,-p*size,0),new Point3D(size,-p*size,0)),//3
                new Triangle3D(new Point3D(0,-size,p*size),new Point3D(-size,-p*size,0),new Point3D(p*size,0,size)),//4
                new Triangle3D(new Point3D(p*size,0,size),new Point3D(0,size,p*size),new Point3D(size,p*size,0)),//5
                new Triangle3D(new Point3D(size,p*size,0),new Point3D(0,size,p*size),new Point3D(-size,p*size,0)),//6
                new Triangle3D(new Point3D(0,size,p*size),new Point3D(-p*size,0,size),new Point3D(-size,p*size,0)),//7
                new Triangle3D(new Point3D(-size,p*size,0),new Point3D(0,-size,p*size),new Point3D(-size,-p*size,0)),//8
                new Triangle3D(new Point3D(size,-p*size,0),new Point3D(p*size,0,size),new Point3D(p*size,0,-size)),//9
                new Triangle3D(new Point3D(p*size,0,size),new Point3D(size,p*size,0),new Point3D(p*size,0,-size)),//10
                new Triangle3D(new Point3D(-size,p*size,0),new Point3D(-p*size,0,size),new Point3D(-p*size,0,-size)),//11
                new Triangle3D(new Point3D(-p*size,0,size),new Point3D(-p*size,0,-size),new Point3D(-size,-p*size,0)),//12
                new Triangle3D(new Point3D(-size,-p*size,0),new Point3D(size,-p*size,0),new Point3D(0,-size,-p*size)),//13
                new Triangle3D(new Point3D(size,-p*size,0),new Point3D(0,-size,-p*size),new Point3D(p*size,0,-size)),//14
                new Triangle3D(new Point3D(p*size,0,-size),new Point3D(size,p*size,0),new Point3D(0,size,-p*size)),//15
                new Triangle3D(new Point3D(size,p*size,0),new Point3D(-size,p*size,0),new Point3D(0,size,-p*size)),//16
                new Triangle3D(new Point3D(-size,p*size,0),new Point3D(0,size,-p*size),new Point3D(-p*size,0,-size)),//17
                new Triangle3D(new Point3D(-p*size,0,-size),new Point3D(-size,-p*size,0),new Point3D(0,-size,-p*size)),//18
                new Triangle3D(new Point3D(-p*size,0,-size),new Point3D(0,-size,-p*size),new Point3D(0,size,-p*size)),//19
                new Triangle3D(new Point3D(0,-size,-p*size),new Point3D(p*size,0,-size),new Point3D(0,size,-p*size)),//20

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
        public void reflectX()
        {
            if (polygons != null)
                foreach (var f in polygons)
                    f.reflectX();
            UpdateCenter();
        }

        public void reflectY()
        {
            if (polygons != null)
                foreach (var f in polygons)
                    f.reflectY();
            UpdateCenter();
        }

        public void reflectZ()
        {
            if (polygons != null)
                foreach (var f in polygons)
                    f.reflectZ();
            UpdateCenter();
        }
    }
}
