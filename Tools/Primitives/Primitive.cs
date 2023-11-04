using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Primitives
{
    public abstract class Primitive
    {
        public abstract Primitive Clone();
        public abstract void Translate(Point3D vec);
        public abstract void Scale(Point3D vec);
        public abstract void Rotate(Point3D vec);
        public abstract void RotateAroundAxis(double angle, Axis a, Edge3D line = null);
        public abstract void Draw(Graphics g, Scene.Camera camera, Projection pr = 0, Pen pen = null);
    }
}
