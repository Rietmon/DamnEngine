using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace DamnEngine
{
    public static class ResourcesLoader
    {
        private static readonly Dictionary<string, ResourceContainer<Bitmap>> loadedBitmaps = new();

        public static Bitmap UseBitmap(string name)
        {
            if (loadedBitmaps.TryGetValue(name, out var container))
            {
                container.referencesCount++;
                return container.resource;
            }

            var bitmapPath = $"GameData/Textures/{name}";
            var bitmap = new Bitmap(bitmapPath);
            
            container = new ResourceContainer<Bitmap>(bitmap);
            container.referencesCount++;
            loadedBitmaps.Add(name, container);
            
            return bitmap;
        }

        public static void FreeBitMap(string name)
        {
            if (loadedBitmaps.TryGetValue(name, out var container))
            {
                container.referencesCount--;
                if (container.referencesCount == 0)
                    loadedBitmaps.Remove(name);
            }
        }

        public static string GetVertexShaderCode(string name)
        {
            var vertexShaderPath = $"GameData/Shaders/{name}.vert";
            return File.ReadAllText(vertexShaderPath);
        }

        public static string GetFragmentShaderCode(string name)
        {
            var fragmentShaderPath = $"GameData/Shaders/{name}.frag";
            return File.ReadAllText(fragmentShaderPath);
        }
    }
}