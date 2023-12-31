﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Primitives;

namespace Tools.Scene
{
    public class Light: Primitive
    {
        public Point3D position;
        public Color color;

        public Light(Point3D pos, Color color)
        {
            position = pos;
            this.color = color;
        }

        public override Primitive Clone()
        {
            return new Light(position.Clone(), Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        public override void Draw(Graphics g, Camera camera, Projection pr = Projection.PERSPECTIVE)
        {
            var prj = position.GetPerspectiveProj(camera);
            if (position.Z > 0 && Math.Abs(prj.Y) < camera.Height && Math.Abs(prj.X) < camera.Width)
                g.DrawIcon(Tools.Properties.Resources.light, (int)prj.X - 24, (int)prj.Y - 24);
        }

        public override void Rotate(Point3D vec)
        {
            return;
        }

        public override void RotateAroundAxis(double angle, Axis a, Edge3D line = null)
        {
            return;
        }

        public override void Scale(Point3D vec)
        {
            return;
        }

        public override void Translate(Point3D vec)
        {
            position += vec;
        }

        public override void Apply(float[][] transform)
        {
            position.Apply(transform);
        }
    }
}
