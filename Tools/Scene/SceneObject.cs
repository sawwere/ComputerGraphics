using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Primitives;
using Tools.Scene;
using static Tools.MatrixFactory;

namespace Tools
{
    /// <summary>
    /// Используется для манипуляции положением, вращением и масштабированием объекта
    /// </summary>
    public class Transform
    {
        public Point3D forward { get; private set; }

        private Point3D _position;
        public Point3D position 
        {
            get { return _position; }
            private set { _position = value; changed = true; }
        }
        private Point3D _rotation;
        public Point3D rotation
        {
            get { return _rotation; }
            private set { _rotation = value; changed = true; }
        }
        private Point3D _scale;
        public Point3D scale
        {
            get { return _scale; }
            private set { _scale = value; changed = true; }
        }

        private bool changed;

        public Transform() 
        { 
            _position = new Point3D(0, 0, 0);
            _rotation = new Point3D(0, 0, 0);
            _scale = new Point3D(1, 1, 1);
            forward = new Point3D(0, 0, 1);
            changed = false;
        }

        public void reflectX()
        {
            position = new Point3D(position.X * -1, position.Y, position.Z);
        }

        public void reflectY()
        {
            position = new Point3D(position.X, position.Y * -1, position.Z);
        }

        public void reflectZ()
        {
            position = new Point3D(position.X, position.Y, position.Z * -1);
        }

        /// <summary>
        /// Параллельный перенос объекта
        /// </summary>
        public void Translate(Point3D vec)
        {
            position += vec;
        }

        /// <summary>
        /// Поворот объекта вокруг собственного центра
        /// </summary>
        public void Rotate(Point3D vec)
        {
            rotation += vec;
        }

        /// <summary>
        /// Растяжение объекта
        /// </summary>
        public void Scale(Point3D vec)
        {
            scale = scale * vec;
        }

        /// <summary>
        /// Повернуть трансформ так, чтобы вектор forward указывал на точку target.
        /// </summary>
        /// <param name="target">Object to point towards.</param>
        public void LookAt(Point3D target)
        {
            //TODO
        }
                
        public void RotateAroundAxis(float angle, Edge3D edge = null)
        {

        }

        /// <summary>
        /// Изменился ли трансформ с момента последнего вызова этой функции
        /// </summary>
        public bool HasChanged()
        {
            bool res = changed;
            changed = false;
            return res;
        }
    }

    /// <summary>
    /// Олицетворяет объект на сцене
    /// </summary>
    public class SceneObject
    {
        public System.Drawing.Color Color { get; set; }

        public Guid Id { get; private set; }
        public string Name { get; set; }
        public Primitive Local { get; private set; }

        /// <summary>
        /// Используется для манипуляции положением, вращением и масштабированием объекта
        /// </summary>
        public Transform Transform { get; private set; }

        public SceneObject(Primitive init, string name = "")
        {
            Name = name;
            Local = init;
            Transform  = new Transform();
            Id = Guid.NewGuid();
            Color = System.Drawing.Color.Red;
        }

        /// <summary>
        /// Преобразованный объект
        /// </summary>
        /// <returns>КОПИЯ данного объекта в системе координат относительно камеры</returns>
        public Primitive GetTransformed(Camera camera)
        {
            var res = Local.Clone();

            // что здесь происходит?
            // вместо того, чтобы последовательно вращать, масштабировать и перемещать каждую точку
            // (а значит несколько раз умножать на несколько матриц),
            // мы 1 раз считаем итоговую матрицу и умножаем все точки фигуры на нее
            float[][] matr = MatrixProduct(MatrixRotate(Transform.rotation), MatrixScale(Transform.scale));
            matr = MatrixProduct(matr, MatrixTranslate(Transform.position));
            matr = MatrixProduct(matr, MatrixTranslate(-1 * camera.position));
            matr = MatrixProduct(matr, MatrixRotate(-1 * camera.rotation));
            
            res.Apply(matr);
            return res;
        }
    }
}
