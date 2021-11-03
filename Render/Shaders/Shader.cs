using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using Rietmon.Extensions;

#pragma warning disable 618

namespace DamnEngine
{
    public sealed class Shader : DamnObject
    {
        private readonly Dictionary<string, int> uniformLocations = new();

        private readonly int shaderProgramPointer;

        public Shader(int shaderProgramPointer)
        {
            this.shaderProgramPointer = shaderProgramPointer;
            
            GL.GetProgram(this.shaderProgramPointer, ProgramParameter.ActiveUniforms, out var uniformsCount);
            for (var i = 0; i < uniformsCount; i++)
            {
                var key = GL.GetActiveUniform(shaderProgramPointer, i, out _, out _);
                var location = GL.GetUniformLocation(shaderProgramPointer, key);
                uniformLocations.Add(key, location);
            }
        }

        internal void Use() => GL.UseProgram(shaderProgramPointer);

        internal int GetAttributeLocation(string attributeName) => GL.GetAttribLocation(shaderProgramPointer, attributeName);

        internal int GetUniformLocation(string uniformLocation) => uniformLocations[uniformLocation];
        
        protected override void OnDestroy() => GL.DeleteProgram(shaderProgramPointer);

        public static Shader CreateFromFile(string shaderName) => CreateFromFiles(shaderName, shaderName);
        
        public static Shader CreateFromFiles(string vertexShaderName, string fragmentShaderName)
        {
            var vertexShaderPointer = GL.CreateShader(ShaderType.VertexShader);
            var fragmentShaderPointer = GL.CreateShader(ShaderType.FragmentShader);

            var vertexShaderCode = ResourcesLoader.GetVertexShaderCode(vertexShaderName);
            var fragmentShaderCode = ResourcesLoader.GetFragmentShaderCode(fragmentShaderName);

            GL.ShaderSource(vertexShaderPointer, vertexShaderCode);
            GL.ShaderSource(fragmentShaderPointer, fragmentShaderCode);
            
            GL.CompileShader(vertexShaderPointer);
            var log = GL.GetShaderInfoLog(vertexShaderPointer);
            Debug.CrashAssert(log.IsNullOrEmpty(), $"[{nameof(Shader)}] ({nameof(CreateFromFiles)}) " +
                                                   $"Can't compile vertex shader with code:\n{vertexShaderName}\nLog:\n{log}");

            GL.CompileShader(fragmentShaderPointer);
            log = GL.GetShaderInfoLog(fragmentShaderPointer);
            Debug.CrashAssert(log.IsNullOrEmpty(), $"[{nameof(Shader)}] ({nameof(CreateFromFiles)}) " +
                                                   $"Can't compile fragment shader with code:\n{fragmentShaderName}\nLog:\n{log}");
            
            var shaderProgramPointer = GL.CreateProgram();
            GL.AttachShader(shaderProgramPointer, vertexShaderPointer);
            GL.AttachShader(shaderProgramPointer, fragmentShaderPointer);
            GL.LinkProgram(shaderProgramPointer);
            GL.GetProgram(shaderProgramPointer, GetProgramParameterName.LinkStatus, out var code);
            Debug.CrashAssert(code == (int)All.True, $"[{nameof(Shader)}] ({nameof(CreateFromFiles)}) " +
                                                     $"Can't link shaders in program {shaderProgramPointer}\nCode:{code}\nVS = {vertexShaderName}, FS = {fragmentShaderName}");
            
            GL.DetachShader(shaderProgramPointer, vertexShaderPointer);
            GL.DetachShader(shaderProgramPointer, fragmentShaderPointer);
            GL.DeleteShader(vertexShaderPointer);
            GL.DeleteShader(fragmentShaderPointer);

            return new Shader(shaderProgramPointer);
        }
    }
}