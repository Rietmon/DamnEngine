using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace DamnEngine.Render
{
    public static class Rendering
    {
        public static Matrix4 ViewMatrix { get; set; }
        
        public static Matrix4 ProjectionMatrix { get; set; }
        
        public static Action OnRenderFrame { get; set; }

        public static void Initialize()
        {
            GL.ClearColor(Color.Black);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.Blend);
        }

        public static void RenderFrame(RenderWindow window)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            OnRenderFrame.Invoke();
            
            window.SwapBuffers();
        }
    }
}