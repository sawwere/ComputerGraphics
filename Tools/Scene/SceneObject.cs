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

        public Mesh Mesh { get; set; }
        public Point3D position { get; set; }
        public Point3D rotation { get; set; }
        public Point3D scale { get; set; }

        public SceneObject()
        {
            Name = string.Empty;
            Mesh = new Mesh();
            Id = Guid.NewGuid();
            position = new Point3D(0, 0, 0);
            rotation = new Point3D(0, 0, 0);
            scale = new Point3D(1 , 1, 1);
        }

        public void Translate(float dx, float dy, float dz)
        {
            Translate(new Point3D(dx, dy, dz));
        }

        public void RotateAroundAxis(float angle, Axis axis, Edge3D edge = null)
        {
            Mesh.Rotate(angle, axis, edge);
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
            Mesh.Translate(vec.X, vec.Y, vec.Z);
            position += vec;
        }

        public void Rotate(Point3D vec)
        {
            Mesh.Rotate(vec.X, Axis.AXIS_X);
            Mesh.Rotate(vec.Y, Axis.AXIS_Y);
            Mesh.Rotate(vec.Z, Axis.AXIS_Z);
            rotation += vec;
        }

        public void Scale(Point3D vec)
        {
            Mesh.Scale(vec.X, vec.Y, vec.Z);
            scale.X *= vec.X;
            scale.Y *= vec.Y;
            scale.Z *= vec.Z;
        }
    }
}
