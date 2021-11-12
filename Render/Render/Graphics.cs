using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace DamnEngine.Render
{
    public static class Graphics
    {
        public static void ClearColor(Color color) => GL.ClearColor(color);

        public static void DrawRay(Vector3 position, Vector3 direction, float length)
        {
            var lineVertices = new[]
            {
                position.X, position.Y, position.Z,
                position.X * direction.X * length, position.Y * direction.Y * length, position.Z * direction.Z * length
            };
            
            GL.VertexPointer(3, VertexPointerType.Float, 0, lineVertices);
            GL.DrawArrays(BeginMode.Lines, 0, 2);
        }
    }
}