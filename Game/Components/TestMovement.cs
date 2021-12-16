using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Rietmon.Extensions;

namespace DamnEngine
{
    public class TestMovement : Component
    {
        protected internal override void OnUpdate()
        {
            if (Input.IsKeyPress(KeyCode.X))
            {
                Transform.Position += new Vector3(10, 0, 0) * Time.DeltaTime;
            }

            if (Input.IsKeyPress(KeyCode.Y))
            {
                Transform.Position += new Vector3(0, 10, 0) * Time.DeltaTime;
            }

            if (Input.IsKeyPress(KeyCode.Z))
            {
                Transform.Position += new Vector3(0, 0, 10) * Time.DeltaTime;
            }

            if (Input.IsKeyDown(KeyCode.G))
            {
                var x = RandomUtilities.Range(0, 360);
                var y = RandomUtilities.Range(0, 360);
                var z = RandomUtilities.Range(0, 360);
                Transform.LocalRotation = new Vector3(x, y, z);

                var x1 = RandomUtilities.Range(-2, 2);
                var y1 = RandomUtilities.Range(0, 2);
                var z1 = RandomUtilities.Range(-2, 2);
                Transform.LocalRotation += new Vector3(x1, y1, z1);
            }
        }
    }
}