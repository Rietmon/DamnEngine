using System;
using DamnEngine.Render;
using OpenTK;

namespace DamnEngine
{
    public static class Application
    {
        private static RenderWindow renderWindow;
        
        private static void Main(string[] args)
        {
            renderWindow = new RenderWindow("DamnEngine", 800, 600);
            
            renderWindow.Load += OnLoad;
            renderWindow.UpdateFrame += OnUpdateFrame;
            
            renderWindow.Run(60);
        }

        private static void OnLoad(object sender, EventArgs arguments)
        {
            Rendering.Initialize();
        }

        private static void OnUpdateFrame(object sender, FrameEventArgs arguments)
        {
            Time.DeltaTime = (float)arguments.Time;
            
            Rendering.RenderFrame(renderWindow);
        }
    }
}