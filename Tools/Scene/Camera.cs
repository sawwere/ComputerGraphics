using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Primitives;

namespace Tools.Scene
{
    public class Camera
    {
        public Point3D position { get; set; }
        public Point3D forward { get; set; }
        public Camera(Point3D pos, Point3D forward)
        {
            position = pos;
            this.forward = forward;
        }
    }
}
