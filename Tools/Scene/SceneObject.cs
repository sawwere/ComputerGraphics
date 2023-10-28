using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Primitives;

namespace Tools
{
    /// <summary>
    /// Используется для манипуляции положением, вращением и масштабированием объекта
    /// </summary>
    public class Transform
    {
        public Point3D forward { get; private set; }
        public Point3D position { get; set; }
        public Point3D rotation { get; set; }
        public Point3D scale { get; set; }

        public Transform() 
        { 
            position = new Point3D(0, 0, 0);
            rotation = new Point3D(0, 0, 0);
            scale = new Point3D(1, 1, 1);
            forward = new Point3D(0, 0, 1);
        }

        public void reflectX()
        {
            position = new Point3D(position.X * -1, position.Y, position.Z);
            scale = new Point3D(scale.X * -1, scale.Y, scale.Z);
        }

        public void reflectY()
        {
            position = new Point3D(position.X, position.Y * -1, position.Z);
            scale = new Point3D(scale.X, scale.Y * -1, scale.Z);
        }

        public void reflectZ()
        {
            position = new Point3D(position.X, position.Y, position.Z * -1);
            scale = new Point3D(scale.X, scale.Y, scale.Z * -1);
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

        /// <summary>
        /// Поверрнуть трансформ так, чтобы вектор forward указывал на точку target.
        /// </summary>
        /// <param name="target">Object to point towards.</param>
        public void LookAt(Point3D target)
        {
            //TODO
        }
                
        public void RotateAroundAxis(float angle, Edge3D edge = null)
        {
            /// <summary>
            /// Не надо вызывать эту функцию!!!
            /// </summary>
        }
    }

    /// <summary>
    /// Олицетворяет объект на сцене
    /// </summary>
    public class SceneObject
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public Mesh Local { get; private set; }

        /// <summary>
        /// Используется для манипуляции положением, вращением и масштабированием объекта
        /// </summary>
        public Transform Transform { get; private set; }

        public SceneObject(Mesh init)
        {
            Name = string.Empty;
            Local = init;
            Transform  = new Transform();
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Копия данного объекта, к которой применены операции трансформирования
        /// </summary>
        /// <returns>КОПИЯ данного объекта</returns>
        public ITransformable GetTransformed()
        {
            var res = Local.Clone();
            res.Rotate(Transform.rotation);
            res.Scale(Transform.scale);
            res.Translate(Transform.position);
            return (Mesh)res;
        }

        /// <summary>
        /// Не надо вызывать эту функцию!!!
        /// </summary>
        public void RotateAroundAxis(float angle, Axis axis, Edge3D edge = null)
        {
            //Local.Rotate(angle, axis, edge); 
        }
    }
}
