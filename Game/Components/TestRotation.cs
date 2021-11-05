using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace DamnEngine
{
    public class TestRotation : Component
    {
        protected internal override void OnUpdate()
        {
            if (Input.IsKeyPress(Keys.X))
            {
                Transform.EulerRotation += Vector3.UnitX / 100;
            }
            if (Input.IsKeyPress(Keys.Y))
            {
                Transform.EulerRotation += Vector3.UnitY / 100;
            }
            if (Input.IsKeyPress(Keys.Y))
            {
                Transform.EulerRotation += Vector3.UnitZ / 100;
            }
        }
    }
}