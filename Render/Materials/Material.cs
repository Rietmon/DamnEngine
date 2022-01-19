using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace DamnEngine.Render
{
    public class Material : LowLevelDamnObject
    {
        public static Material DefaultMaterial => CreateFromShadersFiles("Default");
        
        public static Material InvalidMaterial => CreateFromShadersFiles("Invalid");
        
        public int TexturesCount => textures.Count;

        public int[] TexturesPointers => textures.Keys.ToArray();
        
        #region Quick Properties
        public Color ColorProperty { set => SetColor("color", value); }
        public Matrix4 ModelProperty { set => SetMatrix4("model", value); }
        public Matrix4 ViewProperty { set => SetMatrix4("view", value); }
        public Matrix4 ProjectionProperty { set => SetMatrix4("projection", value); }
        public Texture Texture0Property { set => SetTexture(0, value); }
        public Texture Texture1Property { set => SetTexture(1, value); }
        public Texture Texture2Property { set => SetTexture(2, value); }
        public Texture Texture3Property { set => SetTexture(3, value); }
        public Texture Texture4Property { set => SetTexture(4, value); }
        public Texture Texture5Property { set => SetTexture(5, value); }
        public Texture Texture6Property { set => SetTexture(6, value); }
        public Texture Texture7Property { set => SetTexture(7, value); }
        public Texture Texture8Property { set => SetTexture(8, value); }
        public Texture Texture9Property { set => SetTexture(9, value); }
        #endregion
        
        private readonly Shader shader;

        private readonly Dictionary<int, Texture> textures = new();

        private Material(Shader shader)
        {
            this.shader = shader;
        }

        public void Use()
        {
            foreach (var texture in textures) 
                texture.Value.Use(texture.Key);
            
            shader.Use();
        }

        public int GetAttributeLocation(string attributeName) => shader.GetAttributeLocation(attributeName);
        
        public void SetInt(string name, int value)
        {
            shader.Use();
            GL.Uniform1(shader.GetUniformLocation(name), value);
        }
        
        public void SetFloat(string name, float value)
        {
            shader.Use();
            GL.Uniform1(shader.GetUniformLocation(name), value);
        }

        public void SetVector2(string name, Vector2 value)
        {
            shader.Use();
            GL.Uniform2(shader.GetUniformLocation(name), value);
        }

        public void SetVector3(string name, Vector3 value)
        {
            shader.Use();
            GL.Uniform3(shader.GetUniformLocation(name), value);
        }

        public void SetVector4(string name, Vector4 value)
        {
            shader.Use();
            GL.Uniform4(shader.GetUniformLocation(name), value);
        }

        public void SetColor(string name, Color value)
        {
            shader.Use();
            GL.Uniform4(shader.GetUniformLocation(name), value);
        }
        
        public void SetMatrix4(string name, Matrix4 value)
        {
            shader.Use();
            GL.UniformMatrix4(shader.GetUniformLocation(name), true, ref value);
        }

        public void SetTexture(int index, Texture value)
        {
            textures.GetOrAdd(index, value);
            
            value.Use(index);
                
            SetInt($"texture{index}", index);
        }

        protected override void OnDestroy()
        {
            shader.Destroy();
        }

        public static Material CreateFromShader(Shader shader, bool compileShader = true)
        {
            if (compileShader && !shader.IsCompiled)
                shader.Compile();
            return new Material(shader);
        }

        public static Material CreateFromShadersFiles(string shadersName, bool compileShader = true)
        {
            var shader = Shader.CreateFromFiles(shadersName);
            if (compileShader)
                shader.Compile();
            return new Material(shader);
        }

        public static Material CreateFromShadersFiles(string vertexShaderName, string fragmentShaderName,
            bool compileShader = true)
        {
            var shader = Shader.CreateFromFiles(vertexShaderName, fragmentShaderName);
            if (compileShader)
                shader.Compile();
            return new Material(shader);
        }
    }
}