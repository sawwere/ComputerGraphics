using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Tools.Primitives;

namespace Tools.Scene
{
    public class Scene
    {
        private Dictionary<Guid, SceneObject> sceneObjects;
        public List<Light> lights;
        public Camera camera;

        public Scene(Dictionary<Guid, SceneObject> objects, List<Light> lights, Camera camera)
        {
            sceneObjects = objects;
            this.lights = lights;
            this.camera = camera;
        }

        public Scene()
        {
            sceneObjects = new Dictionary<Guid, SceneObject>();
            lights = new List<Light>();
            camera = new Camera(600, 600, new Primitives.Point3D(0,0,-1000), new Primitives.Point3D(0,0,1));
        }

        public Dictionary<Guid, SceneObject> GetAllSceneObjects()
        {
            var res = new Dictionary<Guid, SceneObject>();
            foreach (var pair in sceneObjects)
                if (!pair.Value.Name.StartsWith("_axis"))
                    res[pair.Key] = pair.Value;
            return res;
        }

        public void Clear()
        {
            var res = new Dictionary<Guid, SceneObject>();
            foreach (var pair in sceneObjects)
                if (pair.Value.Name.StartsWith("_axis"))
                    res[pair.Key] = pair.Value;
            sceneObjects = res;
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
        }

        public void RemoveObject(SceneObject obj)
        {
            sceneObjects.Remove(obj.Id);
        }

        public SceneObject GetObject(Guid id)
        {
            return sceneObjects[id];
        }

        float Interpolate(float x0, float y0, float x1, float y1, float i)
        {
            if (Math.Abs(x0 - x1) < 1e-5)
                return (y0 + y1) / 2;
            return y0 + ((y1 - y0) * (i - x0)) / (x1 - x0);
        }

        public Bitmap RasterizedRender(Projection pr)
        {
            var bitmap = new Bitmap(camera.width, camera.height);
            using (var fs = new FastBitmap.FastBitmap(bitmap))
            {
                float[] buff = new float[fs.Width * fs.Height];
                for (int i = 0; i < fs.Width * fs.Height; ++i)
                    buff[i] = float.MaxValue;
                if (GetAllSceneObjects().Count > 0)
                {
                    foreach (SceneObject obj in GetAllSceneObjects().Values)
                    {
                        Primitive m = obj.GetTransformed();
                        m.Translate(-1 * camera.position);
                        (m as Mesh).calculateZBuffer(camera, fs.Width, fs.Height, ref buff);
                    }
                    var maxZ = buff.Where(x => x < float.MaxValue).Max();
                    var minZ = buff.Where(x => x > float.MinValue).Min();
                    for (int x = 0; x < fs.Width; ++x)
                        for (int y = 0; y < fs.Height; ++y)
                        {
                            var cd = buff[x + fs.Width * y];
                            if (buff[x + fs.Width * y] < float.MaxValue)
                            {

                                Color c = Color.FromArgb(
                                    (int)Interpolate(minZ, 128, maxZ, 1, buff[x + fs.Width * y]),
                                    (int)Interpolate(minZ, 128, maxZ, 1, buff[x + fs.Width * y]),
                                    (int)Interpolate(minZ, 128, maxZ, 1, buff[x + fs.Width * y]));
                                fs[x, y] = c;
                            }
                            else
                            {
                                fs[x, y] = Color.White;
                            }
                        }
                }
            }
            return bitmap;
        }

        public void Render(Graphics g, Projection pr = 0, Pen pen = null)
        {
            foreach (SceneObject obj in sceneObjects.Values)
            {
                Tools.Primitives.Primitive m = obj.GetTransformed();
                //TODO move figure in all projections ??
                //if (pr == Projection.PERSPECTIVE)
                {
                    m.Translate(-1 * camera.position);
                }
                m.Draw(g, camera, pr, pen);
            }
        }
    }
}
