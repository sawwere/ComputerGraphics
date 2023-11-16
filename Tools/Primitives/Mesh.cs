using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Tools.Primitives
{
    public class Mesh : Primitive
    {
        public List<Point3D> Vertexes { get; private set; } = new List<Point3D>();

        public List<List<int>> Faces { get; private set; } = new List<List<int>>();
        
        /// <summary>
        /// таблица смежности вершин. ИндексВершины => список смежных с ней
        /// </summary>
        public Dictionary<int, List<int>> Adjacency { get; set; } = new Dictionary<int, List<int>>();
        private List<Triangle3D> polygons;

        public Point3D Center
        {
            get
            {
                var res = new Point3D(0, 0, 0);
                foreach (var f in Vertexes)
                {
                    res += f;
                }
                res = (1f / Vertexes.Count) * res;
                return res;
            }
        }

        public List<Triangle3D> get_polygons()
        {
            return Faces.Select(x=> new Triangle3D(Vertexes[x[0]], Vertexes[x[1]], Vertexes[x[2]])).ToList();
        }

        public Mesh(List<Point3D> points, List<List<int>> faces)
        {
            polygons = new List<Triangle3D>();
            Vertexes = new List<Point3D>();
            Faces = new List<List<int>>();
            int i = 0;
            foreach (Point3D point in points)
            {
                i++;
                Adjacency.Add(i, new List<int>());
                Vertexes.Add(new Point3D(point.X, point.Y, point.Z));
            }
            foreach (var item in faces)
            {
                Faces.Add(item);
            }
            foreach (var lst in Faces)
            {
                polygons.Add(new Triangle3D(Vertexes[lst[0]], Vertexes[lst[1]], Vertexes[lst[2]]));
            }
            
        }

        public Mesh(List<Triangle3D> list = null)
        {

            polygons = new List<Triangle3D>();
            if (list == null)
            {
                return;
            }
            List<Point3D> points = new List<Point3D>();
            int k = 0;
            foreach (var t in list)
            {
                for (int i = 0; i < 3;i++)
                {
                    if (!points.Contains(t[i]))
                    {
                        k++;
                        Adjacency.Add(k, new List<int>());
                        Vertexes.Add(new Point3D(t[i].X, t[i].Y, t[i].Z));
                        points.Add(t[i]);
                    }
                }
            }

            foreach (var t in list)
            {
                for (int i = 0; i < 3; i++)
                {
                    int i0 = points.IndexOf(t[0]);
                    int i1 = points.IndexOf(t[1]);
                    int i2 = points.IndexOf(t[2]);
                    Faces.Add(new List<int>() { i0, i1, i2 });
                }
            }

            if (list != null)
            {
                polygons.AddRange(list);
            }
        }

        public override Primitive Clone()
        {
            Mesh res = new Mesh(Vertexes, Faces);
            return res;
        }

        public override void Translate(Point3D vec)
        {
            for (int i = 0; i < Vertexes.Count; i++)
            {
                var p = Vertexes[i];
                p.Translate(vec.X, vec.Y, vec.Z);
                Vertexes[i] = p;
            }
        }

        public override void Rotate(Point3D vec)
        {
            for (int i = 0; i < Vertexes.Count; i++)
            {
                var p = Vertexes[i];
                p.Rotate(vec.X, Axis.AXIS_X);
                p.Rotate(vec.Y, Axis.AXIS_Y);
                p.Rotate(vec.Z, Axis.AXIS_Z);
                Vertexes[i] = p;
            }
        }

        public override void Scale(Point3D vec)
        {
            for (int i = 0; i < Vertexes.Count; i++)
            {
                var p = Vertexes[i];
                p.Scale(vec.X, vec.Y, vec.Z);
                Vertexes[i] = p;
            }
        }

        public override void RotateAroundAxis(double angle, Axis a, Edge3D line = null)
        {
            foreach (Triangle3D f in polygons)
            {
                f.RotateAroundAxis(angle, a, line);
            }
        }
        public void make_tetrahedron(float size = 1)
        {
            polygons = new List<Triangle3D>
            {
                 new Triangle3D(new Point3D(size,size,size),new Point3D(-size,-size,size),new Point3D(-size,size,-size)),
                 new Triangle3D(new Point3D(size,size,size),new Point3D(-size,-size,size),new Point3D(size,-size,-size)),
                 new Triangle3D(new Point3D(size,size,size),new Point3D(size,-size,-size),new Point3D(-size,size,-size)),
                 new Triangle3D(new Point3D(-size,-size,size),new Point3D(-size,size,-size),new Point3D(size,-size,-size))
            };
        }

        public void make_octahedron(float size = 1)
        {
            polygons = new List<Triangle3D>
            {
                new Triangle3D(new Point3D(0,size,0),new Point3D(0,0,size),new Point3D(size,0,0)),
                new Triangle3D(new Point3D(0,size,0),new Point3D(0,0,size),new Point3D(-size,0,0)),
                new Triangle3D(new Point3D(0,-size,0),new Point3D(0,0,size),new Point3D(-size,0,0)),
                new Triangle3D(new Point3D(0,-size,0),new Point3D(0,0,size),new Point3D(size,0,0)),
                new Triangle3D(new Point3D(0,size,0),new Point3D(0,0,-size),new Point3D(size,0,0)),
                new Triangle3D(new Point3D(0,size,0),new Point3D(0,0,-size),new Point3D(-size,0,0)),
                new Triangle3D(new Point3D(0,-size,0),new Point3D(0,0,-size),new Point3D(-size,0,0)),
                new Triangle3D(new Point3D(0,-size,0),new Point3D(0,0,-size),new Point3D(size,0,0))
            };
        }
        public void make_icosahedron(float size = 1)
        {
            float p = (float)(1 + System.Math.Sqrt(5)) / 2;//phi
            polygons = new List<Triangle3D>
            {
                new Triangle3D(new Point3D(-p*size,0,size),new Point3D(0,-size,p*size),new Point3D(0,size,p*size)),//1
                new Triangle3D(new Point3D(0,-size,p*size),new Point3D(0,size,p*size),new Point3D(p*size,0,size)),//2
                new Triangle3D(new Point3D(0,-size,p*size),new Point3D(-size,-p*size,0),new Point3D(size,-p*size,0)),//3
                new Triangle3D(new Point3D(0,-size,p*size),new Point3D(-size,-p*size,0),new Point3D(p*size,0,size)),//4
                new Triangle3D(new Point3D(p*size,0,size),new Point3D(0,size,p*size),new Point3D(size,p*size,0)),//5
                new Triangle3D(new Point3D(size,p*size,0),new Point3D(0,size,p*size),new Point3D(-size,p*size,0)),//6
                new Triangle3D(new Point3D(0,size,p*size),new Point3D(-p*size,0,size),new Point3D(-size,p*size,0)),//7
                new Triangle3D(new Point3D(-size,p*size,0),new Point3D(0,-size,p*size),new Point3D(-size,-p*size,0)),//8
                new Triangle3D(new Point3D(size,-p*size,0),new Point3D(p*size,0,size),new Point3D(p*size,0,-size)),//9
                new Triangle3D(new Point3D(p*size,0,size),new Point3D(size,p*size,0),new Point3D(p*size,0,-size)),//10
                new Triangle3D(new Point3D(-size,p*size,0),new Point3D(-p*size,0,size),new Point3D(-p*size,0,-size)),//11
                new Triangle3D(new Point3D(-p*size,0,size),new Point3D(-p*size,0,-size),new Point3D(-size,-p*size,0)),//12
                new Triangle3D(new Point3D(-size,-p*size,0),new Point3D(size,-p*size,0),new Point3D(0,-size,-p*size)),//13
                new Triangle3D(new Point3D(size,-p*size,0),new Point3D(0,-size,-p*size),new Point3D(p*size,0,-size)),//14
                new Triangle3D(new Point3D(p*size,0,-size),new Point3D(size,p*size,0),new Point3D(0,size,-p*size)),//15
                new Triangle3D(new Point3D(size,p*size,0),new Point3D(-size,p*size,0),new Point3D(0,size,-p*size)),//16
                new Triangle3D(new Point3D(-size,p*size,0),new Point3D(0,size,-p*size),new Point3D(-p*size,0,-size)),//17
                new Triangle3D(new Point3D(-p*size,0,-size),new Point3D(-size,-p*size,0),new Point3D(0,-size,-p*size)),//18
                new Triangle3D(new Point3D(-p*size,0,-size),new Point3D(0,-size,-p*size),new Point3D(0,size,-p*size)),//19
                new Triangle3D(new Point3D(0,-size,-p*size),new Point3D(p*size,0,-size),new Point3D(0,size,-p*size))//20

            };
        }

        public void make_dodecahedron(float size = 1)
        {
            float p = (float)(1 + System.Math.Sqrt(5)) / 2;//phi
            float p1 = 1 / p;//  1/phi
            polygons = new List<Triangle3D>
            {
                new Triangle3D(new Point3D(size,size,size),new Point3D(-size,size,size),new Point3D(0,p*size,p1*size)),//1
                new Triangle3D(new Point3D(size,size,size),new Point3D(p1*size,0,p*size),new Point3D(-p1*size,0,p*size)),//2
                new Triangle3D(new Point3D(size,size,size),new Point3D(-size,size,size),new Point3D(-p1*size,0,p*size)),//3

                new Triangle3D(new Point3D(-size,-size,size),new Point3D(size,-size,size),new Point3D(0,-p*size,p1*size)),//4
                new Triangle3D(new Point3D(-size,-size,size),new Point3D(-p1*size,0,p*size),new Point3D(p1*size,0,p*size)),//5
                new Triangle3D(new Point3D(-size,-size,size),new Point3D(size,-size,size),new Point3D(p1*size,0,p*size)),//6

                new Triangle3D(new Point3D(size,-size,size),new Point3D(p1*size,0,p*size),new Point3D(size,size,size)),//7
                new Triangle3D(new Point3D(size,size,size),new Point3D(size,-size,size),new Point3D(p*size,-p1*size,0)),//8
                new Triangle3D(new Point3D(size,size,size),new Point3D(p*size,p1*size,0),new Point3D(p*size,-p1*size,0)),//9

                new Triangle3D(new Point3D(size,-size,-size),new Point3D(size,size,-size),new Point3D(p1*size,0,-p*size)),//10
                new Triangle3D(new Point3D(size,-size,-size),new Point3D(size,size,-size),new Point3D(p*size,p1*size,0)),//11
                new Triangle3D(new Point3D(size,-size,-size),new Point3D(p*size,-p1*size,0),new Point3D(p*size,p1*size,0)),//12

                new Triangle3D(new Point3D(0,p*size,p1*size),new Point3D(size,size,size),new Point3D(p*size,p1*size,0)),//13
                new Triangle3D(new Point3D(0,p*size,p1*size),new Point3D(size,size,-size),new Point3D(p*size,p1*size,0)),//14
                new Triangle3D(new Point3D(0,p*size,p1*size),new Point3D(size,size,-1),new Point3D(0,p*size,-p1*size)),//15

                new Triangle3D(new Point3D(-size,size,size),new Point3D(0,p*size,p1*size),new Point3D(-p*size,p1*size,0)),//16
                new Triangle3D(new Point3D(0,p*size,-p1*size),new Point3D(0,p*size,p1*size),new Point3D(-p*size,p1*size,0)),//17
                new Triangle3D(new Point3D(0,p*size,-p1*size),new Point3D(-size,size,-size),new Point3D(-p*size,p1*size,0)),//18

                new Triangle3D(new Point3D(-p1*size,0,p*size),new Point3D(-size,size,size),new Point3D(-size,-size,size)),//19
                new Triangle3D(new Point3D(-p*size,p1*size,0),new Point3D(-size,size,size),new Point3D(-size,-size,size)),//20
                new Triangle3D(new Point3D(-p*size,p1*size,0),new Point3D(-p*size,-p1*size,0),new Point3D(-size,-size,size)),//21

                new Triangle3D(new Point3D(-p*size,p1*size,0),new Point3D(-p*size,-p1*size,0),new Point3D(-size,size,-size)),//22
                new Triangle3D(new Point3D(-size,-size,-size),new Point3D(-p*size,-p1*size,0),new Point3D(-size,size,-size)),//23
                new Triangle3D(new Point3D(-size,-size,-size),new Point3D(-p1*size,0,-p*size),new Point3D(-size,size,-size)),//24

                new Triangle3D(new Point3D(-size,-size,-size),new Point3D(-p*size,-p1*size,0),new Point3D(-size,-size,size)),//25
                new Triangle3D(new Point3D(-size,-size,-size),new Point3D(0,-p*size,p1*size),new Point3D(-size,-size,size)),//26
                new Triangle3D(new Point3D(-size,-size,-size),new Point3D(0,-p*size,p1*size),new Point3D(0,-p*size,-p1*size)),//27

                new Triangle3D(new Point3D(size,-size,size),new Point3D(0,-p*size,p1*size),new Point3D(0,-p*size,-p1*size)),//28
                new Triangle3D(new Point3D(size,-size,size),new Point3D(p*size,-p1*size,0),new Point3D(0,-p*size,-p1*size)),//29
                new Triangle3D(new Point3D(size,-size,-size),new Point3D(p*size,-p1*size,0),new Point3D(0,-p*size,-p1*size)),//30

                new Triangle3D(new Point3D(size,-size,-size),new Point3D(-size,-size,-size),new Point3D(0,-p*size,-p1*size)),//31
                new Triangle3D(new Point3D(size,-size,-size),new Point3D(-size,-size,-size),new Point3D(p1*size,0,-p*size)),//32
                new Triangle3D(new Point3D(-p1*size,0,-p*size),new Point3D(-size,-size,-size),new Point3D(p1*size,0,-p*size)),//33

                new Triangle3D(new Point3D(-p1*size,0,-p*size),new Point3D(size,size,-size),new Point3D(p1*size,0,-p*size)),//34
                new Triangle3D(new Point3D(-p1*size,0,-p*size),new Point3D(size,size,-size),new Point3D(-size,size,-size)),//35
                new Triangle3D(new Point3D(0,p*size,-p1*size),new Point3D(size,size,-size),new Point3D(-size,size,-size))//36
            };
        }

        public override void Draw(Graphics g, Scene.Camera camera, Projection pr = 0, Pen pen = null)
        {
            foreach (var lst in Faces)
            {
                var t = new Triangle3D(Vertexes[lst[0]], Vertexes[lst[1]], Vertexes[lst[2]]);
                t.FindNormal(Center, camera);
                if (t.IsVisible)
                    t.Draw(g, camera, pr, pen);
            }
        }

        //==============================================
        //
        // Gouraud_shading
        //
        //==============================================
        public void CalculateLambert(Scene.Light light, Scene.Camera camera)
        {
            Dictionary<int, Point3D> pointNormal = new Dictionary<int, Point3D>();
            for (int i = 0; i < Vertexes.Count; i++)
            {
                List<List<int>> adjacentFaces = Faces.Where(x => x.Contains(i)).ToList();
                Point3D res = new Point3D(0, 0, 0);
                foreach (var face in adjacentFaces)
                {
                    var t = new Triangle3D(Vertexes[face[0]], Vertexes[face[1]], Vertexes[face[2]]);
                    t.FindNormal(Center, camera);
                    res += t.Norm;
                }
                res = (1.0f / adjacentFaces.Count) * res;
                pointNormal.Add(i, res);
            }
            for (int i = 0; i < Vertexes.Count; i++)
            {
                Vertexes[i].illumination = ModelLambert(Vertexes[i], pointNormal[i], light.position);
            }
        }

        private float ModelLambert(Point3D vertex, Point3D normal, Point3D lightPos)
        {
            Point3D rayLight = new Point3D(vertex.X - lightPos.X, vertex.Y - lightPos.Y, vertex.Z - lightPos.Z);
            double cos = rayLight.DotProduct( normal) / (rayLight.Length * normal.Length);
            return (float)((cos + 1) / 2);//перевод в диапазон [0,1]
        }

        //==============================================
        //
        // Z-BUFFER
        //
        //==============================================

        public void CalculateZBuffer(Scene.Camera camera, Point3D[] buf)
        {
            foreach (var lst in Faces)
            {
                var f = new Triangle3D(Vertexes[lst[0]], Vertexes[lst[1]], Vertexes[lst[2]]);
                RasterizePolygon(camera, f[0], f[1], f[2], buf);
            }

        }

        private void RasterizePolygon(Scene.Camera camera, Point3D P0, Point3D P1, Point3D P2, Point3D[] buff)
        {
            PointF projected0 = P0.GetPerspectiveProj(camera);
            projected0.Y = (int)projected0.Y;
            PointF projected1 = P1.GetPerspectiveProj(camera);
            projected1.Y = (int)projected1.Y;
            PointF projected2 = P2.GetPerspectiveProj(camera);
            projected2.Y = (int)projected2.Y;
            if (projected0.Y > projected1.Y)
            {
                (projected0, projected1) = (projected1, projected0);
                (P0, P1) = (P1, P0);
            }
            if (projected1.Y > projected2.Y)
            {
                (projected1, projected2) = (projected2, projected1);
                (P1, P2) = (P2, P1);
            }
            if (projected0.Y > projected1.Y)
            {
                (projected0, projected1) = (projected1, projected0);
                (P0, P1) = (P1, P0);
            }
            Point3D left = new Point3D(projected1.X, projected1.Y, P1.Z, P1.illumination);
            Point3D right = new Point3D(projected2.X, projected2.Y, P2.Z, P2.illumination);
            Edge2D edge = new Edge2D(projected0.X, projected0.Y, projected1.X, projected1.Y, Color.Black);
            var pp = new Point2D(projected2);
            if (pp.CompareToEdge2(edge) < 0)
            {
                (left, right) = (right, left);
            }
            float mid = projected1.Y;
            for (int y = (int)projected0.Y; y <= mid; y++)
            {
                DrawGradientLines(y, P0.Z, projected0, left, right, camera, buff, P0.illumination);
            }

            left = new Point3D(projected1.X, projected1.Y, P1.Z, P1.illumination);
            right = new Point3D(projected0.X, projected0.Y, P0.Z, P0.illumination);
            edge = new Edge2D(projected2.X, projected2.Y, projected1.X, projected1.Y, Color.Black);
            pp = new Point2D(projected0);
            if (pp.CompareToEdge2(edge) > 0)
            {
                (left, right) = (right, left);
            }
            for (int y = (int)projected2.Y; y >= mid; y--)
            {
                DrawGradientLines(y, P2.Z, projected2, left, right, camera, buff, P2.illumination);
            }
        }
        private void DrawGradientLines(int y, float middleZ, PointF middle, Point3D left, Point3D right,
            Scene.Camera camera, Point3D[] buff, float i)
        {
            var leftBound = (int)Interpolate(middle.Y, middle.X, left.Y, left.X, y);
            var rightBound = (int)Interpolate(middle.Y, middle.X, right.Y, right.X, y);
            if (leftBound > rightBound)
            {
                return;
            }
            var zLeft = Interpolate(middle.Y, middleZ, left.Y, left.Z, y);
            var zRight = Interpolate(middle.Y, middleZ, right.Y, right.Z, y);
            var ilumLeft = Interpolate(middle.Y, i, left.Y, left.illumination, y);
            var ilumRight = Interpolate(middle.Y, i, right.Y, right.illumination, y);
            for (int x = (int)leftBound; x <= rightBound; x++)
            {
                int xx = x + camera.Width / 2;
                int yy = -y + camera.Height / 2;
                if (xx < 0 || xx > camera.Width 
                        || yy < 0 || yy > camera.Height 
                        || (xx + camera.Width * yy) < 0 
                        || (xx + camera.Width * yy) > (buff.Length - 1))
                    continue;
                float tempZ = Interpolate(leftBound, zLeft, rightBound, zRight, x);
                float ilum = Interpolate(leftBound, ilumLeft, rightBound, ilumRight, x);
                if (tempZ < buff[xx + camera.Width * yy].Z)
                {
                    buff[xx + yy * camera.Width] = new Point3D(x, y, tempZ, ilum );
                }
            }
        }
     
        float Interpolate(float x0, float y0, float x1, float y1, float i)
        {
            if (Math.Abs(x0 - x1) < 1e-8)
                return (y0 + y1) / 2;
            return y0 + ((y1 - y0) * (i - x0)) / (x1 - x0);
        }
    }
}
