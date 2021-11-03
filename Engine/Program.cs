using System.ComponentModel;
using DamnEngine.Render;
using OpenTK.Windowing.Common;

namespace DamnEngine
{
    public static class Program
    {
        private static RenderWindow renderWindow;
        
        private static void Main()
        {
            Debug.Log($"[{nameof(Program)}] ({nameof(Main)}) Starting engine...");
            Debug.OnCrash += () => OnClosing(null);
            
            Debug.Log($"[{nameof(Program)}] ({nameof(Main)}) Creating window...");
            renderWindow = new RenderWindow("DamnEngine 1.0", 800, 600);
            
            Debug.Log($"[{nameof(Program)}] ({nameof(Main)}) Registering callbacks...");
            renderWindow.Load += OnLoad;
            renderWindow.UpdateFrame += OnUpdateFrame;
            renderWindow.Closing += OnClosing;
            
            renderWindow.KeyDown += (arguments) => Input.OnKeyDown(arguments.Key);
            renderWindow.KeyUp += (arguments) => Input.OnKeyUp(arguments.Key);

            Debug.Log($"[{nameof(Program)}] ({nameof(Main)}) Starting window...");
            renderWindow.Run();
            
            Debug.Log($"[{nameof(Program)}] ({nameof(Main)}) Engine started!");
        }

        private static void OnLoad()
        {
            Debug.Log($"[{nameof(Program)}] ({nameof(OnLoad)}) Initializing engine...");
            
            Debug.Log($"[{nameof(Program)}] ({nameof(OnLoad)}) Initializing render...");
            Rendering.Initialize();

            Debug.Log($"[{nameof(Program)}] ({nameof(OnLoad)}) Initializing application API...");
            Input.MouseState = renderWindow.MouseState;
            Application.Window = renderWindow;
            Application.Initialize();
            
            Debug.Log($"[{nameof(Program)}] ({nameof(OnLoad)}) Engine has been initialized!");
        }

        private static void OnUpdateFrame(FrameEventArgs arguments)
        {
            Time.DeltaTime = (float)arguments.Time;
            
            renderWindow.Title = $"DamnEngine 1.0 | FPS: {(int)(1f / Time.DeltaTime)} | Faces: {Statistics.TotalFacesDrawled}";
            
            renderWindow.ProcessEvents();

            Application.Update();
            
            Statistics.TotalFacesDrawled = 0;

            Rendering.RenderFrame(renderWindow);
            
            Input.Update(renderWindow.MouseState);
        }

        private static void OnClosing(CancelEventArgs arguments)
        {
            Debug.Log($"[{nameof(Program)}] ({nameof(OnClosing)}) Shutdown engine...");
        }
    }
}