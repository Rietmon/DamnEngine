using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace DamnEngine.Render
{
    public static class Rendering
    {
        public static Matrix4 ViewMatrix { get; set; }
        
        public static Matrix4 ProjectionMatrix { get; set; }

        public static Action OnPreRendering { get; set; }

        public static Action OnRendering { get; set; }
        
        public static Action OnPostRendering { get; set; }

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

            OnPreRendering?.Invoke();
            
            OnRendering?.Invoke();
            
            OnPostRendering?.Invoke();
            
            window.SwapBuffers();
        }
    }
}