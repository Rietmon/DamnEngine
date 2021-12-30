using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace DamnEngine.Render
{
    public static class Graphics
    {
        public static void ClearColor(Color color) => GL.ClearColor(color);

        public static void DrawPixels(Vector2i position, Color32[] pixels) => GL.DrawPixels(position.X, position.Y,
            PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
    }
}