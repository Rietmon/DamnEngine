using System;
using DamnEngine.Render;
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
            meshRenderer.Mesh = new Mesh
            {
                Vertices = new[]
                {
                    new Vector3(-1.0f, -1.0f, 1.0f),
                    new Vector3( 1.0f, -1.0f, 1.0f),
                    new Vector3( 1.0f, 1.0f, 1.0f),
                    new Vector3(-1.0f, 1.0f, 1.0f),
                    new Vector3(-1.0f, -1.0f, -1.0f),
                    new Vector3(1.0f, -1.0f, -1.0f),
                    new Vector3(1.0f, 1.0f, -1.0f),
                    new Vector3(-1.0f, 1.0f, -1.0f)
                },
                Uv = new[]
                {
                    new Vector2(0, 0),
                    new Vector2( 0, 1),
                    new Vector2( 1, 1),
                    new Vector2(1, 0),
                    new Vector2(0, 0),
                    new Vector2( 0, 1),
                    new Vector2( 1, 1),
                    new Vector2(1, 0),
                },
                Indices = new[] 
                {
                    0, 1, 2, 2, 3, 0,
                    3, 2, 6, 6, 7, 3,
                    7, 6, 5, 5, 4, 7,
                    4, 0, 3, 3, 7, 4,
                    0, 1, 5, 5, 4, 0,
                    1, 5, 6, 6, 2, 1 
                }
            };

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