using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using Rietmon.Extensions;
#pragma warning disable 618

namespace DamnEngine
{
    public sealed class Shader : LowLevelDamnObject
    {
        public bool IsCompiled => shaderProgramPointer != -1;

        private readonly Dictionary<string, int> uniformLocations = new();

        private readonly ShaderData shaderData;

        private int shaderProgramPointer = -1;

        private Shader(ShaderData shaderData)
        {
            this.shaderData = shaderData;
        }

        public void Compile()
        {
            var vertexShaderPointer = GL.CreateShader(ShaderType.VertexShader);
            var fragmentShaderPointer = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(vertexShaderPointer, shaderData.VertexShaderCode);
            GL.ShaderSource(fragmentShaderPointer, shaderData.FragmentShaderCode);
            
            GL.CompileShader(vertexShaderPointer);
            var log = GL.GetShaderInfoLog(vertexShaderPointer);
            Debug.CrashAssert(log.IsNullOrEmpty(), $"[{nameof(Shader)}] ({nameof(Compile)}) " +
                                                   $"Can't compile vertex shader with code:\n{shaderData.VertexShaderName}\nLog:\n{log}");

            GL.CompileShader(fragmentShaderPointer);
            log = GL.GetShaderInfoLog(fragmentShaderPointer);
            Debug.CrashAssert(log.IsNullOrEmpty(), $"[{nameof(Shader)}] ({nameof(Compile)}) " +
                                                   $"Can't compile fragment shader with code:\n{shaderData.FragmentShaderName}\nLog:\n{log}");
            
            shaderProgramPointer = GL.CreateProgram();
            GL.AttachShader(shaderProgramPointer, vertexShaderPointer);
            GL.AttachShader(shaderProgramPointer, fragmentShaderPointer);
            GL.LinkProgram(shaderProgramPointer);
            GL.GetProgram(shaderProgramPointer, GetProgramParameterName.LinkStatus, out var code);
            Debug.CrashAssert(code == (int)All.True, $"[{nameof(Shader)}] ({nameof(Compile)}) " +
                                                     $"Can't link shaders in program {shaderProgramPointer}\nError code: {code}\nVS = {shaderData.VertexShaderName}, FS = {shaderData.FragmentShaderName}");
            
            GL.DetachShader(shaderProgramPointer, vertexShaderPointer);
            GL.DetachShader(shaderProgramPointer, fragmentShaderPointer);
            GL.DeleteShader(vertexShaderPointer);
            GL.DeleteShader(fragmentShaderPointer);
            
            GL.GetProgram(shaderProgramPointer, ProgramParameter.ActiveUniforms, out var uniformsCount);
            for (var i = 0; i < uniformsCount; i++)
            {
                var key = GL.GetActiveUniform(shaderProgramPointer, i, out _, out _);
                var location = GL.GetUniformLocation(shaderProgramPointer, key);
                uniformLocations.Add(key, location);
            }
        }

        public void Use() => GL.UseProgram(shaderProgramPointer);

        public int GetAttributeLocation(string attributeName) => GL.GetAttribLocation(shaderProgramPointer, attributeName);

        public int GetUniformLocation(string uniformLocation) => uniformLocations[uniformLocation];

        protected override void OnDestroy()
        {
            GL.DeleteProgram(shaderProgramPointer);
            ResourcesLoader.FreeShaderData(shaderData.VertexShaderName, shaderData.FragmentShaderName);
        }

        public static Shader CreateFromFiles(string shadersName)
        {
            var shaderData = ResourcesLoader.UseShaderData(shadersName, shadersName);
            return new Shader(shaderData);
        }

        public static Shader CreateFromFiles(string vertexShaderName, string fragmentShaderName)
        {
            var shaderData = ResourcesLoader.UseShaderData(vertexShaderName, fragmentShaderName);
            return new Shader(shaderData);
        }
    }
}