using System.Collections.Generic;
using System.Drawing;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Cursor = System.Windows.Forms.Cursor;

namespace DamnEngine
{
    public static class Input
    {
        public static bool GrabMouse { get; set; }
        
        public static MouseState MouseState { get; set; }

        public static Vector2 MousePosition
        {
            get => mousePosition;
            set
            {
                var mousePoint = new Point((int)value.X, (int)value.Y);
                Cursor.Position = mousePoint;
            }
        }
        public static Vector2 MousePreviousPosition { get; private set; }
        public static Vector2 MouseDeltaPosition { get; private set; }

        private static readonly List<KeyCode> keysDown = new();
        private static readonly List<KeyCode> keysPress = new();
        private static readonly List<KeyCode> keysUp = new();

        private static Vector2 mousePosition;

        public static void Initialize(RenderWindow renderWindow)
        {
            Cursor.Position = new Point((int)renderWindow.Bounds.Center.X, (int)renderWindow.Bounds.Center.Y);
        }
        
        public static void OnKeyDown(Keys key)
        {
            var keyCode = (KeyCode)key;
            if (!keysPress.Contains(keyCode))
                keysDown.Add(keyCode);
        }

        public static void OnKeyUp(Keys key)
        {
            var keyCode = (KeyCode)key;
            keysPress.Remove(keyCode);
            keysUp.Add(keyCode);
        }

        public static void Update(RenderWindow renderWindow)
        {
            foreach (var keyDown in keysDown)
                keysPress.Add(keyDown);
            
            keysDown.Clear();
            
            keysUp.Clear();

            MouseState = renderWindow.MouseState;

            var mousePoint = Cursor.Position;
            MousePreviousPosition = mousePosition;
            mousePosition = new Vector2(mousePoint.X, mousePoint.Y);
            if (GrabMouse && renderWindow.IsFocused)
            {
                var windowCenter = renderWindow.Bounds.Center;
                MouseDeltaPosition = MousePosition - windowCenter;
                Cursor.Position = new Point((int)windowCenter.X, (int)windowCenter.Y);
            }
            else
            {
                MouseDeltaPosition = mousePosition - MousePreviousPosition;
            }
        }

        public static bool IsKeyDown(KeyCode key) => keysDown.Contains(key);

        public static bool IsKeyPress(KeyCode key) => keysDown.Contains(key) || keysPress.Contains(key);

        public static bool IsKeyUp(KeyCode key) => keysUp.Contains(key);
    }
}