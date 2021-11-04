using System;
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
            var mesh = Mesh.CreateFromFile("Man.obj").First();
            
            var obj = new GameObject($"Man");
            var meshRender = obj.AddComponent<MeshRenderer>();
            var material = Material.CreateFromShadersFiles("Light");
            var collider = obj.AddComponent<BoxCollider>();
            collider.Dynamic();
            material.SetTexture(0, texture);
            meshRender.Material = material;
            meshRender.Mesh = mesh;

            var planeMesh = Mesh.CreateFromFile("Cube.obj").First();
            var obj1 = new GameObject("Plane");
            var mr = obj1.AddComponent<MeshRenderer>();
            var material1 = Material.CreateFromShadersFiles("Light");
            collider = obj1.AddComponent<BoxCollider>();
            collider.Static();
            material1.SetTexture(0, texture);
            mr.Material = material1;
            mr.Mesh = planeMesh;
            obj1.Transform.Position = new Vector3(0, -10, 0);
            obj1.Transform.Scale = Vector3.One;
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