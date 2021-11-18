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

        private static readonly List<Keys> keysDown = new();
        private static readonly List<Keys> keysPress = new();
        private static readonly List<Keys> keysUp = new();

        private static Vector2 mousePosition;

        public static void Initialize(RenderWindow renderWindow)
        {
            Cursor.Position = new Point((int)renderWindow.Bounds.Center.X, (int)renderWindow.Bounds.Center.Y);
        }
        
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

        public static bool IsKeyDown(Keys key) => keysDown.Contains(key);

        public static bool IsKeyPress(Keys key) => keysDown.Contains(key) || keysPress.Contains(key);

        public static bool IsKeyUp(Keys key) => keysUp.Contains(key);
    }
}