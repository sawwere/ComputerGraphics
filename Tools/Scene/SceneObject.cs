using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Primitives;

namespace Tools
{
    public class SceneObject
    {
        public Guid Id { get; private set; }

        public string Name { get; set; }
        public Mesh Local { get; private set; }
        public Mesh Tranform
        {
            get
            {
                var res = (Local as Mesh).Clone();
                res.Rotate(rotation);
                res.Scale(scale);
                res.Translate(position);
                return res;
            }
        }
        public Point3D position { get; set; }
        public Point3D rotation { get; set; }
        public Point3D scale { get; set; }

        public SceneObject(Mesh init)
        {
            Name = string.Empty;
            Local = init;
            Id = Guid.NewGuid();
            position = new Point3D(0, 0, 0);
            rotation = new Point3D(0, 0, 0);
            scale = new Point3D(1 , 1, 1);
        }

        public void Translate(float dx, float dy, float dz)
        {
            Translate(new Point3D(dx, dy, dz));
        }
        /// <summary>
        /// Не надо вызывать эту функцию!!!
        /// </summary>
        public void RotateAroundAxis(float angle, Axis axis, Edge3D edge = null)
        {
            //Local.Rotate(angle, axis, edge); 
        }

        public void Rotate(float rx, float ry, float rz)
        {
            Rotate(new Point3D(rx, ry, rz));
        }

        public void Scale(float kx, float ky, float kz)
        {
            Scale(new Point3D(kx, ky, kz));
        }

        public void Translate(Point3D vec)
        {
            position += vec;
        }

        public void Rotate(Point3D vec)
        {
            rotation += vec;
        }

        public void Scale(Point3D vec)
        {
            scale = scale * vec;
        }
    }
}
