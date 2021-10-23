using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using DamnEngine.Utilities;

namespace DamnEngine
{
    public static class ResourcesLoader
    {
        public static readonly Dictionary<string, ResourceContainer> loadedResources = new();
        private static readonly Dictionary<string, ResourceContainer<Bitmap>> loadedBitmaps = new();
        private static readonly Dictionary<string, ResourceContainer<Mesh>> loadedMeshes = new();

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

        public static void FreeBitMap(string name) => FreeLoadedResource($"Textures/{name}");

        public static Mesh UseMesh(string name)
        {
            var meshKey = $"Meshes/{name}";
            if (TryGetLoadedResource<Mesh>(meshKey, out var mesh))
                return mesh;
            
            var meshPath = $"GameData/Meshes/{name}";
            switch (Path.GetExtension(name))
            {
                case ".obj":
                    mesh = WavefrontObjParser.Parse(meshPath);
                    break;
                default:
                    Debug.LogError($"[{nameof(ResourcesLoader)}] ({nameof(UseMesh)}) Unsupported mesh format! Format: {Path.GetExtension(name)}");
                    return null;
            }
            mesh.Name = name;
            
            RegisterLoadedResource(meshKey, mesh);
            
            return mesh;
        }

        public static void FreeMesh(string name) => FreeLoadedResource($"Meshes/{name}");

        public static string GetVertexShaderCode(string name) => File.ReadAllText($"GameData/Shaders/{name}.vert");

        public static string GetFragmentShaderCode(string name) => File.ReadAllText($"GameData/Shaders/{name}.frag");

        private static bool TryGetLoadedResource<T>(string name, out T result)
        {
            if (loadedMeshes.TryGetValue(name, out var container))
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
            if (loadedMeshes.TryGetValue(key, out var container))
            {
                container.referencesCount--;
                if (container.referencesCount == 0)
                {
                    loadedMeshes.Remove(key);
                    Debug.Log($"[{nameof(ResourcesLoader)}] ({nameof(FreeBitMap)}) Resource {key} unloaded!");
                }
            }
        }
    }
}