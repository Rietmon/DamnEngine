﻿using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace DamnEngine.Render
{
    public class Material : DamnObject
    {
        public int TexturesCount => textures.Count;

        public int[] TexturesPointers => textures.Keys.ToArray();
        
        private readonly Shader shader;

        private readonly Dictionary<int, Texture> textures = new();

        public Material(Shader shader)
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
    }
}