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
            renderWindow.Resize += OnWindowResize;
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
            Rendering.Resolution = renderWindow.Size;
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
            DamnObjectsFactory.UpdateFactory(PipelineTiming.OnBeginFrame);
            
            if (Input.IsKeyPress(KeyCode.Escape))
                renderWindow.Close();

            Time.Update((float)arguments.Time);
            
#if ENABLE_STATISTICS
            renderWindow.Title = $"DamnEngine 1.0 | FPS: {(int)(1f / Time.DeltaTime)} | Faces: {Statistics.TotalFacesDrawled} | Meshes: {Statistics.TotalMeshesDrawled}";
#endif
            
            renderWindow.ProcessEvents();

            Application.Update();
            
#if ENABLE_STATISTICS
            Statistics.Clear();
#endif
            
            Rendering.BeginRender();
            
            renderWindow.SwapBuffers();
            
            DamnObjectsFactory.UpdateFactory(PipelineTiming.OnInputUpdate);
            Input.Update(renderWindow);
            
            DamnObjectsFactory.UpdateFactory(PipelineTiming.OnEndFrame);
        }

        private static void OnWindowResize(ResizeEventArgs arguments)
        {
            Rendering.Resolution = arguments.Size;
        }

        private static void OnClosing(CancelEventArgs arguments)
        {
            Debug.Log($"[{nameof(Engine)}] ({nameof(OnClosing)}) Shutdown engine...");
        }
    }
}