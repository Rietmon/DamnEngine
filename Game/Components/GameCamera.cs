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
            var speed = 10;
            if (Input.IsKeyPress(Keys.LeftControl))
                speed = 5;
            else if (Input.IsKeyPress(Keys.LeftShift))
                speed = 20;

            if (Input.IsKeyPress(Keys.W))
                Transform.Position += Transform.Forward * Time.DeltaTime * speed;
            else if (Input.IsKeyPress(Keys.S))
                Transform.Position -= Transform.Forward * Time.DeltaTime * speed;
            
            if (Input.IsKeyPress(Keys.D))
                Transform.Position += Transform.Right * Time.DeltaTime * speed;
            else if (Input.IsKeyPress(Keys.A))
                Transform.Position -= Transform.Right * Time.DeltaTime * speed;
            
            if (Input.IsKeyPress(Keys.E))
                Transform.Position += Transform.Up * Time.DeltaTime * speed;
            else if (Input.IsKeyPress(Keys.Q))
                Transform.Position -= Transform.Up * Time.DeltaTime * speed;
            
            var targetRotation = new Vector3(-Input.MouseDeltaPosition.Y, -Input.MouseDeltaPosition.X, 0) / 10 + Transform.Rotation;
            targetRotation.X = Mathf.Clamp(targetRotation.X, -90, 90);
            Transform.Rotation = targetRotation;
            
            Physics.RayCast(Transform.Position, Transform.Forward, 10);
        }
    }
}