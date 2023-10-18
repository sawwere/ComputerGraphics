using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Primitives
{
    public class Triangle3D
    {

        public bool isVisible = true;
        private Point3D[] points;

        private Edge3D boundingBox; // TODO

        /// <summary>
        /// Нормаль данного треугольника
        /// </summary>
        public Point3D Norm
        {
            get 
            {
                Point3D a = points[1] - points[0];
                Point3D n = a.CrossProduct(points[2] - points[0]);
                return (1.0f / n.Length) * n;
            }
        }
        public Point3D this[int i]
        {
            get { return points[i]; }
            private set
            {
                if (value is null)
                    throw new ArgumentNullException();
                points[i] = value;
            }
        }

        public Point3D Center
        {
            get
            {
                return new Point3D(points.Sum(p=>p.X) / 3, points.Sum(p => p.Y) / 3, points.Sum(p => p.Z) / 3);
            }
        }

        private void Initializze(ICollection<Point3D> ps)
        {
            points = new Point3D[3];
            for (int i = 0; i < 3; i++)
                points[i] = ps.ElementAt(i);
            boundingBox = new Edge3D(new Point3D(0,0,0), new Point3D(0, 0, 0), Color.Black);
            boundingBox.Origin.X = points.Min(p => p.X);
            boundingBox.Origin.Y = points.Min(p => p.Y);
            boundingBox.Origin.Z = points.Min(p => p.Z);

            boundingBox.Destination.X = points.Max(p => p.X);
            boundingBox.Destination.Y = points.Max(p => p.Y);
            boundingBox.Destination.Z = points.Max(p => p.Z);
        }

        public Triangle3D(Point3D p1, Point3D p2, Point3D p3)
        {
            Initializze(new Point3D[3] { p1, p2, p3 });
        }

        public Triangle3D(ICollection<Point3D> p)
        {
            Initializze(p);
        }

        public void reflectX()
        {
            Center.X = -Center.X;
            if (points != null)
                foreach (var p in points)
                    p.ReflectX();
        }
        public void reflectY()
        {
            Center.Y = -Center.Y;
            if (points != null)
                foreach (var p in points)
                    p.ReflectY();
        }
        public void reflectZ()
        {
            Center.Z = -Center.Z;
            if (points != null)
                foreach (var p in points)
                    p.ReflectZ();
        }

        public void Translate(float x, float y, float z)
        {
            foreach (Point3D p in points)
                p.Translate(x, y, z);
        }

        public void Rotate(double angle, Axis a, Edge3D line = null)
        {
            foreach (Point3D p in points)
                p.Rotate(angle, a, line);
        }

        public void Scale(float kx, float ky, float kz)
        {
            foreach (Point3D p in points)
                p.Scale(kx, ky, kz);
        }

        public List<PointF> GetIsometric()
        {
            List<PointF> res = new List<PointF>();

            foreach (Point3D p in points)
                res.Add(p.GetIsometric());

            return res;
        }

        public List<PointF> GetOrthographic(Axis a)
        {
            List<PointF> res = new List<PointF>();

            foreach (Point3D p in points)
                res.Add(p.GetOrthographic(a));

            return res;
        }

        public List<PointF> GetPerspective(float k = 1000)
        {
            List<PointF> res = new List<PointF>();

            foreach (Point3D p in points)
            {
                res.Add(p.GetPerspective(k));
            }
            return res;
        }

        public void Draw(Graphics g, Projection pr = 0, Pen pen = null)
        {
            if (pen == null)
                pen = Pens.Black;

            List<PointF> pts = new List<PointF>();

            switch (pr)
            {
                case Projection.ISOMETRIC:
                    pts = GetIsometric();
                    break;
                case Projection.ORTHOGR_X:
                    pts = GetOrthographic(Axis.AXIS_X);
                    break;
                case Projection.ORTHOGR_Y:
                    pts = GetOrthographic(Axis.AXIS_Y);
                    break;
                case Projection.ORTHOGR_Z:
                    pts = GetOrthographic(Axis.AXIS_Z);
                    break;
                default:
                    pts = GetPerspective(1000);
                    break;
            }

            
            g.DrawLines(pen, pts.ToArray());
            g.DrawLine(pen, pts[0], pts[pts.Count - 1]);
        }
    }
}
