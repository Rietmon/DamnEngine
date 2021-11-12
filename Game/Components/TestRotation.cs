using DamnEngine.Render;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace DamnEngine
{
    public class TestRotation : Component
    {
        protected internal override void OnCreate()
        {
            Rendering.OnRendering += () =>
                Graphics.DrawRay(Transform.Position, Transform.EulerAngles.Normalized(), 5);
        }

        protected internal override void OnUpdate()
        {
            if (Input.IsKeyPress(Keys.X))
            {
                Transform.Rotation *= Quaternion.FromAxisAngle(Vector3.UnitX, 0.025f);
            }
            if (Input.IsKeyPress(Keys.Y))
            {
                Transform.Rotation *= Quaternion.FromAxisAngle(Vector3.UnitY, 0.025f);
            }
            if (Input.IsKeyPress(Keys.Z))
            {
                Transform.Rotation *= Quaternion.FromAxisAngle(Vector3.UnitZ, 0.025f);
            }

            if (Input.IsKeyPress(Keys.G))
                Transform.EulerAngles = new Vector3(45, -45, 0);
        }
    }
}