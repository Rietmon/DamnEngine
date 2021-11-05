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
            camera.SetData(0.7853982f, 800f / 600f, 0.01f, 2000);
            camera.AddComponent<GameCamera>();

            var texture = Texture2D.CreateFromFile("dark.png");
            var mesh = Mesh.CreateFromFile("Cube.obj").First();
            
            var obj = new GameObject($"PhysicsCube1");
            var meshRender = obj.AddComponent<MeshRenderer>();
            var material = Material.CreateFromShadersFiles("Light");
            material.SetTexture(0, texture);
            obj.Transform.Position = new Vector3(0, 10, 0);
            obj.AddComponent<BoxCollider>();
            obj.AddComponent<RigidBody>();
            meshRender.Material = material;
            meshRender.Mesh = mesh;
            
            // var obj2 = new GameObject($"PhysicsCube2");
            // var material2 = Material.CreateFromShadersFiles("Light");
            // material2.SetTexture(0, texture);
            // material2.SetColor("color", Color.Red);
            // meshRender = obj2.AddComponent<MeshRenderer>();
            // meshRender.Material = material;
            // meshRender.Mesh = mesh;
            // obj2.Transform.Position = new Vector3(0, 102, 0);
            // obj2.Transform.Parent = obj.Transform;

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