using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndTask2
{
    public class Ray
    {
        public Vector3 origin;
        public Vector3 dest;

        public Ray(Vector3 start, Vector3 end)
        {
            origin = start;
            dest = (end - start).Normalize();
        }

        public Ray Clone()
        {
            return new Ray(origin.Clone(), dest.Clone());
        }

        public Ray() { }

        /// <summary>
        /// Отраженный луч
        /// </summary>
        public Ray Reflect(Vector3 hit, Vector3 normal)
        {
            Vector3 reflectDir = dest - 2 * normal * dest.DotProduct(normal);
            return new Ray(hit, hit + reflectDir);
        }

        /// <summary>
        /// Преломленный луч
        /// </summary>
        public Ray Refract(Vector3 hit, Vector3 normal, float eta)
        {
            Ray res = new Ray();
            float sclr = normal.DotProduct(dest);

            float k = 1 - eta * eta * (1 - sclr * sclr);

            if (k >= 0)
            {
                float cos_theta = (float)Math.Sqrt(k);
                res.origin = hit.Clone();
                res.dest = (eta * dest - (cos_theta + eta * sclr) * normal).Normalize();
                return res;
            }
            else
                return null;
        }

        public Vector3 point(float t)
        {
            return (origin + t * (dest - origin));
        }
    }
}
