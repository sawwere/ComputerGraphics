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
        public Point3D position;
        public Point3D rotation;
        public Point3D forward;

        public int width { get; set; }
        public int height { get; set; }
        public float fovy;
        public Camera(int w, int h, Point3D pos, Point3D rotation, Point3D forward)
        {
            width = w;
            height = h;
            fovy = 90;
            position = pos;
            this.rotation = rotation;
            this.forward = forward;
        }
    }
}
