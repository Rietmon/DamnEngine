using System.Drawing;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Rietmon.Extensions;

namespace DamnEngine
{
    public class GameCamera : Component
    {
        private Vector2 prevMousePosition;

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

            var targetRotation = new Vector3(-Input.MouseDeltaPosition.Y, -Input.MouseDeltaPosition.X, 0) / 10 + Transform.Rotation;
            targetRotation.X = Mathf.Clamp(targetRotation.X, -90, 90);
            Transform.Rotation = targetRotation;

            if (Input.IsKeyDown(Keys.B))
            {
                var obj = ScenesManager.CurrentScene.FindGameObjectByName("PhysicsCube1");
                var rigidBody = obj.GetComponent<RigidBody>();
                rigidBody.ApplyImpulse(new Vector3(0.5f,10,0));
            }
        }
    }
}