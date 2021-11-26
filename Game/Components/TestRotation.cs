using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Rietmon.Extensions;

namespace DamnEngine
{
    public class TestRotation : Component
    {
        protected internal override void OnUpdate()
        {
            if (Input.IsKeyPress(KeyCode.X))
            {
                Transform.Rotation += new Vector3(500, 0, 0) * Time.DeltaTime;
            }
            if (Input.IsKeyPress(KeyCode.Y))
            {
                Transform.Rotation += new Vector3(0, 500, 0) * Time.DeltaTime;
            }
            if (Input.IsKeyPress(KeyCode.Z))
            {
                Transform.Rotation += new Vector3(0, 0, 500) * Time.DeltaTime;
            }

            if (Input.IsKeyDown(KeyCode.G))
            {
                var x = RandomUtilities.Range(0, 360);
                var y = RandomUtilities.Range(0, 360);
                var z = RandomUtilities.Range(0, 360);
                Transform.Rotation = new Vector3(x, y, z);
                
                var x1 = RandomUtilities.Range(-2, 2);
                var y1 = RandomUtilities.Range(0, 2);
                var z1 = RandomUtilities.Range(-2, 2);
                Transform.Position += new Vector3(x1, y1, z1);
            }
        }
    }
}