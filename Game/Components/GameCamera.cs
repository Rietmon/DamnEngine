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
                Transform.Position -= Transform.Right * Time.DeltaTime * 10;
            else if (Input.IsKeyPress(Keys.A))
                Transform.Position += Transform.Right * Time.DeltaTime * 10;
            
            if (Input.IsKeyPress(Keys.E))
                Transform.Position += Transform.Up * Time.DeltaTime * 10;
            else if (Input.IsKeyPress(Keys.Q))
                Transform.Position -= Transform.Up * Time.DeltaTime * 10;

            var targetRotationX = new Vector3(0, -Input.MouseDeltaPosition.X, 0) / 10 / Mathf.Rad2Deg;
            var targetQuaternionRotation = QuaternionExtensions.FromEuler(targetRotationX);
            Application.Window.Title = targetQuaternionRotation.ToString();
            Transform.Rotation *= targetQuaternionRotation;

            if (Input.IsKeyDown(Keys.B))
            {
                ScenesManager.CurrentScene.ForEachGameObject((obj) =>
                {
                    if (!obj.Name.Contains("PhysicsCube"))
                        return;
                    
                    var rigidBody = obj.GetComponent<RigidBody>();
                    var x = RandomUtilities.Range(-0.5f, 0.5f);
                    var y = RandomUtilities.Range(1, 10);
                    var z = RandomUtilities.Range(-0.5f, 0.5f);
                    var x2 = RandomUtilities.Range(-0.5f, 0.5f);
                    var y2 = RandomUtilities.Range(-1, 1);
                    var z2 = RandomUtilities.Range(-0.5f, 0.5f);
                    Debug.Log(x + " " + y + " " + z);
                    rigidBody.ApplyImpulse(new Vector3(x,y,z), new Vector3(x2, y2, z2));
                });
            }
        }
    }
}