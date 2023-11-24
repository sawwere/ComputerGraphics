using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IndTask2.MainForm;

namespace IndTask2
{
    public class Mesh
    {
        public Material material;

        public List<Vector3> Vertexes { get; private set; } = new List<Vector3>();

        public List<List<int>> Faces { get; private set; } = new List<List<int>>();

        public Vector3 Center
        {
            get
            {
                var res = new Vector3(0, 0, 0);
                foreach (var f in Vertexes)
                {
                    res += f;
                }
                res = (1f / Vertexes.Count) * res;
                return res;
            }
        }

        public Mesh() 
        {
            Vertexes = new List<Vector3>();
            Faces = new List<List<int>>();
            material = Material.Wall(new Vector3(1, 1, 1));
        }

        public Mesh(List<Vector3> points, List<List<int>> faces)
        {
            Vertexes = new List<Vector3>();
            Faces = new List<List<int>>();
            material = Material.Wall(new Vector3(1, 1, 1));
            foreach (Vector3 point in points)
            {
                Vertexes.Add(new Vector3(point.X, point.Y, point.Z));
            }
            foreach (var item in faces)
            {
                Faces.Add(item);
            }
        }

        public static Mesh Hexahedron(float size)
        {
            var Vertexes = new List<Vector3>()
            {
                new Vector3(size, -size, size), new Vector3(size, -size, -size), new Vector3(size, size, -size), new Vector3(size, size, size),
                new Vector3(-size, size, size), new Vector3(-size, size, -size), new Vector3(-size, -size, -size), new Vector3(-size, -size, size)
            };
            var Faces = new List<List<int>>()
            {
                new List<int>(){ 0, 1, 2}, new List<int>(){ 0, 2, 3}, //right
                new List<int>(){ 4, 5, 6}, new List<int>(){ 4, 6, 7}, //left
                new List<int>(){ 3, 4, 7}, new List<int>(){ 3, 7, 0},
                new List<int>(){ 3, 2, 5}, new List<int>(){ 3, 5, 4}, //up
                new List<int>(){ 1, 6, 5}, new List<int>(){ 1, 5, 2},
                new List<int>(){ 7, 6, 1}, new List<int>(){ 7, 1, 0}
            };
            return new Mesh(Vertexes, Faces);
        }

        public Mesh Clone()
        {
            Mesh res = new Mesh(Vertexes, Faces);
            return res;
        }

        public void Translate(Vector3 vec)
        {
            for (int i = 0; i < Vertexes.Count; i++)
            {
                var p = Vertexes[i];
                p.Translate(vec.X, vec.Y, vec.Z);
                Vertexes[i] = p;
            }
        }

        public void Rotate(Vector3 vec)
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

        public void Scale(Vector3 vec)
        {
            for (int i = 0; i < Vertexes.Count; i++)
            {
                var p = Vertexes[i];
                p.Scale(vec.X, vec.Y, vec.Z);
                Vertexes[i] = p;
            }
        }

        public bool triangle_intersection(Vector3 orig, Vector3 dir, Vector3 v0, Vector3 v1, Vector3 v2, out float param) 
        {
            param = 0;
            Vector3 e1 = v1 - v0;
            Vector3 e2 = v2 - v0;
            // Вычисление вектора нормали к плоскости
            Vector3 pvec = dir.CrossProduct( e2);
            float det = e1.DotProduct( pvec);

            // Луч параллелен плоскости
            if (det< 1e-8 && det> -1e-8) 
            {
                return false;
            }

            float inv_det = 1 / det;
            Vector3 tvec = orig - v0;
            float u = tvec.DotProduct( pvec) * inv_det;
            if (u< 0 || u> 1) 
            {
                return false;
            }

            Vector3 qvec = tvec.CrossProduct(e1);
            float v = dir.DotProduct( qvec) * inv_det;
            if (v< 0 || u + v> 1)
            {
                return false;
            }
            param = e2.DotProduct(qvec) * inv_det;
            if (param > 0)
                return true;
            return false;
        }


        public virtual bool Intersection(Ray r, out float intersectParam, out Vector3 normal)
        {
            intersectParam = 0;
            normal = null;
            int side = -1;

            for (int i = 0; i < Faces.Count; ++i)
            {
                bool tt = triangle_intersection(r.origin, r.dest, Vertexes[Faces[i][0]], Vertexes[Faces[i][1]], Vertexes[Faces[i][2]], out float param);
                if ((intersectParam == 0 || param < intersectParam) && tt)
                {
                    intersectParam = param - 1e-4f;
                    side = i;
                }
            }

            if (intersectParam != 0)
            {
                var storona_1 = Vertexes[Faces[side][1]] - Vertexes[Faces[side][0]];
                var storona_2 = Vertexes[Faces[side][2]] - Vertexes[Faces[side][0]];
                normal = storona_1.CrossProduct(storona_2).Normalize();
                return true;
            }

            return false;
        }
    }
}
