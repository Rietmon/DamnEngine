using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace DamnEngine.Render
{
    public static class Graphics
    {
        public static void ClearColor(Color color) => GL.ClearColor(color);
    }
}