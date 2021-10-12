using OpenTK;
using OpenTK.Graphics;

namespace DamnEngine
{
    public class RenderWindow : GameWindow
    {
        public RenderWindow(string windowName, int width, int height) : base(width, height, GraphicsMode.Default,
            windowName) { }
    }
}