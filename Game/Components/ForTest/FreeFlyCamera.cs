using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace DamnEngine
{
    public class FreeFlyCamera : Component
    {
        protected internal override void OnUpdate()
        {
            var speed = 10;
            if (Input.IsKeyPress(KeyCode.LeftControl))
                speed = 5;
            else if (Input.IsKeyPress(KeyCode.LeftShift))
                speed = 20;

            if (Input.IsKeyPress(KeyCode.W))
                Transform.Position += Transform.Forward * Time.DeltaTime * speed;
            else if (Input.IsKeyPress(KeyCode.S))
                Transform.Position -= Transform.Forward * Time.DeltaTime * speed;
            
            if (Input.IsKeyPress(KeyCode.D))
                Transform.Position += Transform.Right * Time.DeltaTime * speed;
            else if (Input.IsKeyPress(KeyCode.A))
                Transform.Position -= Transform.Right * Time.DeltaTime * speed;
            
            if (Input.IsKeyPress(KeyCode.E))
                Transform.Position += Transform.Up * Time.DeltaTime * speed;
            else if (Input.IsKeyPress(KeyCode.Q))
                Transform.Position -= Transform.Up * Time.DeltaTime * speed;
            
            var targetRotation = new Vector3(-Input.MouseDeltaPosition.Y, -Input.MouseDeltaPosition.X, 0) / 10 + Transform.Rotation;
            targetRotation.X = Mathf.Clamp(targetRotation.X, -90, 90);
            Transform.LocalRotation = targetRotation;
        }
    }
}