using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace DamnEngine
{
    public class GameCamera : Component
    {
        private Vector2 prevMousePosition;
        
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

            if (prevMousePosition != default)
            {
                var mousePosition = Input.MousePosition - prevMousePosition;
                var targetRotation = new Vector3(-mousePosition.Y, -mousePosition.X, 0) / 10 + Transform.Rotation;
                targetRotation.X = Mathf.Clamp(targetRotation.X, -90, 90);
                Transform.Rotation = targetRotation;
            }
            prevMousePosition = Input.MousePosition;

            
        }
    }
}