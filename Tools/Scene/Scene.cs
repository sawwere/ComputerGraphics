using System;
using System.Collections.Generic;
using System.Linq;
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
            camera = new Camera(new Primitives.Point3D(0,0,-1000), new Primitives.Point3D(0,0,1));
        }

        public Dictionary<Guid, SceneObject> GetAllSceneObjects()
        {
            return sceneObjects;
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
        }

        public void RemoveObject(SceneObject obj)
        {
            sceneObjects.Remove(obj.Id);
        }

        public SceneObject GetObject(Guid id)
        {
            return sceneObjects[id];
        }
    }
}
