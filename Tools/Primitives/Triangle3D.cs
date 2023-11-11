using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Primitives
{
    public class Triangle3D: Primitive
    {

        public bool isVisible = true;
        private Point3D[] points;

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
                //if (value is null)
                //    throw new ArgumentNullException();
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
        }

        public Triangle3D(Point3D p1, Point3D p2, Point3D p3)
        {
            Initializze(new Point3D[3] { p1, p2, p3 });
        }

        public Triangle3D(ICollection<Point3D> p)
        {
            Initializze(p);
        }

        public override Primitive Clone()
        {
            var list = new List<Point3D>();
            foreach (Point3D p in points)
            {
                list.Add(p);
            }
            return new Triangle3D(list);
        }

        public void reflectX()
        {
            for (int i = 0; i < points.Length; i++)
                points[i].ReflectX();
        }
        public void reflectY()
        {
            for (int i = 0; i < points.Length; i++)
                points[i].ReflectY();
        }
        public void reflectZ()
        {
            for (int i = 0; i < points.Length; i++)
                points[i].ReflectZ();
        }

        public override void RotateAroundAxis(double angle, Axis a, Edge3D line = null)
        {
            for (int i = 0; i < points.Length; i++)
                points[i].Rotate(angle, a, line);
        }

        public override void Translate(Point3D vec)
        {
            for (int i = 0; i < points.Length; i++)
                points[i].Translate(vec.X, vec.Y, vec.Z);
        }

        public override void Rotate(Point3D vec)
        {
            for (int i = 0; i < points.Length; i++)
            { 
                points[i].Rotate(vec.X, Axis.AXIS_X);
                points[i].Rotate(vec.Y, Axis.AXIS_Y);
                points[i].Rotate(vec.Z, Axis.AXIS_Z);
            }
        }

        public override void Scale(Point3D vec)
        {
            for (int i = 0; i < points.Length; i++)
                points[i].Scale(vec.X, vec.Y, vec.Z);
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

        public List<PointF> GetPerspective(Scene.Camera camera)
        {
            List<PointF> res = new List<PointF>();

            foreach (Point3D p in points)
            {
                res.Add(p.GetPerspective(camera));
            }
            return res;
        }

        public override void Draw(Graphics g, Scene.Camera camera, Projection pr = 0, Pen pen = null)
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
                    pts = GetPerspective(camera);
                    break;
            }

            
            g.DrawLines(pen, pts.ToArray());
            g.DrawLine(pen, pts[0], pts[pts.Count - 1]);
        }
    }
}
