﻿using System.Drawing;
using System.IO;
using DamnEngine.Render;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common.Input;
using Rietmon.Extensions;
using Graphics = DamnEngine.Render.Graphics;
using Image = System.Drawing.Image;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace DamnEngine
{
    public partial class Camera : Component
    {
        public static Camera CurrentRenderingCamera { get; private set; }
        
        public static Camera Main { get; private set; }
        
        public RenderTexture RenderTexture { get; set; }

        public Frustum Frustum { get; private set; }
        
        private float fov;
        private float aspectRatio;
        private float near;
        private float far;

        protected internal override void OnCreate()
        {
            Main = this;

            fov = 0.7853982f;
            var windowSize = Application.Window.Bounds.Max;
            aspectRatio = (float)windowSize.X / (float)windowSize.Y;
            near = 0.01f;
            far = 2000f;

            UpdateProjectionMatrix();
            UpdateViewMatrix();
            UpdateFrustum();

            Rendering.OnBeginRendering += OnBeginRendering;
        }

        private void OnBeginRendering()
        {
            CurrentRenderingCamera = this;
            
            if (RenderTexture)
            {
                RenderTexture.UseToFrameBuffer();
                Rendering.RenderFrame();
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
                Rendering.SetViewport(Vector2i.Zero, Application.WindowResolution);
            }
            else
            {
                Rendering.RenderFrame();
            }

            CurrentRenderingCamera = null;
        }

        protected internal override void OnTransformChanged()
        {
            UpdateViewMatrix();
            UpdateFrustum();
        }

        protected override void OnDestroy()
        {
            Rendering.OnBeginRendering -= OnBeginRendering;
        }
    }
}