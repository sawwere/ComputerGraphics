using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Tools.Primitives;
using System.Globalization;


using Tools.FastBitmap;
using Tools.Primitives;
using Tools;
using Tools.Scene;
using Tools.Meshes;
namespace Tools.Meshes
{
    public static class MeshBuilder
    {
        private static Point3D ParseVertex(string line)
        {

            var parts = line.Split(' ');
            return new Point3D(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
        }

        public static Mesh LoadFromFile(string path)
        {
            List<Triangle3D> triangles = new List<Triangle3D>();
            using (var fs = new StreamReader(path))
            {
                while (!fs.EndOfStream)
                {
                    string line = fs.ReadLine(); 
                    if (line.StartsWith("solid") || line.StartsWith("endsolid"))
                        continue;
                    fs.ReadLine(); // skip outer loop
                    line = fs.ReadLine().Trim();
                    Point3D p1 = ParseVertex(line);
                    line = fs.ReadLine().Trim();
                    Point3D p2 = ParseVertex(line);
                    line = fs.ReadLine().Trim();
                    Point3D p3 = ParseVertex(line);
                    triangles.Add(new Triangle3D(p1, p2, p3));
                    fs.ReadLine(); // skip endloop
                    fs.ReadLine(); // skip endfacet
                }
            }
            Mesh res = new Mesh(triangles);
            return res;
        }

        public static void SaveToFile(string path, Mesh mesh,string name)
        {
            using (var fr=new StreamWriter(path, false))
            {
                fr.WriteLine("solid "+name);
                
                var p=mesh.get_poligons();
                for (int i = 0; i <p.Count ; i++)
                {
                    //var points = p[i][0].X;
                    fr.WriteLine("facet normal 0.0 0.0 0.0");
                    fr.WriteLine("  outer loop");
                    fr.WriteLine("    vertex "+p[i][0].X+" "+p[i][0].Y+" "+p[i][0].Z);
                    fr.WriteLine("    vertex " + p[i][1].X + " " + p[i][1].Y + " " + p[i][1].Z);
                    fr.WriteLine("    vertex " + p[i][2].X + " " + p[i][2].Y + " " + p[i][2].Z);
                    fr.WriteLine("  endloop");
                    fr.WriteLine("endfacet");
                }
                fr.WriteLine("endsolid " + name);
            }
        }

        //TODO
        public static Mesh BuildRotationFigure(List<Point3D> points, Axis axis, int steps)
        {
            Mesh mesh = new Mesh();
            var polygons = new List<Triangle3D>();
            List<Point3D> rotatedPoints = new List<Point3D>();
            float angle = 360f / steps;
            for (int i = 0; i < steps; ++i)
            {
                rotatedPoints = new List<Point3D>();
                foreach (var p in points)
                {
                    p.Rotate(angle, axis);
                    rotatedPoints.Add(new Point3D(p.X, p.Y, p.Z));
                }

                for (int j = 1; j < rotatedPoints.Count; ++j)
                {

                    polygons.Add(new Triangle3D(new List<Point3D>()
                        {
                                rotatedPoints[j - 1],
                                points[j - 1],
                                points[j]
                        }));
                    polygons.Add(new Triangle3D(new List<Point3D>()
                        {
                                rotatedPoints[j - 1],
                                points[j],
                                rotatedPoints[j]
                        }));
                }

                //for (int h = 0; h < points.Count; h++) { points[h].Rotate(angle, axis); }
                //rotatedPoints.Clear();
                points.Clear();
                foreach (var p in rotatedPoints)
                {
                    points.Add(p);
                }
                //points = new List<Point3D>();
            }
            mesh = new Mesh(polygons);
            mesh.Translate(-1*mesh.Center);

            return mesh;
        }

        //TODO
        public static (Mesh, Point3D) BuildFunctionFigure(int x0, int y0, int x1, int y1, int steps, Func<float, float, float> func)
        {
            var polygons = new List<Triangle3D>();

            float stepX = (x1 - x0) / (steps + 1e-7f);
            float stepY = (y1 - y0) / (steps + 1e-7f);
            float dx = -(x1 + x0) / 2.0f;
            float dy = -(y1 + y0) / 2.0f;

            List<Point3D> pointsPrev = new List<Point3D>();
            List<Point3D> points = new List<Point3D>();
            float minZ = float.MaxValue;
            float maxZ = float.MinValue;
            for (float xi = 0; xi <= steps; xi++)
            {
                for (float yi = 0; yi <= steps; yi++)
                {
                    float z = func(x0 + xi*stepX, y0 + yi * stepY);
                    if (z > maxZ)
                        maxZ = z;
                    if (z <  minZ)
                        minZ = z;
                    points.Add(new Point3D(dx + x0 + xi * stepX, dy + y0 + yi * stepY, z));
                }

                if (pointsPrev.Count != 0)
                    for (int i = 1; i < pointsPrev.Count; ++i)
                    {
                        polygons.Add(new Triangle3D(new List<Point3D>()
                        {
                                pointsPrev[i - 1],
                                points[i - 1],
                                points[i]
                        }));
                        polygons.Add(new Triangle3D(new List<Point3D>()
                        {
                                pointsPrev[i - 1],
                                points[i],
                                pointsPrev[i]
                        }));
                    }
                pointsPrev.Clear();
                pointsPrev = points;
                points = new List<Point3D>();
            }
            Mesh mesh = new Mesh(polygons);
            mesh.Translate(new Point3D(0, 0, -(maxZ - minZ) / 2.0f));
            return (mesh, new Point3D(dx, dy, minZ -(maxZ - minZ) / 2.0f));
        }
    }
}
