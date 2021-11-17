﻿using System;
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


            for (var x = 0; x < 10; x++)
            {
                for (var y = 0; y < 10; y++)
                {
                    Create(new Vector3(x * 2, 0, y * 2), texture, mesh);
                }
            }
        }

        public static void Create(Vector3 pos, Texture texture, Mesh mesh)
        {
            var obj1 = new GameObject("Plane");
            var mr = obj1.AddComponent<MeshRenderer>();
            var material1 = Material.CreateFromShadersFiles("Light");
            obj1.Transform.Position = pos;
            obj1.AddComponent<BoxCollider>();
            material1.SetTexture(0, texture);
            mr.Material = material1;
            mr.Mesh = mesh;
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