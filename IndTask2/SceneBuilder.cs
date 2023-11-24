using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndTask2
{
    class SceneBuilder
    {
        private List<Mesh> scene;
        public float ROOM_SIZE { get; private set; }

        public SceneBuilder(float size)
        {
            ROOM_SIZE = size;
            scene = new List<Mesh>();
        }

        public void AddForwardWall(Color color, bool isMirrored)
        {
            var frontWall = new Mesh(new List<Vector3>() {
                new Vector3(ROOM_SIZE, ROOM_SIZE, -ROOM_SIZE), new Vector3(-ROOM_SIZE, ROOM_SIZE, -ROOM_SIZE),
                new Vector3(-ROOM_SIZE, -ROOM_SIZE, -ROOM_SIZE), new Vector3(ROOM_SIZE, -ROOM_SIZE, -ROOM_SIZE)},
                new List<List<int>>() { new List<int>() { 0, 1, 2 }, new List<int>() { 0, 2, 3 } });
            if (isMirrored)
            {
                frontWall.material = Material.Mirror();
            }
            else
            {
                frontWall.material = Material.Wall(color);
            }
            scene.Add(frontWall);
        }

        public void AddBackWall(Color color, bool isMirrored)
        {
            var backWall = new Mesh(new List<Vector3>() {
                new Vector3(ROOM_SIZE, ROOM_SIZE, ROOM_SIZE), new Vector3(-ROOM_SIZE, ROOM_SIZE, ROOM_SIZE),
                new Vector3(-ROOM_SIZE, -ROOM_SIZE, ROOM_SIZE), new Vector3(ROOM_SIZE, -ROOM_SIZE, ROOM_SIZE)},
                new List<List<int>>() { new List<int>() { 0, 1, 2 }, new List<int>() { 0, 2, 3 } });
            if (isMirrored)
            {
                backWall.material = Material.Mirror();
            }
            else
            {
                backWall.material = Material.Wall(color);
            }
            scene.Add(backWall);
        }

        public void AddLeftWall(Color color, bool isMirrored)
        {
            var leftWall = new Mesh(new List<Vector3>() {
                new Vector3(-ROOM_SIZE, ROOM_SIZE, ROOM_SIZE), new Vector3(-ROOM_SIZE, ROOM_SIZE, -ROOM_SIZE),
                new Vector3(-ROOM_SIZE, -ROOM_SIZE, -ROOM_SIZE), new Vector3(-ROOM_SIZE, -ROOM_SIZE, ROOM_SIZE)},
                new List<List<int>>() { new List<int>() { 0, 1, 2 }, new List<int>() { 0, 2, 3 } });
            if (isMirrored)
            {
                leftWall.material = Material.Mirror();
            }
            else
            {
                leftWall.material = Material.Wall(color);
            }
            scene.Add(leftWall);
        }

        public void AddRightWall(Color color, bool isMirrored)
        {
            var rightWall = new Mesh(new List<Vector3>() {
                new Vector3(ROOM_SIZE, -ROOM_SIZE, ROOM_SIZE), new Vector3(ROOM_SIZE, -ROOM_SIZE, -ROOM_SIZE),
                new Vector3(ROOM_SIZE, ROOM_SIZE, -ROOM_SIZE), new Vector3(ROOM_SIZE, ROOM_SIZE, ROOM_SIZE)},
                new List<List<int>>() { new List<int>() { 0, 1, 2 }, new List<int>() { 0, 2, 3 } });
            if (isMirrored)
            {
                rightWall.material = Material.Mirror();
            }
            else
            {
                rightWall.material = Material.Wall(color);
            }

            scene.Add(rightWall);
        }

        public void AddUpperWall(Color color, bool isMirrored)
        {
            var upperWall = new Mesh(new List<Vector3>() {
                new Vector3(ROOM_SIZE, ROOM_SIZE, ROOM_SIZE), new Vector3(ROOM_SIZE, ROOM_SIZE, -ROOM_SIZE),
                new Vector3(-ROOM_SIZE, ROOM_SIZE, -ROOM_SIZE), new Vector3(-ROOM_SIZE, ROOM_SIZE, ROOM_SIZE)},
                new List<List<int>>() { new List<int>() { 0, 1, 2 }, new List<int>() { 0, 2, 3 } });
            upperWall.material = Material.Wall(Color.White);
            if (isMirrored)
            {
                upperWall.material = Material.Mirror();
            }
            else
            {
                upperWall.material = Material.Wall(color);
            }
            scene.Add(upperWall);
        }

        public void AddBottomWall(Color color, bool isMirrored)
        {
            var bottomWall = new Mesh(new List<Vector3>() {
                new Vector3(ROOM_SIZE, -ROOM_SIZE, ROOM_SIZE), new Vector3(ROOM_SIZE, -ROOM_SIZE, -ROOM_SIZE),
                new Vector3(-ROOM_SIZE, -ROOM_SIZE, -ROOM_SIZE), new Vector3(-ROOM_SIZE, -ROOM_SIZE, ROOM_SIZE)},
                new List<List<int>>() { new List<int>() { 2, 1, 0 }, new List<int>() { 3, 2, 0 } });
            if (isMirrored)
            {
                bottomWall.material = Material.Mirror();
            }
            else
            {
                bottomWall.material = Material.Wall(color);
            }
            scene.Add(bottomWall);
        }

        public void AddCube(float size, Color color, MainForm.MaterialType mat, Vector3 offset, Vector3 rotation, Vector3 scale)
        {
            Mesh cube = Mesh.Hexahedron(size);
            cube.Scale(scale);
            cube.Rotate(rotation);
            cube.Translate(offset);

            switch (mat)
            {
                case MainForm.MaterialType.WALL:
                    cube.material = new Material(0.0f, 0.0f, 0.1f, 0.8f, 1.0f, color);
                    break;
                case MainForm.MaterialType.MIRROR:
                    cube.material = Material.Mirror();
                    break;
                case MainForm.MaterialType.TRANSPARENT:
                    cube.material = Material.Transparent();
                    break;
            }
            scene.Add(cube);
        }

        public void AddSphere(Vector3 pos, float radius, Color color, MainForm.MaterialType mat)
        {
            Sphere sphere = new Sphere(pos, radius);
            switch (mat)
            {
                case MainForm.MaterialType.WALL:
                    sphere.material = new Material(0.0f, 0.0f, 0.1f, 0.8f, 1.0f, color);
                    break;
                case MainForm.MaterialType.MIRROR:
                    sphere.material = Material.Mirror();
                    break;
                case MainForm.MaterialType.TRANSPARENT:
                    sphere.material = Material.Transparent();
                    break;
            }
            scene.Add(sphere);
        }

        public List<Mesh> Get()
        {
            return scene;
        }
    }
}
