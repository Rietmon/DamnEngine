using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Rietmon.Extensions;

namespace DamnEngine.Render
{
    public unsafe class RenderTexture : Texture
    {
        public int TextureSizeInBytes => Width * Height * 4;
        
        public int Width { get; }
        public int Height { get; }

        public Vector2i Resolution => new(Width, Height);

        private readonly int renderBufferPointer;
        private readonly int frameBufferPointer;

        public RenderTexture(int width, int height, int texturePointer, int renderBufferPointer, int frameBufferPointer) : base(texturePointer)
        {
            Width = width;
            Height = height;
            this.renderBufferPointer = renderBufferPointer;
            this.frameBufferPointer = frameBufferPointer;
        }

        public void UseToFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferPointer);
            Rendering.SetViewport(Vector2i.Zero, Resolution);
        }

        public void UnUseFromFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            Rendering.SetViewport(Vector2i.Zero, Rendering.Resolution);
        }

        public Color32[] ReadPixels()
        {
            Use();
            var pixels = new Color32[TextureSizeInBytes / 4];
            GL.ReadPixels(0, 0, Width, Height, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
            return pixels;
        }

        public IntPtr ReadPixelsAsPointer()
        {
            Use();
            var pixelsPointer = (IntPtr)MemoryUtilities.Allocate(TextureSizeInBytes);
            GL.ReadPixels(0, 0, Width, Height, PixelFormat.Rgba, PixelType.UnsignedByte, pixelsPointer);
            return pixelsPointer;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            GL.DeleteRenderbuffer(renderBufferPointer);
            GL.DeleteFramebuffer(frameBufferPointer);
        }

        public static RenderTexture Create(int width, int height)
        {
            var frameBufferPointer = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferPointer);
            
            var texturePointer = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texturePointer);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, width, height, 0, 
                PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, new[] { (int)TextureMinFilter.Nearest });
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, new[] { (int)TextureMinFilter.Nearest });

            var renderBufferPointer = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, renderBufferPointer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, width, height);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment,
                RenderbufferTarget.Renderbuffer, renderBufferPointer);
            
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, texturePointer, 0);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment0);

            if (Debug.LogErrorAssert(
                    GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) ==
                    FramebufferErrorCode.FramebufferComplete,
                    $"[{nameof(RenderTexture)}] ({nameof(Create)}) Unable to create renderTexture!"))
                Debug.LogError(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer));
            
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);  

            return new RenderTexture(width, height, texturePointer, renderBufferPointer, frameBufferPointer);
        }
    }
}