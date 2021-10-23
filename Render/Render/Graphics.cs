using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace DamnEngine.Render
{
    public static class Graphics
    {
        public static void ClearColor(Color color) => GL.ClearColor(color);
    }
}