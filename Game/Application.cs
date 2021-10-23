using System;
using DamnEngine.Render;
using DamnEngine.Utilities;
using OpenTK;

namespace DamnEngine
{
    public static class Application
    {
        public static RenderWindow Window { get; set; }
        public static Action OnNextFrameUpdate { get; set; }

        public static void Initialize()
        {
            ScenesManager.SetScene(new Scene("New Scene"));

            var cameraObject = new GameObject("Camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.SetData(0.7853982f, 800f / 600f, 0.01f, 2000);
            camera.AddComponent<GameCamera>();

            var quadObject = new GameObject("Quad");
            var meshRenderer = quadObject.AddComponent<MeshRenderer>();
            meshRenderer.Mesh = WavefrontObjParser.Parse("GameData/Meshes/Cube.obj");

            var texture = Texture2D.CreateFromFile("dark.png");
            var shader = Shader.CreateFromFile("Default");
            var material = new Material(shader);
            material.SetTexture(0, texture);

            meshRenderer.Material = material;
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