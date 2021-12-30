using System.Drawing;
using DamnEngine;
using DamnEngine.Render;
using OpenTK.Mathematics;
using Rietmon.Extensions;
using Graphics = DamnEngine.Render.Graphics;

namespace Puzzle
{
    public class PuzzleCamera : Component
    {
        public static Camera sec;
        protected override void OnCreate()
        {
            Transform.Position = new Vector3(0, 10, -2);
            //Transform.Position = new Vector3(-5, 5, -5);
            //Transform.LocalRotation = new Vector3(-35, 45, 0);
            
            AddComponent<Camera>();
            sec = AddComponent<Camera>();
            sec.RenderTexture = RenderTexture.Create(512, 512, 1);
            AddComponent<FreeFlyCamera>();
        }

        protected override void OnUpdate()
        {
            if (Input.IsKeyDown(KeyCode.Minus))
            {
                sec.Far -= 100;
                Debug.Log(sec.Far);
            }
        }
    }
}