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
        float radius;

        public Sphere(Vector3 p, float r)
        {
            Vertexes.Add(p);
            radius = r;
        }


        public static bool raySphereIntersection(Ray r, Vector3 sphere_pos, float sphere_rad, out float t)
        {
            //Vector3 k = r.start - sphere_pos;
            //float b = k.DotProduct(r.direction);
            //float c = k.DotProduct(k) - sphere_rad * sphere_rad;
            //float d = b * b - c;
            //t = 0;

            //if (d >= 0)
            //{
            //    float sqrtd = (float)Math.Sqrt(d);
            //    float t1 = -b + sqrtd;
            //    float t2 = -b - sqrtd;

            //    float min_t = Math.Min(t1, t2);
            //    float max_t = Math.Max(t1, t2);

            //    t = (min_t > EPS) ? min_t : max_t;
            //    return t > EPS;
            //}
            //return false;
            t = 0;
            var b = 2 * r.dest.DotProduct(r.origin - sphere_pos);
            var c = (r.origin - sphere_pos).Length * (r.origin - sphere_pos).Length - sphere_rad * sphere_rad;
            var delta = b * b - 4 * c;
            if (delta > 0)
            {
                var t1 = (-b + (float)Math.Sqrt(delta)) / 2;
                var t2 = (-b - (float)Math.Sqrt(delta)) / 2;
                if (t1 > 0 && t2 > 0)
                {
                    t = Math.Min(t1, t2);
                    return true;
                }
            }
            return false;
        }

        public override bool figureIntersection(Ray r, out float t, out Vector3 normal)
        {
            t = 0;
            normal = null;

            if (raySphereIntersection(r, Vertexes[0], radius, out t) && (t > 1e-4f))
            {
                normal = (r.origin + r.dest * t) - Vertexes[0];
                normal = normal.Normalize();
                return true;
            }
            return false;
        }
    }
}
