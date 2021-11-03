using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace DamnEngine.Render
{
    public class Texture2D : Texture
    {
        private static readonly Dictionary<Bitmap, BitmapData> bitmapBits = new();
        
        public string OriginalTextureName { get; }
        public int Width { get; }
        public int Height { get; }
        public Bitmap Bitmap { get; }
        
        public Texture2D(int texturePointer, int width, int height, Bitmap bitmap, string originalTextureName) : base(texturePointer)
        {
            Width = width;
            Height = height;
            Bitmap = bitmap;
            OriginalTextureName = originalTextureName;
            Name = originalTextureName;
        }

        public static Texture2D CreateFromFile(string name)
        {
            var texturePointer = GL.GenTexture();
            UseTexture(texturePointer, 0);

            var bitmap = ResourcesLoader.UseBitmap(name);

            if (!bitmapBits.TryGetValue(bitmap, out var bitmapData))
            {
                bitmapData = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                bitmapBits.Add(bitmap, bitmapData);
            }
            
            GL.TexImage2D(TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                bitmap.Width,
                bitmap.Height,
                0,
                PixelFormat.Bgra,
                PixelType.UnsignedByte,
                bitmapData.Scan0);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return new Texture2D(texturePointer, bitmap.Width, bitmap.Height, bitmap, name);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            ResourcesLoader.FreeBitMap(OriginalTextureName);
        }
    }
}