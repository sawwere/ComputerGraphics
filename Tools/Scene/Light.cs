using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Primitives;

namespace Tools.Scene
{
    public class Light
    {
        public Point3D position;
        public Color color;

        public Light(Point3D pos, Color color)
        {
            position = pos;
            this.color = color;
        }
    }
}
