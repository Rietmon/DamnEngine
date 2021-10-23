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
            renderWindow = new RenderWindow("DamnEngine", 800, 600);
            
            renderWindow.Load += OnLoad;
            renderWindow.UpdateFrame += OnUpdateFrame;

            renderWindow.KeyDown += (_, arguments) => Input.OnKeyDown(arguments.Key);
            renderWindow.KeyUp += (_, arguments) => Input.OnKeyUp(arguments.Key);
            
            renderWindow.Run(60);
        }

        private static void OnLoad(object sender, EventArgs arguments)
        {
            Rendering.Initialize();

            Application.Window = renderWindow;
            Application.Initialize();
        }

        private static void OnUpdateFrame(object sender, FrameEventArgs arguments)
        {
            Time.DeltaTime = (float)arguments.Time;
            
            renderWindow.ProcessEvents();

            Application.Update();
            
            Rendering.RenderFrame(renderWindow);
            
            Input.Update();
        }
    }
}