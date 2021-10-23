using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;

namespace DamnEngine
{
    public static class Input
    {
        public static MouseState MouseState => Mouse.GetState();
        
        public static Vector2 MousePosition => new(MouseState.X, MouseState.Y);

        private static readonly List<Key> keysDown = new();
        private static readonly List<Key> keysPress = new();
        private static readonly List<Key> keysUp = new();
        
        public static void OnKeyDown(Key key)
        {
            if (!keysPress.Contains(key))
                keysDown.Add(key);
        }

        public static void OnKeyUp(Key key)
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

        public static bool IsKeyDown(Key key) => keysDown.Contains(key);

        public static bool IsKeyPress(Key key) => keysDown.Contains(key) || keysPress.Contains(key);

        public static bool IsKeyUp(Key key) => keysUp.Contains(key);
    }
}