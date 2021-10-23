using System;
using DamnEngine.Render;
using OpenTK;
using OpenTK.Input;

namespace DamnEngine
{
    public static class Program
    {
        private static RenderWindow renderWindow;
        
        private static void Main()
        {
            Debug.Log($"[{nameof(Program)}] ({nameof(Main)}) Starting engine...");
            
            Debug.Log($"[{nameof(Program)}] ({nameof(Main)}) Creating window...");
            renderWindow = new RenderWindow("DamnEngine 1.0", 800, 600);
            
            Debug.Log($"[{nameof(Program)}] ({nameof(Main)}) Registering callbacks...");
            renderWindow.Load += OnLoad;
            renderWindow.UpdateFrame += OnUpdateFrame;

            renderWindow.KeyDown += (_, arguments) => Input.OnKeyDown(arguments.Key);
            renderWindow.KeyUp += (_, arguments) => Input.OnKeyUp(arguments.Key);
            
            Debug.Log($"[{nameof(Program)}] ({nameof(Main)}) Starting window...");
            renderWindow.Run(60, 60);
            
            Debug.Log($"[{nameof(Program)}] ({nameof(Main)}) Engine started!");
        }

        private static void OnLoad(object sender, EventArgs arguments)
        {
            Debug.Log($"[{nameof(Program)}] ({nameof(OnLoad)}) Initializing engine...");
            
            Debug.Log($"[{nameof(Program)}] ({nameof(OnLoad)}) Initializing render...");
            Rendering.Initialize();

            Debug.Log($"[{nameof(Program)}] ({nameof(OnLoad)}) Initializing application API...");
            Application.Window = renderWindow;
            Application.Initialize();
            
            Debug.Log($"[{nameof(Program)}] ({nameof(OnLoad)}) Engine has been initialized!");
        }

        private static void OnUpdateFrame(object sender, FrameEventArgs arguments)
        {
            Time.DeltaTime = (float)arguments.Time;
            
            renderWindow.Title = $"DamnEngine 1.0 | FPS: {1f / Time.DeltaTime}";
            
            renderWindow.ProcessEvents();

            Application.Update();
            
            Rendering.RenderFrame(renderWindow);
            
            Input.Update();
        }
    }
}