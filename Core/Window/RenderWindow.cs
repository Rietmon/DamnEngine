using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace DamnEngine
{
    public class RenderWindow : GameWindow
    {
        public RenderWindow(string windowName, int width, int height)
            : base(new GameWindowSettings(){ UpdateFrequency = 60, RenderFrequency = 60, IsMultiThreaded = false},
                new NativeWindowSettings() { Size = new Vector2i(width, height), Title = windowName })
        {
            
        }
    }
}