using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tools.Primitives
{
    public class Edge3D
    {
        public Point3D Origin { get; set; }
        public Point3D Destination { get; set; }

        public float X { get { return Destination.X; } }
        public float Y { get { return Destination.Y; } }
        public float Z { get { return Destination.Z; } }

        public Color Color { get; set; }

        public Edge2D ProjectedEdge(Projection pr, Scene.Camera camera)
        {
            var p1 = new PointF(0, 0);
            var p2 = new PointF(0, 0);
            switch (pr)
            {
                case Projection.ISOMETRIC:
                    p1 = Origin.GetIsometric();
                    p2 = Destination.GetIsometric();
                    break;
                case Projection.ORTHOGR_X:
                    p1 = Origin.GetOrthographic(Axis.AXIS_X);
                    p2 = Destination.GetOrthographic(Axis.AXIS_X);
                    break;
                case Projection.ORTHOGR_Y:
                    p1 = Origin.GetOrthographic(Axis.AXIS_Y);
                    p2 = Destination.GetOrthographic(Axis.AXIS_Y);
                    break;
                case Projection.ORTHOGR_Z:
                    p1 = Origin.GetOrthographic(Axis.AXIS_Z);
                    p2 = Destination.GetOrthographic(Axis.AXIS_Z);
                    break;
                default:
                    p1 = Origin.GetPerspective(camera);
                    p2 = Destination.GetPerspective(camera);
                    break;
            }
            return new Edge2D(new Point2D(p1), new Point2D(p2));
        }

        public Edge3D(Point3D org, Point3D dest, Color color)
        {
            Origin = org;
            Destination = dest;
            Color = color;
        }

        public int Intesect(Triangle3D triangle, ref float t)
        {
            return 0;
        }

        /// <summary>
        /// Точка на текущей прямой, соответствующая параметрическому значению t
        /// </summary>
        public Point3D PointOnEdge(float t)
        {
            return Origin + t * (Destination - Origin);
        }

        public void Draw(Graphics g, Scene.Camera camera, Projection pr)
        {
            var p1 = new PointF(0, 0);
            var p2 = new PointF(0, 0);
            switch (pr)
            {
                case Projection.ISOMETRIC:
                    p1 = Origin.GetIsometric();
                    p2 = Destination.GetIsometric();
                    break;
                case Projection.ORTHOGR_X:
                    p1 = Origin.GetOrthographic(Axis.AXIS_X);
                    p2 = Destination.GetOrthographic(Axis.AXIS_X);
                    break;
                case Projection.ORTHOGR_Y:
                    p1 = Origin.GetOrthographic(Axis.AXIS_Y);
                    p2 = Destination.GetOrthographic(Axis.AXIS_Y);
                    break;
                case Projection.ORTHOGR_Z:
                    p1 = Origin.GetOrthographic(Axis.AXIS_Z);
                    p2 = Destination.GetOrthographic(Axis.AXIS_Z);
                    break;
                default:
                    p1 = Origin.GetPerspective(camera);
                    p2 = Destination.GetPerspective(camera);
                    break;
            }
            Console.WriteLine($"{p1.X} {p1.Y} {p2.X} {p2.Y}");
            g.DrawLine(new Pen(Color, 2), p1, p2);
        }
    }
}
