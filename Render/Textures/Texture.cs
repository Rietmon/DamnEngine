using OpenTK.Graphics.OpenGL;

namespace DamnEngine.Render
{
    public abstract class Texture : DamnObject
    {
        private readonly int texturePointer;

        protected Texture(int texturePointer)
        {
            this.texturePointer = texturePointer;
        }

        public void Use(int textureUnit) => UseTexture(texturePointer, textureUnit);

        protected override void OnDestroy()
        {
            GL.DeleteTexture(texturePointer);
        }

        protected static void UseTexture(int texturePointer, int textureUnit)
        {
            textureUnit += 33984;
            
            GL.ActiveTexture((TextureUnit)textureUnit);
            GL.BindTexture(TextureTarget.Texture2D, texturePointer);
        }
    }
}