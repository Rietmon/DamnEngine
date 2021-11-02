using System;
using System.Linq;
using System.Threading.Tasks;
using DamnEngine.Render;
using DamnEngine.Utilities;
using OpenTK;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
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
            
            Physics.Initialize();
            
            ScenesManager.SetScene(new Scene("New Scene"));

            var cameraObject = new GameObject("Camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.SetData(0.7853982f, 800f / 600f, 0.01f, 2000);
            camera.AddComponent<GameCamera>();

            var shader = Shader.CreateFromFile("Light");
            var texture = Texture2D.CreateFromFile("dark.png");
            var mesh = Mesh.CreateFromFile("Man.obj").First();
            var material = new Material(shader);
            material.SetTexture(0, texture);
            
            for (var x = -25; x < 26; x++)
            {
                for (var y = -25; y < 26; y++)
                {
                    var obj = new GameObject($"Obj {x} {y}");
                    var meshRender = obj.AddComponent<MeshRenderer>();
                    meshRender.Material = material;
                    meshRender.Mesh = mesh;
                    obj.Transform.Position = new Vector3(x, 0, y);
                }
            }
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