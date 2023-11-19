using Tools.Primitives;

namespace Tools.Scene
{
    public class Camera
    {
        public Point3D position 
        { 
            get; 
            private set; 
        }

        public Point3D rotation 
        { 
            get; 
            private set; 
        }

        public Point3D forward
        {
            get;
            private set;
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public float fovy;
        public Camera(int w, int h, Point3D pos, Point3D rotation, Point3D forward)
        {
            Width = w;
            Height = h;
            fovy = 90;
            position = pos;
            this.rotation = rotation;
            this.forward = forward;
        }

        public void Translate(Point3D vec)
        {
            position += vec;
        }

        public void Rotate(Point3D vec)
        {
            rotation += vec;
        }
    }
}
