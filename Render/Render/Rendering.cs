using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace DamnEngine.Render
{
    public static class Rendering
    {
        public static Matrix4 ViewMatrix { get; set; }
        
        public static Matrix4 ProjectionMatrix { get; set; }
        public static readonly List<IRenderer> renderers = new();

        public static void Initialize()
        {
            Graphics.ClearColor(Color.Black);

            GL.Enable(EnableCap.DepthTest);
            
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
        }

        public static void RenderFrame(RenderWindow window)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            foreach (var renderer in renderers)
                renderer.OnPreRendering();
            
            foreach (var renderer in renderers)
                renderer.OnRendering();
            
            foreach (var renderer in renderers)
                renderer.OnPostRendering();
            
            window.SwapBuffers();
        }
    }
}