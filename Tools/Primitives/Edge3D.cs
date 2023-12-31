﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tools.Primitives
{
    public class Edge3D: Primitive
    {
        public Point3D Origin;
        public Point3D Destination;

        public Color Color { get; set; }

        public Edge2D ProjectedEdge(Projection pr, Scene.Camera camera)
        {
            var p1 = new PointF(0, 0);
            var p2 = new PointF(0, 0);
            switch (pr)
            {
                case Projection.ISOMETRIC:
                    p1 = Origin.GetIsometricProj();
                    p2 = Destination.GetIsometricProj();
                    break;
                case Projection.ORTHOGR_X:
                    p1 = Origin.GetOrthographicProj(Axis.AXIS_X);
                    p2 = Destination.GetOrthographicProj(Axis.AXIS_X);
                    break;
                case Projection.ORTHOGR_Y:
                    p1 = Origin.GetOrthographicProj(Axis.AXIS_Y);
                    p2 = Destination.GetOrthographicProj(Axis.AXIS_Y);
                    break;
                case Projection.ORTHOGR_Z:
                    p1 = Origin.GetOrthographicProj(Axis.AXIS_Z);
                    p2 = Destination.GetOrthographicProj(Axis.AXIS_Z);
                    break;
                default:
                    p1 = Origin.GetPerspectiveProj(camera);
                    p2 = Destination.GetPerspectiveProj(camera);
                    break;
            }
            //Console.WriteLine($"{p1.X} {p1.Y} {p2.X} {p2.Y}");
            return new Edge2D(new Point2D(p1), new Point2D(p2));
        }

        public Edge3D(Point3D org, Point3D dest)
        {
            Origin = org;
            Destination = dest;
            Color = Color.Black;
        }

        public Edge3D(Point3D org, Point3D dest, Color color)
        {
            Origin = org;
            Destination = dest;
            Color = color;
        }

        public override Primitive Clone()
        {
            var no = new Point3D(Origin.X, Origin.Y, Origin.Z);
            var nd = new Point3D(Destination.X, Destination.Y, Destination.Z);
            Edge3D res = new Edge3D(no, nd, Color);
            return res;
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

        public override void Translate(Point3D vec)
        {
            Origin.Translate(vec.X, vec.Y, vec.Z);
            Destination.Translate(vec.X, vec.Y, vec.Z);
        }

        public override void Rotate(Point3D vec)
        {
            Origin.Rotate(vec.X, Axis.AXIS_X);
            Origin.Rotate(vec.Y, Axis.AXIS_Y);
            Origin.Rotate(vec.Z, Axis.AXIS_Z);

            Destination.Rotate(vec.X, Axis.AXIS_X);
            Destination.Rotate(vec.Y, Axis.AXIS_Y);
            Destination.Rotate(vec.Z, Axis.AXIS_Z);
        }

        public override void Scale(Point3D vec)
        {
            Origin.Scale(vec.X, vec.Y, vec.Z);
            Destination.Scale(vec.X, vec.Y, vec.Z);
        }
        public override void RotateAroundAxis(double angle, Axis a, Edge3D line = null)
        {
            Origin.Rotate(angle, a, line);
            Destination.Rotate(angle, a, line);
        }

        public override void Draw(Graphics g, Scene.Camera camera, Projection pr = 0)
        {
            var p1 = new PointF(0, 0);
            var p2 = new PointF(0, 0);
            switch (pr)
            {
                case Projection.ISOMETRIC:
                    p1 = Origin.GetIsometricProj();
                    p2 = Destination.GetIsometricProj();
                    break;
                case Projection.ORTHOGR_X:
                    p1 = Origin.GetOrthographicProj(Axis.AXIS_X);
                    p2 = Destination.GetOrthographicProj(Axis.AXIS_X);
                    break;
                case Projection.ORTHOGR_Y:
                    p1 = Origin.GetOrthographicProj(Axis.AXIS_Y);
                    p2 = Destination.GetOrthographicProj(Axis.AXIS_Y);
                    break;
                case Projection.ORTHOGR_Z:
                    p1 = Origin.GetOrthographicProj(Axis.AXIS_Z);
                    p2 = Destination.GetOrthographicProj(Axis.AXIS_Z);
                    break;
                default:
                    p1 = Origin.GetPerspectiveProj(camera);
                    p2 = Destination.GetPerspectiveProj(camera);
                    break;
            }
            var len = (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);
            if (Origin.Z > 0 && Destination.Z > 0 && Math.Abs(p1.Y) < 1000 && Math.Abs(p2.Y) < 1000)
                g.DrawLine(new Pen(Color, 2), p1, p2);
        }

        public override void Apply(float[][] transform)
        {
            Origin.Apply(transform);
            Destination.Apply(transform);
        }
    }
}
