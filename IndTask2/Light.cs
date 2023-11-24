using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndTask2
{
    public class Light
    {
        public Vector3 position;
        public Vector3 color;

        public Light(Vector3 pos, Color color)
        {
            position = pos;
            this.color = new Vector3(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f);
        }

        public Light(Vector3 pos, Vector3 color)
        {
            position = pos;
            this.color = color;
        }

        public Light Clone()
        {
            return new Light(position.Clone(), color.Clone());
        }

        public Vector3 Shading(Vector3 hit_point, Vector3 normal, Vector3 colorObject, float diffuse_coef)
        {
            Vector3 rayLight = (position - hit_point).Normalize();
            float cos = rayLight.DotProduct(normal);
            Vector3 diff = diffuse_coef * color * Math.Max(cos, 0);

            return new Vector3(diff.X * colorObject.X, diff.Y * colorObject.Y, diff.Z * colorObject.Z);
        }
    }
}
