using System.Collections.Generic;
using System.Drawing;
using System.IO;
using DamnEngine.Utilities;

namespace DamnEngine
{
    public static class ResourcesLoader
    {
        public static readonly Dictionary<string, ResourceContainer> loadedResources = new();
        
        public static Bitmap UseBitmap(string name)
        {
            var bitmapKey = $"Textures/{name}";
            if (TryGetLoadedResource<Bitmap>(bitmapKey, out var bitmap))
                return bitmap;

            var bitmapPath = $"GameData/Textures/{name}";
            bitmap = new Bitmap(bitmapPath);
            
            RegisterLoadedResource(bitmapKey, bitmap);
            
            return bitmap;
        }

        public static void FreeBitMap(string name) => FreeLoadedResource($"GameData/Textures/{name}");

        public static Mesh[] UseMeshes(string name)
        {
            var meshKey = $"Meshes/{name}";
            if (TryGetLoadedResource<Mesh[]>(meshKey, out var mesh))
                return mesh;
            
            var meshPath = $"GameData/Meshes/{name}";
            switch (Path.GetExtension(name))
            {
                case ".obj":
                    mesh = WavefrontObjParser.ParseObj(meshPath);
                    break;
                default:
                    Debug.LogError($"[{nameof(ResourcesLoader)}] ({nameof(UseMeshes)}) Unsupported mesh format! Format: {Path.GetExtension(name)}");
                    return null;
            }
            
            RegisterLoadedResource(meshKey, mesh);
            
            return mesh;
        }

        public static void FreeMesh(string name) => FreeLoadedResource($"GameData/Meshes/{name}");

        public static ShaderData UseShaderData(string vertexShaderName, string fragmentShaderName)
        {
            var shaderDataKey = $"Shaders/{vertexShaderName}_{fragmentShaderName}";
            if (TryGetLoadedResource<ShaderData>(shaderDataKey, out var shaderData))
                return shaderData;
            
            var vertexShaderPath = $"GameData/Shaders/{vertexShaderName}.vert";
            var fragmentShaderPath = $"GameData/Shaders/{vertexShaderName}.frag";
            
            var vertexShaderCode = File.ReadAllText(vertexShaderPath);
            var fragmentShaderCode = File.ReadAllText(fragmentShaderPath);

            shaderData = new ShaderData(vertexShaderName, fragmentShaderName, vertexShaderCode, fragmentShaderCode);
            
            RegisterLoadedResource(shaderDataKey, shaderData);

            return shaderData;
        }

        public static void FreeShaderData(string vertexShaderName, string fragmentShaderName) => FreeLoadedResource($"Shaders/{vertexShaderName}_{fragmentShaderName}");

        private static bool TryGetLoadedResource<T>(string name, out T result)
        {
            if (loadedResources.TryGetValue(name, out var container))
            {
                container.referencesCount++;
                result = container.GetResource<T>();
                return true;
            }

            result = default;
            return false;
        }

        private static void RegisterLoadedResource<T>(string name, T resource)
        {
            var container = new ResourceContainer<T>(resource);
            container.referencesCount++;
            loadedResources.Add(name, container);
            
            Debug.Log($"[{nameof(ResourcesLoader)}] ({nameof(RegisterLoadedResource)}) Resource {name} loaded!");
        }

        private static void FreeLoadedResource(string key)
        {
            if (loadedResources.TryGetValue(key, out var container))
            {
                container.referencesCount--;
                if (container.referencesCount == 0)
                {
                    loadedResources.Remove(key);
                    Debug.Log($"[{nameof(ResourcesLoader)}] ({nameof(FreeBitMap)}) Resource {key} unloaded!");
                }
            }
        }
    }
}