using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Tools.Primitives
{
    public class Mesh : Primitive
    {
        public Color color = Color.Gray;
        private List<Triangle3D> polygons;

        public Point3D Center
        {
            get;
            private set;
        }

        public List<Triangle3D> get_poligons()
        {
            return polygons;
        }

        public Mesh(List<Triangle3D> list = null)
        {
            polygons = new List<Triangle3D>();
            Center = new Point3D(0, 0, 0);
            if (list != null)
            {
                polygons.AddRange(list);
            }
            //var rand = new Random();
            //foreach (Triangle3D f in polygons)
            //{
            //    f.color = Color.FromArgb(255, rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
            //}
        }

        public override Primitive Clone()
        {
            Mesh res = new Mesh();
            foreach (Triangle3D f in polygons)
            {
                res.polygons.Add(new Triangle3D(f[0], f[1], f[2]));
                res.polygons.Last();
            }
            return res;
        }

        private void UpdateCenter()
        {
            Center = new Point3D(0, 0, 0);
            foreach (Triangle3D f in polygons)
            {
                Center += f.Center;
            }
            Center = (1f / polygons.Count) * Center;
        }

        public override void Translate(Point3D vec)
        {
            foreach (Triangle3D f in polygons)
            {
                f.Translate(vec);
            }
        }

        public override void Rotate(Point3D vec)
        {
            foreach (Triangle3D f in polygons)
            {
                f.Rotate(vec);
            }
        }

        public override void Scale(Point3D vec)
        {
            foreach (Triangle3D f in polygons)
            {
                f.Scale(vec);
            }
        }

        public override void RotateAroundAxis(double angle, Axis a, Edge3D line = null)
        {
            foreach (Triangle3D f in polygons)
            {
                f.RotateAroundAxis(angle, a, line);
            }
        }

        public void make_hexahedron(float size = 1)
        {
            polygons = new List<Triangle3D> {
                new Triangle3D( new Point3D(size, -size, size), new Point3D(size, -size, -size), new Point3D(size, size, -size)),
                new Triangle3D( new Point3D(size, -size, size), new Point3D(size, size, -size), new Point3D(size, size, size)),

                new Triangle3D( new Point3D(-size, size, size), new Point3D(-size, size, -size), new Point3D(-size, -size, -size)),
                new Triangle3D( new Point3D(-size, size, size), new Point3D(-size, -size, -size), new Point3D(-size, -size, size)),

                new Triangle3D( new Point3D(size, size, size), new Point3D(-size, size, size), new Point3D(-size, -size, size)),
                new Triangle3D( new Point3D(size, size, size), new Point3D(-size, -size, size), new Point3D(size, -size, size)),

                new Triangle3D( new Point3D(size, size, size), new Point3D(-size, size, -size), new Point3D(size, size, -size)),
                new Triangle3D( new Point3D(size, size, size), new Point3D(-size, size, -size), new Point3D(-size, size, size)),

                new Triangle3D( new Point3D(size, -size, -size), new Point3D(-size, -size, -size), new Point3D(-size, size, -size)),
                new Triangle3D( new Point3D(size, -size, -size), new Point3D(-size, size, -size), new Point3D(size, size, -size)),

                new Triangle3D( new Point3D(-size, -size, size), new Point3D(-size, -size, -size), new Point3D(size, -size, -size)),
                new Triangle3D( new Point3D(-size, -size, size), new Point3D(size, -size, -size), new Point3D(size, -size, size))
            };
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
            foreach (Triangle3D t in polygons)
            {
                if (t.isVisible)
                    t.Draw(g, camera, pr, pen);
            }
        }
        public void reflectX()
        {
            if (polygons != null)
                foreach (var f in polygons)
                    f.reflectX();
            UpdateCenter();
        }

        public void reflectY()
        {
            if (polygons != null)
                foreach (var f in polygons)
                    f.reflectY();
            UpdateCenter();
        }

        public void reflectZ()
        {
            if (polygons != null)
                foreach (var f in polygons)
                    f.reflectZ();
            UpdateCenter();
        }

        public void calculateZBuffer(Scene.Camera camera, int width, int height, ref float[] buf)
        {
            foreach (var f in polygons)
            {
                DrawPolygon(camera, f[0], f[1], f[2], width, height, buf);
                //help(camera, f[0], f[1], f[2], buf, width, height);
            }

        }

        private void DrawPolygon(Scene.Camera camera, Point3D P0, Point3D P1, Point3D P2, int width, int height, float[] buff)
        {
            PointF projected0 = P0.GetPerspective(camera);
            projected0.Y = (int)projected0.Y;
            PointF projected1 = P1.GetPerspective(camera);
            projected1.Y = (int)projected1.Y;
            PointF projected2 = P2.GetPerspective(camera);
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

            PointF projectedLeft = projected1;
            PointF projectedRight = projected2;
            Point3D pLeft = P1;
            Point3D pRight = P2;
            Edge2D edge = new Edge2D(projected0.X, projected0.Y, projected1.X, projected1.Y, Color.Black);
            

            var pp = new Point2D(projected2);
            float crossx1 = pp.CompareToEdge2(edge);
            if (crossx1 < 0)
            {
                (projectedLeft, projectedRight) = (projectedRight, projectedLeft);
                (pLeft, pRight) = (pRight, pLeft);
            }
            float mid = projected1.Y;
            for (int y = (int)projected0.Y; y < mid; y++)
            {
                DrawGradientLines2(y, P0, projected0, pLeft, projectedLeft, pRight, projectedRight, width, height, buff);
            }


            projectedLeft = projected1;
            projectedRight = projected0;
            pLeft = P1;
            pRight = P0;

            edge = new Edge2D(projected2.X, projected2.Y, projected1.X, projected1.Y, Color.Black);
            pp = new Point2D(projected0);
            crossx1 = pp.CompareToEdge2(edge);
            if (crossx1 > 0)
            {
                (projectedLeft, projectedRight) = (projectedRight, projectedLeft);
                (pLeft, pRight) = (pRight, pLeft);
            }
            for (int y = (int)projected2.Y; y >= mid; y--)
            {
                DrawGradientLines2(y, P2, projected2, pLeft, projectedLeft, pRight, projectedRight, width, height, buff);
            }

        }

        private void DrawGradientLines2(int y, Point3D middle, PointF projectedMiddle,
            Point3D left, PointF projectedLeft,
            Point3D right, PointF projectedRight,
            int width, int height, float[] buff)
        {
            var leftBound = (int)Interpolate(projectedMiddle.Y, projectedMiddle.X, projectedLeft.Y, projectedLeft.X, y);
            var rightBound = (int)Interpolate(projectedMiddle.Y, projectedMiddle.X, projectedRight.Y, projectedRight.X, y);
            if (leftBound > rightBound)
            {
                return;
            }
            var zLeft = Interpolate(projectedMiddle.Y, middle.Z, projectedLeft.Y, left.Z, y);
            var zRight = Interpolate(projectedMiddle.Y, middle.Z, projectedRight.Y, right.Z, y);

            for (int x = (int)leftBound; x <= rightBound; x++)
            {
                int xx = x + width / 2;
                int yy = -y + height / 2;
                if (xx < 0 || xx > width || yy < 0 || yy > height || (xx + width * yy) < 0 || (xx + width * yy) > (buff.Length - 1))
                    continue;
                float tempZ = Interpolate(leftBound, zLeft, rightBound, zRight, x);
                if (tempZ < buff[xx + width * yy])
                {
                    buff[xx + yy * width] = tempZ;
                }
            }
        }

     
        float Interpolate(float x0, float y0, float x1, float y1, float i)
        {
            if (Math.Abs(x0 - x1) < 1e-8)
                return (y0);
            return y0 + ((y1 - y0) * (i - x0)) / (x1 - x0);
        }
    }
}
