using System.Collections.Generic;
using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace DamnEngine
{
    public static class Input
    {
        public static MouseState State { get; set; }
        
        public static Vector2 MousePosition => new(State.X, State.Y);

        private static readonly List<Keys> keysDown = new();
        private static readonly List<Keys> keysPress = new();
        private static readonly List<Keys> keysUp = new();
        
        public static void OnKeyDown(Keys key)
        {
            if (!keysPress.Contains(key))
                keysDown.Add(key);
        }

        public static void OnKeyUp(Keys key)
        {
            keysPress.Remove(key);
            keysUp.Add(key);
        }

        public static void Update()
        {
            foreach (var keyDown in keysDown)
                keysPress.Add(keyDown);
            
            keysDown.Clear();
            
            keysUp.Clear();
        }

        public static bool IsKeyDown(Keys key) => keysDown.Contains(key);

        public static bool IsKeyPress(Keys key) => keysDown.Contains(key) || keysPress.Contains(key);

        public static bool IsKeyUp(Keys key) => keysUp.Contains(key);
    }
}