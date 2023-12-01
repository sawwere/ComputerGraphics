using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;

namespace Tools.Primitives
{
    public class Mesh : Primitive
    {
        public List<Point3D> Vertexes { get; private set; } = new List<Point3D>();

        public List<List<int>> Faces { get; private set; } = new List<List<int>>();
        
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
            foreach (Point3D point in points)
            {
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
            foreach (var t in list)
            {
                for (int i = 0; i < 3;i++)
                {
                    if (!points.Contains(t[i]))
                    {
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
            for (int i = 0; i < Vertexes.Count; i++)
            {
                var p = Vertexes[i];
                p.Rotate(angle, a, line);
                Vertexes[i] = p;
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

        public override void Draw(Graphics g, Scene.Camera camera, Projection pr = 0)
        {
            foreach (var lst in Faces)
            {
                var t = new Triangle3D(Vertexes[lst[0]], Vertexes[lst[1]], Vertexes[lst[2]]);
                t.FindNormal(Center, camera);
                if (t.IsVisible)
                    t.Draw(g, camera, pr);
            }
        }

        //==============================================
        //
        // Gouraud_shading
        //
        //==============================================
        public void CalculateLambert(Point3D lightPos, Scene.Camera camera)
        {
            Dictionary<int, Point3D> pointNormal = new Dictionary<int, Point3D>();
            for (int i = 0; i < Vertexes.Count; i++)
            {
                List<List<int>> adjacentFaces = Faces.Where(x => x.Contains(i)).ToList();
                Point3D res = new Point3D(0, 0, 0);
                foreach (var face in adjacentFaces)
                {
                    var t = new Triangle3D(Vertexes[face[0]], Vertexes[face[1]], Vertexes[face[2]]);
                    res += t.FindNormalWorld(Center, camera);
                }
                res.X /= adjacentFaces.Count;
                res.Y /= adjacentFaces.Count;
                res.Z /= adjacentFaces.Count;
                res = res.Normalize();
                pointNormal.Add(i, res);
            }
            for (int i = 0; i < Vertexes.Count; i++)
            {
                var cos = ModelLambert(Vertexes[i], pointNormal[i], lightPos);
                Vertexes[i].illumination = Math.Max(cos, 0);
            }
        }

        private float ModelLambert(Point3D vertex, Point3D normal, Point3D lightPos)
        {
            Point3D rayLight = new Point3D(lightPos.X - vertex.X, lightPos.Y - vertex.Y, lightPos.Z - vertex.Z);
            rayLight = rayLight.Normalize();
            double cos = rayLight.DotProduct( normal) / (rayLight.Length * normal.Length);
            return (float)((cos ) );//перевод в диапазон [0,1]?
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

        public override void Apply(float[][] transform)
        {
            for (int i = 0; i < Vertexes.Count; i++)
            {
                Vertexes[i].Apply(transform);
            }
        }
        public void texturing(Bitmap map, Scene.Camera camera, Bitmap texture, float[,] zBuffer, Color[,] frameBuffer)
		{
            foreach (Triangle3D polygon in get_polygons())
            {
                Point3D[] polygonVertex = new Point3D[3];
                for (int i = 0; i < 3; i++)
                    polygonVertex[i] = polygon[i].Clone();

                PointF[] textureVertex = new PointF[3];
                textureVertex[0] = new PointF(0, 0);
                textureVertex[1] = new PointF(0, 1);
                textureVertex[2] = new PointF(1, 0);
                
                var p1 = polygonVertex[0];
                p1.X = (int)p1.GetPerspectiveProj(camera).X;
                p1.Y = (int)p1.GetPerspectiveProj(camera).Y;
                p1.Z = (int)(p1.Z );

                var p2 = polygonVertex[1];
                p2.X = (int)p2.GetPerspectiveProj(camera).X;
                p2.Y = (int)p2.GetPerspectiveProj(camera).Y;
                p2.Z = (int)(p2.Z );

                var p3 = polygonVertex[2];
                p3.X = (int)p3.GetPerspectiveProj(camera).X;
                p3.Y = (int)p3.GetPerspectiveProj(camera).Y;
                p3.Z = (int)(p3.Z );
                ConvertToRasterWithTexture(ref p1, ref p2, ref p3, textureVertex[0], textureVertex[1], textureVertex[2], texture, zBuffer, frameBuffer);
            }
            
        }

        private void ConvertToRasterWithTexture(ref Point3D p0, ref Point3D p1, ref Point3D p2, PointF tp0, PointF tp1, PointF tp2, Bitmap texture, float[,] zBuffer, Color[,] frameBuffer)
        {
            if (p1.Y < p0.Y)
            {
                var temp = p0;
                p0 = p1;
                p1 = temp;
            }

            if (p2.Y < p0.Y)
            {
                var temp = p0;
                p0 = p2;
                p2 = temp;
            }

            if (p2.Y < p1.Y)
            {
                var temp = p2;
                p2 = p1;
                p1 = temp;
            }

            var x01 = InterpolateList((int)p0.Y, (int)p0.X, (int)p1.Y, (int)p1.X);
            var x12 = InterpolateList((int)p1.Y, (int)p1.X, (int)p2.Y, (int)p2.X);
            var x02 = InterpolateList((int)p0.Y, (int)p0.X, (int)p2.Y, (int)p2.X);

            var z01 = InterpolateList((int)p0.Y, (int)p0.Z, (int)p1.Y, (int)p1.Z);
            var z12 = InterpolateList((int)p1.Y, (int)p1.Z, (int)p2.Y, (int)p2.Z);
            var z02 = InterpolateList((int)p0.Y, (int)p0.Z, (int)p2.Y, (int)p2.Z);

            var t01 = InterpolateTexture((int)p0.Y, tp0, (int)p1.Y, tp1);
            var t12 = InterpolateTexture((int)p1.Y, tp1, (int)p2.Y, tp2);
            var t02 = InterpolateTexture((int)p0.Y, tp0, (int)p2.Y, tp2);

            x01.Remove(x01.Last());
            List<int> x012 = new List<int>();
            x012.AddRange(x01);
            x012.AddRange(x12);

            z01.Remove(z01.Last());
            List<int> z012 = new List<int>();
            z012.AddRange(z01);
            z012.AddRange(z12);

            t01.Remove(t01.Last());
            List<PointF> t012 = new List<PointF>();
            t012.AddRange(t01);
            t012.AddRange(t12);

            var m = x012.Count / 2;
            List<int> x_left;
            List<int> x_right;

            List<int> z_left;
            List<int> z_right;

            List<PointF> lefttexture = new List<PointF>();
            List<PointF> righttexture = new List<PointF>();

            if (x02[m] < x012[m])
            {
                x_left = x02;
                x_right = x012;

                z_left = z02;
                z_right = z012;

                lefttexture = t02;
                righttexture = t012;
            }
            else
            {
                x_left = x012;
                x_right = x02;

                z_left = z012;
                z_right = z02;

                lefttexture = t012;
                righttexture = t02;
            }


            for (int y = (int)p0.Y; y < (int)p2.Y - 1; y++)
            {
                int x_l = x_left[(int)(y - p0.Y)];
                int x_r = x_right[(int)(y - p0.Y)];

                var z_segment = InterpolateList(x_l, z_left[(int)(y - p0.Y)], x_r, z_right[(int)(y - p0.Y)]);
                var texture_segment = InterpolateTexture(x_l, lefttexture[(int)(y - p0.Y)], x_r, righttexture[(int)(y - p0.Y)]);
                for (int x = x_l; x < x_r; x++)
                {
                    float depth = z_segment[x - x_l];

                    ApplyZBufferAlgorithmWithTexture(x, y, depth, texture_segment[x - x_l],texture, zBuffer, frameBuffer);
                }
            }
        }
        private List<int> InterpolateList(int x1, int y1, int x2, int y2)
        {
            List<int> res = new List<int>();
            if (x1 == x2)
            {
                res.Add(y2);
            }
            double step = (y2 - y1) * 1.0f / (x2 - x1);
            double y = y1;
            for (int i = x1; i <= x2; i++)
            {
                res.Add((int)y);
                y += step;
            }
            return res;
        }

        private List<PointF> InterpolateTexture(int x1, PointF t1, int x2, PointF t2)
        {
            List<PointF> res = new List<PointF>();
            if (x1 == x2)
            {
                res.Add(t1);
                res.Add(t1);
                return res;
            }
            PointF step = new PointF((t2.X - t1.X) / (x2 - x1), (t2.Y - t1.Y) / (x2 - x1));

            PointF y = t1;
            for (int i = x1; i <= x2; i++)
            {
                res.Add(y);
                y.X += step.X;
                y.Y += step.Y;
            }
            return res;
        }

        public void ApplyZBufferAlgorithmWithTexture(int x, int y, float depth, PointF color, Bitmap texture, float[,] zBuffer, Color [,] frameBuffer)
        {
            if ((x + (zBuffer.GetLength(1) / 2) >0)&& ((x + (zBuffer.GetLength(1)/2 ) < zBuffer.GetLength(1)))
                && (y + (zBuffer.GetLength(0) / 2) > 0) && ((y + (zBuffer.GetLength(0) / 2) < zBuffer.GetLength(0)))) {
                if (depth < zBuffer[x + (zBuffer.GetLength(1) / 2), y + (zBuffer.GetLength(0) / 2)])
                {
                    int tx = (int)(color.X * (texture.Width - 1));
                    int ty = (int)(color.Y * (texture.Height - 1));
                    frameBuffer[x + (zBuffer.GetLength(1) / 2), y + (zBuffer.GetLength(0) / 2)] = texture.GetPixel(tx, ty);
                    zBuffer[x + (zBuffer.GetLength(1) / 2), y + (zBuffer.GetLength(0) / 2)] = depth;
                }
            }
        }
        /*
        public void ApplyTexture(BitmapData bmpData, byte[] rgbValues, Bitmap texture, BitmapData bmpDataTexture, byte[] rgbValuesTexture, Scene.Camera camera)
        {
            foreach (var lst in Faces)
            {
                var f = new Triangle3D(Vertexes[lst[0]], Vertexes[lst[1]], Vertexes[lst[2]]);
                f.FindNormal(Center, camera);
                if (!f.IsVisible)
                    continue;

                // 3 vertices
                Point3D P0 = f[0];
                Point3D P1 = f[1];
                Point3D P2 = f[2];
                DrawTexture(P0, P1, P2, bmpData, rgbValues, texture, bmpDataTexture, rgbValuesTexture, camera);
            }
        }
        
        private static void DrawTexture(Point3D P0, Point3D P1, Point3D P2, BitmapData bmpData, byte[] rgbValues, Bitmap texture, BitmapData bmpDataTexture, byte[] rgbValuesTexture, Scene.Camera camera)
        {
            // Отсортируйте точки так, чтобы y0 <= y1 <= y2D            
            var points = SortTriangleVertices(P0, P1, P2, camera);//РАСТЕРИЗОВАТЬ ПОЛИГОН то же самое
            Point3D SortedP0 = points[0], SortedP1 = points[1], SortedP2 = points[2];

            // Вычислите координаты x и U, V текстурных координат ребер треугольника
            var x01 = NewInterpolate((int)SortedP0.Y, SortedP0.X, (int)SortedP1.Y, SortedP1.X);
            var u01 = NewInterpolate((int)SortedP0.Y, SortedP0.TextureCoordinates.X, (int)SortedP1.Y, SortedP1.TextureCoordinates.X);
            var v01 = NewInterpolate((int)SortedP0.Y, SortedP0.TextureCoordinates.Y, (int)SortedP1.Y, SortedP1.TextureCoordinates.Y);
            var x12 = NewInterpolate((int)SortedP1.Y, SortedP1.X, (int)SortedP2.Y, SortedP2.X);
            var u12 = NewInterpolate((int)SortedP1.Y, SortedP1.TextureCoordinates.X, (int)SortedP2.Y, SortedP2.TextureCoordinates.X);
            var v12 = NewInterpolate((int)SortedP1.Y, SortedP1.TextureCoordinates.Y, (int)SortedP2.Y, SortedP2.TextureCoordinates.Y);
            var x02 = NewInterpolate((int)SortedP0.Y, SortedP0.X, (int)SortedP2.Y, SortedP2.X);
            var u02 = NewInterpolate((int)SortedP0.Y, SortedP0.TextureCoordinates.X, (int)SortedP2.Y, SortedP2.TextureCoordinates.X);
            var v02 = NewInterpolate((int)SortedP0.Y, SortedP0.TextureCoordinates.Y, (int)SortedP2.Y, SortedP2.TextureCoordinates.Y);

            // Concatenate the short sides
            x01 = x01.Take(x01.Length - 1).ToArray(); // remove last element, it's the first in x12
            var x012 = x01.Concat(x12).ToArray();
            u01 = u01.Take(u01.Length - 1).ToArray(); // remove last element, it's the first in u12
            var u012 = u01.Concat(u12).ToArray();
            v01 = v01.Take(v01.Length - 1).ToArray(); // remove last element, it's the first in v12
            var v012 = v01.Concat(v12).ToArray();

            // Determine which is left and which is right
            int m = x012.Length / 2;
            double[] x_left, x_right, u_left, u_right, v_left, v_right;
            if (x02[m] < x012[m])
            {
                x_left = x02;
                x_right = x012;
                u_left = u02;
                u_right = u012;
                v_left = v02;
                v_right = v012;
            }
            else
            {
                x_left = x012;
                x_right = x02;
                u_left = u012;
                u_right = u02;
                v_left = v012;
                v_right = v02;
            }

            // Рисует горизонтальные сегменты
            for (int y = (int)SortedP0.Y; y < (int)SortedP2.Y; ++y)
            {
                int screen_y = -y + camera.Height / 2;
                if (screen_y < 0)
                    break;
                if (camera.Height <= screen_y)
                    continue;

                var x_l = x_left[y - (int)SortedP0.Y];
                var x_r = x_right[y - (int)SortedP0.Y];

                var u_segment = NewInterpolate((int)x_l, u_left[y - (int)SortedP0.Y], (int)x_r, u_right[y - (int)SortedP0.Y]);
                var v_segment = NewInterpolate((int)x_l, v_left[y - (int)SortedP0.Y], (int)x_r, v_right[y - (int)SortedP0.Y]);
                for (int x = (int)x_l; x < (int)x_r; ++x)
                {
                    int screen_x = x + camera.Width / 2;
                    if (screen_x < 0)
                        continue;
                    if (camera.Width <= screen_x)
                        break;

                    int texture_u = (int)(u_segment[x - (int)x_l] * (texture.Width - 1));
                    int texture_v = (int)(v_segment[x - (int)x_l] * (texture.Height - 1));

                    rgbValues[screen_x * 3 + screen_y * bmpData.Stride] = rgbValuesTexture[texture_u * 3 + texture_v * bmpDataTexture.Stride];
                    rgbValues[screen_x * 3 + 1 + screen_y * bmpData.Stride] = rgbValuesTexture[texture_u * 3 + 1 + texture_v * bmpDataTexture.Stride];
                    rgbValues[screen_x * 3 + 2 + screen_y * bmpData.Stride] = rgbValuesTexture[texture_u * 3 + 2 + texture_v * bmpDataTexture.Stride];
                }
            }
        }

        private static double[] NewInterpolate(int i0, double d0, int i1, double d1)
        {
            if (i0 == i1)
                return new double[] { d0 };
            double[] values = new double[i1 - i0 + 1];
            double a = (d1 - d0) / (i1 - i0);
            double d = d0;

            int ind = 0;
            for (int i = i0; i <= i1; ++i)
            {
                values[ind++] = d;
                d += a;
            }

            return values;
        }

        private static Point3D[] SortTriangleVertices(Point3D P0, Point3D P1, Point3D P2, Scene.Camera camera)
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
            Point3D[] points = new Point3D[3] {P0,P1,P2 };
            return points;
        }*/
    }
}
