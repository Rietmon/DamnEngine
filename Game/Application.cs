using System;
using System.Linq;
using System.Threading.Tasks;
using DamnEngine.Render;
using DamnEngine.Utilities;
using OpenTK;
using OpenTK.Input;
using Rietmon.Extensions;

namespace DamnEngine
{
    public static class Application
    {
        public static RenderWindow Window { get; set; }
        public static Action OnNextFrameUpdate { get; set; }

        public static void Initialize()
        {
            Window.VSync = VSyncMode.On;
            
            ScenesManager.SetScene(new Scene("New Scene"));

            var cameraObject = new GameObject("Camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.SetData(0.7853982f, 800f / 600f, 0.01f, 2000);
            camera.AddComponent<GameCamera>();

            var shader = Shader.CreateFromFile("Default");
            var texture = Texture2D.CreateFromFile("dark.png");
            var mesh = Mesh.CreateFromFile("Man.obj").First();
            var material = new Material(shader);
            material.SetTexture(0, texture);
            
            for (var x = -10; x < 11; x++)
            {
                for (var y = -10; y < 11; y++)
                {
                    var obj = new GameObject($"Obj {x} {y}");
                    var meshRender = obj.AddComponent<MeshRenderer>();
                    meshRender.Material = material;
                    meshRender.Mesh = mesh;
                    obj.Transform.Position = new Vector3(x * 2, 0, y * 2);
                }
            }
            
            GC.Collect();
        }

        public static void Update()
        {
            ScenesManager.CurrentScene.ForEachGameObjectComponent((component) => component.OnPreUpdate());
            
            OnNextFrameUpdate?.Invoke();
            OnNextFrameUpdate = null;
            ScenesManager.CurrentScene.ForEachGameObjectComponent((component) => component.OnUpdate());
            
            ScenesManager.CurrentScene.ForEachGameObjectComponent((component) => component.OnPostUpdate());
        }
    }
}