using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Rietmon.Extensions;

namespace DamnEngine
{
    public class GameCamera : Component
    {
        protected internal override void OnCreate()
        {
            Transform.Position = new Vector3(0, 0, -10);
        }

        protected internal override void OnUpdate()
        {
            if (Input.IsKeyPress(Keys.W))
                Transform.Position += Transform.Forward * Time.DeltaTime * 10;
            else if (Input.IsKeyPress(Keys.S))
                Transform.Position -= Transform.Forward * Time.DeltaTime * 10;
            
            if (Input.IsKeyPress(Keys.D))
                Transform.Position += Transform.Right * Time.DeltaTime * 10;
            else if (Input.IsKeyPress(Keys.A))
                Transform.Position -= Transform.Right * Time.DeltaTime * 10;
            
            if (Input.IsKeyPress(Keys.E))
                Transform.Position += Transform.Up * Time.DeltaTime * 10;
            else if (Input.IsKeyPress(Keys.Q))
                Transform.Position -= Transform.Up * Time.DeltaTime * 10;
            
            // var rotationX = Quaternion.FromAxisAngle(Vector3.UnitX, Input.MouseDeltaPosition.Y / 10 * Time.DeltaTime);
            // var rotationY = Quaternion.FromAxisAngle(Vector3.UnitY, -Input.MouseDeltaPosition.X / 10 * Time.DeltaTime);
            // var rotation = rotationX * rotationY;
            // Transform.Rotation *= rotation;

            var rotation = new Vector3(-Input.MouseDeltaPosition.Y / 10, -Input.MouseDeltaPosition.X / 10, 0);
            Transform.Rotation += rotation;
            
            

            //Application.Window.Title = Transform.EulerAngles.ToString();
        }
    }
}