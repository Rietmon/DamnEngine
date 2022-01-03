using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace DamnEngine.Render
{
    public static class Rendering
    {
        public static Matrix4 ViewMatrix { get; set; }
        
        public static Matrix4 ProjectionMatrix { get; set; }

        public static Action OnBeginRendering { get; set; }

        public static Action OnPreRendering { get; set; }

        public static Action OnRendering { get; set; }
        
        public static Action OnPostRendering { get; set; }
        
        public static RenderParameters RenderParameters { get; set; }

        public static void SetViewport(Vector2i position, Vector2i size) =>
            GL.Viewport(position.X, position.Y, size.X, size.Y);
        
        public static void Initialize()
        {
            Graphics.ClearColor(Color.Black);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
        }

        public static void BeginRender()
        {
            OnBeginRendering?.Invoke();
        }

        public static void RenderFrame()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            GL.ClearDepth(1);

            OnPreRendering?.Invoke();
            
            OnRendering?.Invoke();
            
            OnPostRendering?.Invoke();
        }
    }
}