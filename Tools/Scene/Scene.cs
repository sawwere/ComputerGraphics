using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

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
