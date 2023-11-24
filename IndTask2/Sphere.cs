using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace IndTask2
{
    public class Sphere: Mesh
    {
        public float Radius { get; private set; }

        public Sphere(Vector3 p, float r)
        {
            Vertexes.Add(p);
            Radius = r;
        }
        public Vector3 Position
        {
            get { return Vertexes[0].Clone(); }
        }

        public override bool Intersection(Ray r, out float t, out Vector3 normal)
        {
            t = 0;
            normal = null;
            var b = 2 * r.dest.DotProduct(r.origin - Position);
            var c = (r.origin - Position).Length * (r.origin - Position).Length - Radius * Radius;
            var delta = b * b - 4 * c;
            if (delta > 0)
            {
                var t1 = (-b + (float)Math.Sqrt(delta)) / 2;
                var t2 = (-b - (float)Math.Sqrt(delta)) / 2;
                if (t1 > 0 && t2 > 0)
                {
                    t = Math.Min(t1, t2);
                    if (t > 1e-4f)
                    {
                        normal = (r.origin + r.dest * t) - Vertexes[0];
                        normal = normal.Normalize();
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
