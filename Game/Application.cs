using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DamnEngine.Render;
using DamnEngine.Serialization;
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
            
            var gridTexture = Texture2D.CreateFromFile("dark.png");
            var cubeMesh = Mesh.CreateFromFile("Cube.obj").First();
            var sphereMesh = Mesh.CreateFromFile("Sphere.obj").First();
            var springboardMesh = Mesh.CreateFromFile("Springboard.obj").First();

            // var plane = CreateObject("Plane", cubeMesh, gridTexture);
            // plane.Transform.Position = new Vector3(0,-4,0);
            // plane.Transform.LocalScale = new Vector3(100, 1, 100);
            // plane.AddComponent<BoxCollider>();

            var springboard = CreateObject("Springboard", springboardMesh, gridTexture);
            springboard.Transform.Position = new Vector3(0,-30,-10);
            springboard.AddComponent<MeshCollider>().Mesh = springboardMesh;

            // var cube = CreateObject("Cube", cubeMesh, gridTexture);
            // cube.AddComponent<BoxCollider>();
            // cube.AddComponent<RigidBody>();

            var sphere = CreateObject("Sphere", sphereMesh, gridTexture);
            sphere.Transform.Position = new Vector3(0, 0, 0);
            sphere.AddComponent<SphereCollider>();
            sphere.AddComponent<RigidBody>();
            sphere.AddComponent<TestPhysics>();
        }

        private static GameObject CreateObject(string name, Mesh mesh, Texture texture)
        {
            var obj = new GameObject(name);
            var mr = obj.AddComponent<MeshRenderer>();
            var material = Material.CreateFromShadersFiles("Default");
            material.SetTexture(0, texture);
            mr.Material = material;
            mr.Mesh = mesh;
            return obj;
        }

        public static void Update()
        {
            ScenesManager.CurrentScene.ForEachActiveGameObjectEnabledComponent((component) => component.OnPreUpdate());
            
            OnNextFrameUpdate?.Invoke();
            OnNextFrameUpdate = null;
            ScenesManager.CurrentScene.ForEachActiveGameObjectEnabledComponent((component) => component.OnUpdate());
            
            Physics.Update(Time.DeltaTime);
            
            ScenesManager.CurrentScene.ForEachActiveGameObjectEnabledComponent((component) => component.OnPostUpdate());
        }
    }
}