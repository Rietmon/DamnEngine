using DamnEngine.Render;
using OpenTK.Graphics.OpenGL;
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
                Transform.Rotation += new Vector3(0.25f, 0, 0);
            }
            if (Input.IsKeyPress(Keys.Y))
            {
                Transform.Rotation += new Vector3(0, 0.25f, 0);
            }
            if (Input.IsKeyPress(Keys.Z))
            {
                Transform.Rotation += new Vector3(0, 0, 0.25f);
            }
        }
    }
}