using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DamnEngine.Render;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace DamnEngine
{
    public static class Application
    {
        public static RenderWindow Window { get; set; }
        public static Action OnNextFrameUpdate { get; set; }

        public static void Initialize()
        {
            Window.VSync = VSyncMode.On;
            Input.GrabMouse = true;
            Cursor.Hide();
            
            Physics.Initialize();
            
            ScenesManager.SetScene(new Scene("New Scene"));

            var cameraObject = new GameObject("Camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.AddComponent<GameCamera>();

            var texture = Texture2D.CreateFromFile("dark.png");
            var mesh = Mesh.CreateFromFile("Cube.obj").First();

            CreateDynamicCube(new Vector3(0,2,0), 1, mesh, texture);
            // for (var x = 0; x < 26; x++)
            // {
            //     for (var y = 0; y < 26; y++)
            //     {
            //         CreateDynamicCube(new Vector3(x, 10, y), (x + 1) * y, mesh, texture);
            //     }
            // }

            var obj1 = new GameObject("ColliderCube1");
            var mr = obj1.AddComponent<MeshRenderer>();
            var material1 = Material.CreateFromShadersFiles("Light");
            obj1.Transform.Position = new Vector3(0, -5, 0);
            obj1.Transform.Scale = new Vector3(100, 1, 100);
            obj1.AddComponent<BoxCollider>();
            material1.SetTexture(0, texture);
            mr.Material = material1;
            mr.Mesh = mesh;
            obj1.AddComponent<TestRotation>();
        }

        private static void CreateDynamicCube(Vector3 pos, int num, Mesh mesh, Texture tex)
        {
            var obj = new GameObject($"PhysicsCube{num}");
            var meshRender = obj.AddComponent<MeshRenderer>();
            var material = Material.CreateFromShadersFiles("Light");
            material.SetTexture(0, tex);
            obj.Transform.Position = pos;
            obj.AddComponent<BoxCollider>();
            obj.AddComponent<RigidBody>();
            meshRender.Material = material;
            meshRender.Mesh = mesh;
        }

        public static void Update()
        {
            ScenesManager.CurrentScene.ForEachGameObjectComponent((component) => component.OnPreUpdate());
            
            OnNextFrameUpdate?.Invoke();
            OnNextFrameUpdate = null;
            ScenesManager.CurrentScene.ForEachGameObjectComponent((component) => component.OnUpdate());
            
            Physics.Update(Time.DeltaTime);
            
            ScenesManager.CurrentScene.ForEachGameObjectComponent((component) => component.OnPostUpdate());
        }
    }
}