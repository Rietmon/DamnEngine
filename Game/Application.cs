﻿using System;
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
            var texture = Texture2D.CreateFromFile("dark.png");
            var mesh = Mesh.CreateFromFile("Cube.obj").First();
            
            var obj1 = new GameObject("Plane");
            var mr = obj1.AddComponent<MeshRenderer>();
            var material1 = Material.CreateFromShadersFiles("Default");
            obj1.Transform.Position = new Vector3(0,-4,0);
            obj1.Transform.Scale = new Vector3(100, 1, 100);
            obj1.AddComponent<BoxCollider>();
            material1.SetTexture(0, texture);
            mr.Material = material1;
            mr.Mesh = mesh;
            
            obj1 = new GameObject("Cube");
            mr = obj1.AddComponent<MeshRenderer>();
            material1 = Material.CreateFromShadersFiles("Default");
            obj1.Transform.Position = new Vector3(0,0,0);
            obj1.AddComponent<TestRotation>();
            material1.SetTexture(0, texture);
            mr.Material = material1;
            mr.Mesh = mesh;
            var obj2 = obj1;
            
            obj1 = new GameObject("SubCube");
            mr = obj1.AddComponent<MeshRenderer>();
            material1 = Material.CreateFromShadersFiles("Default");
            obj1.Transform.Position = new Vector3(0,2,0);
            obj1.Transform.Parent = obj2;
            material1.SetTexture(0, texture);
            mr.Material = material1;
            mr.Mesh = mesh;
            
            File.WriteAllText(Paths.AppDataPath + "test.dat", obj1.SerializationObject.Serialize());
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