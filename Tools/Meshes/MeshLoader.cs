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
    public static class MeshLoader
    {
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
                    var parts = line.Split(' ');
                    Point3D p1 = new Point3D(float.Parse(parts[1], new CultureInfo("en-US")), float.Parse(parts[2], new CultureInfo("en-US")), float.Parse(parts[3], new CultureInfo("en-US")));
                    line = fs.ReadLine().Trim();
                    parts = line.Split(' ');
                    Point3D p2 = new Point3D(float.Parse(parts[1], new CultureInfo("en-US")), float.Parse(parts[2], new CultureInfo("en-US")), float.Parse(parts[3], new CultureInfo("en-US")));
                    line = fs.ReadLine().Trim();
                    parts = line.Split(' ');
                    Point3D p3 = new Point3D(float.Parse(parts[1], new CultureInfo("en-US")), float.Parse(parts[2], new CultureInfo("en-US")), float.Parse(parts[3], new CultureInfo("en-US")));
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
    }
}
