using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Tools.Primitives;
using System.Globalization;

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

        public static void SaveToFile(string path, Mesh mesh)
        {
            
        }

        //TODO
        public static Mesh BuildRotationFigure(List<Point2D> points)
        {

            Mesh mesh = new Mesh();
                
            return mesh;
        }

        //TODO
        public static Mesh BuildFunctionFigure(List<Point2D> points)
        {

            Mesh mesh = new Mesh();

            return mesh;
        }
    }
}
