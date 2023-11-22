using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Tools.Primitives;

namespace Tools.Scene
{
    public class Scene
    {
        public Dictionary<string, SceneObject> systemObjects;
        private Dictionary<Guid, SceneObject> sceneObjects;
        public SceneObject Light;
        public Camera Camera { get; private set; }
        
        public Scene(Camera camera)
        {
            sceneObjects = new Dictionary<Guid, SceneObject>();
            Camera = camera;

            systemObjects = new Dictionary<string, SceneObject>();
            var axisLineX = new Edge3D(new Point3D(0, 0, 0), new Point3D(2, 0, 0), Color.Red);
            var axisLineY = new Edge3D(new Point3D(0, 0, 0), new Point3D(0, 2, 0), Color.Green);
            var axisLineZ = new Edge3D(new Point3D(0, 0, 0), new Point3D(0, 0, -2), Color.Blue);
            var line_1 = new Edge3D(new Point3D(0, 0, 0), new Point3D(0, 0, 0), Color.Purple);
            systemObjects.Add("axisLineX", new SceneObject(axisLineX, "axisLineX"));
            systemObjects.Add("axisLineY", new SceneObject(axisLineY, "axisLineY"));
            systemObjects.Add("axisLineZ", new SceneObject(axisLineZ, "axisLineZ"));
            systemObjects.Add("axisLineRotation", new SceneObject(line_1, "axisLineRotation"));
        }

        public Dictionary<Guid, SceneObject> GetAllSceneObjects()
        {
            var res = new Dictionary<Guid, SceneObject>();
            foreach (var pair in sceneObjects)
                res[pair.Key] = pair.Value;
            return res;
        }

        private IEnumerable<Primitive> GetAllTransformedMeshes()
        {
            return sceneObjects.Select(x => x.Value.GetTransformed(Camera)).Where(x=>x is Mesh);
        }

        public void Clear()
        {
            sceneObjects.Clear();
        }

        public int Count()
        {
             return sceneObjects.Count;
        }

        public void AddObject(SceneObject obj)
        {
            StringBuilder figureName = new StringBuilder(obj.Name);
            int i = 1;
            while (sceneObjects.Any(x=>x.Value.Name==figureName.ToString()))
            {
                figureName.Append(i);
                i++;
            }
            obj.Name = figureName.ToString();
            sceneObjects.Add(obj.Id, obj);

            if (obj.Local is Light)
            {
                Light = obj;
            }
        }

        public void RemoveObject(SceneObject obj)
        {
            sceneObjects.Remove(obj.Id);
        }

        public SceneObject GetObject(Guid id)
        {
            return sceneObjects[id];
        }

        private float Interpolate(float x0, float y0, float x1, float y1, float i)
        {
            if (Math.Abs(x0 - x1) < 1e-5)
                return (y0 + y1) / 2;
            return y0 + ((y1 - y0) * (i - x0)) / (x1 - x0);
        }

        public Bitmap RasterizedRender(Projection pr)
        {
            var bitmap = new Bitmap(Camera.Width, Camera.Height);
            
            using (var fs = new FastBitmap.FastBitmap(bitmap))
            {
                Point3D[] buff = new Point3D[fs.Width * fs.Height];
                for (int i = 0; i < fs.Width * fs.Height; ++i)
                    buff[i] = buff[i] = new Point3D(0, 0, float.MaxValue);
                var meshes = GetAllTransformedMeshes();
                if (meshes.Count() > 0)
                {
                    foreach (var obj in meshes)
                    {
                        (obj as Mesh).CalculateZBuffer(Camera, buff);
                    }
                    var filtered = buff.Select(x=>x.Z).Where(z => z < float.MaxValue);
                    if (filtered.Count() > 0)
                    {
                        var maxZ = filtered.Max();
                        var minZ = filtered.Min();
                        for (int x = 0; x < fs.Width; x++)
                            for (int y = 0; y < fs.Height; y++)
                            {
                                var cd = buff[x + fs.Width * y];
                                if (buff[x + fs.Width * y].Z < float.MaxValue)
                                {

                                    Color c = Color.FromArgb(
                                        (int)Interpolate(minZ, 128, maxZ, 1, buff[x + fs.Width * y].Z),
                                        (int)Interpolate(minZ, 128, maxZ, 1, buff[x + fs.Width * y].Z),
                                        (int)Interpolate(minZ, 128, maxZ, 1, buff[x + fs.Width * y].Z));
                                    fs[x, y] = c;
                                }
                                else
                                {
                                    fs[x, y] = Color.LightGray;
                                }
                            }
                    }
                }
            }
            return bitmap;
        }

        public Bitmap GourodRender(Projection pr)
        {
            
            var bitmap = new Bitmap(Camera.Width, Camera.Height);
            using (var fs = new FastBitmap.FastBitmap(bitmap))
            {
                Point3D[] buff = new Point3D[fs.Width * fs.Height];
                for (int i = 0; i < fs.Width * fs.Height; ++i)
                    buff[i] = new Point3D(0, 0, float.MaxValue);
                var meshes = GetAllTransformedMeshes();
                Light transformedLight = (Light)Light.GetTransformed(Camera);
                if (meshes.Count() > 0)
                {
                    foreach (var obj in meshes)
                    {
                        (obj as Mesh).CalculateLambert(transformedLight.position, Camera);
                        (obj as Mesh).CalculateZBuffer(Camera, buff);
                        
                    }
                    for (int x = 0; x < fs.Width; x++)
                        for (int y = 0; y < fs.Height; y++)
                        {
                            var cd = buff[x + fs.Width * y];
                            if (cd.Z < float.MaxValue)
                            {
                                fs[x, y] = Color.FromArgb((int)(255 * cd.illumination), 
                                    (int)(0 * cd.illumination), 
                                    (int)(0 * cd.illumination));
                            }
                            else
                            {
                                fs[x, y] = Color.LightGray;
                            }
                        }
                }
            }
            return bitmap;
        }

        public void Render(Graphics g, Projection pr = 0)
        {
            foreach (SceneObject obj in systemObjects.Values)
            {
                Primitive m = obj.GetTransformed(Camera);
                m.Draw(g, Camera, pr);
            }
            foreach (SceneObject obj in sceneObjects.Values)
            {
                Primitive m = obj.GetTransformed(Camera);
                //TODO move figure in all projections ??
                //if (pr == Projection.PERSPECTIVE)
                m.Draw(g, Camera, pr);
            }
        }

        public void MoveCamera(Point3D vec)
        {
            Camera.Translate(vec);
            //TODO some magic here?
        }

        public void RotateCamera(Point3D vec)
        {
            Camera.Rotate(vec);
            //TODO some magic here?
        }
    }
}
