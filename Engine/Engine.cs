using System;
using System.ComponentModel;
using DamnEngine.Render;
using OpenTK.Windowing.Common;

namespace DamnEngine
{
    public static class Engine
    {
        public static Action OnEngineStarted { get; set; }
        
        private static RenderWindow renderWindow;
        
        public static void Run()
        {
            Debug.Log($"[{nameof(Engine)}] ({nameof(Run)}) Starting engine...");
            Debug.OnCrash += () => OnClosing(null);
            
            Debug.Log($"[{nameof(Engine)}] ({nameof(Run)}) Creating window...");
            renderWindow = new RenderWindow("DamnEngine 1.0", 800, 600);
            
            Debug.Log($"[{nameof(Engine)}] ({nameof(Run)}) Registering callbacks...");
            renderWindow.Load += OnLoad;
            renderWindow.UpdateFrame += OnUpdateFrame;
            renderWindow.Closing += OnClosing;
            
            renderWindow.KeyDown += (arguments) => Input.OnKeyDown(arguments.Key);
            renderWindow.KeyUp += (arguments) => Input.OnKeyUp(arguments.Key);

            Debug.Log($"[{nameof(Engine)}] ({nameof(Run)}) Starting window...");
            renderWindow.Run();
        }

        private static void OnLoad()
        {
            Debug.Log($"[{nameof(Engine)}] ({nameof(OnLoad)}) Initializing engine...");
            
            Debug.Log($"[{nameof(Engine)}] ({nameof(OnLoad)}) Initializing input...");
            Input.Initialize(renderWindow);
            
            Debug.Log($"[{nameof(Engine)}] ({nameof(OnLoad)}) Initializing render...");
            Rendering.Initialize();

            Debug.Log($"[{nameof(Engine)}] ({nameof(OnLoad)}) Initializing application API...");
            Input.MouseState = renderWindow.MouseState;
            Application.Window = renderWindow;
            Application.Initialize();
            
            Debug.Log($"[{nameof(Engine)}] ({nameof(OnLoad)}) Engine has been initialized!");
            
            OnEngineStarted?.Invoke();
        }

        private static void OnUpdateFrame(FrameEventArgs arguments)
        {
            if (Input.IsKeyPress(KeyCode.Escape))
                renderWindow.Close();
            
            Time.DeltaTime = (float)arguments.Time;
            
            renderWindow.Title = $"DamnEngine 1.0 | FPS: {(int)(1f / Time.DeltaTime)} | Faces: {Statistics.TotalFacesDrawled} | Statics: {Physics.Simulation.Statics.Count} | Bodies: {Physics.Simulation.Bodies.ActiveSet.Count}";
            
            renderWindow.ProcessEvents();

            Application.Update();
            
            Statistics.TotalFacesDrawled = 0;

            Rendering.RenderFrame(renderWindow);
            
            Input.Update(renderWindow);
        }

        private static void OnClosing(CancelEventArgs arguments)
        {
            Debug.Log($"[{nameof(Engine)}] ({nameof(OnClosing)}) Shutdown engine...");
        }
    }
}