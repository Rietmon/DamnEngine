﻿using OpenTK.Mathematics;
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
            
            if (Input.IsKeyPress(KeyCode.D4))
            {
                Transform.Scale += new Vector3(1, 0, 0) * Time.DeltaTime;
            }
            if (Input.IsKeyPress(KeyCode.D5))
            {
                Transform.Scale += new Vector3(0, 1, 0) * Time.DeltaTime;
            }
            if (Input.IsKeyPress(KeyCode.D6))
            {
                Transform.Scale += new Vector3(0, 0, 1) * Time.DeltaTime;
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
    
    public class TestRotation2 : Component
    {
        protected internal override void OnUpdate()
        {
            if (Input.IsKeyPress(KeyCode.D1))
            {
                Transform.Rotation += new Vector3(500, 0, 0) * Time.DeltaTime;
            }
            if (Input.IsKeyPress(KeyCode.D2))
            {
                Transform.Rotation += new Vector3(0, 500, 0) * Time.DeltaTime;
            }
            if (Input.IsKeyPress(KeyCode.D3))
            {
                Transform.Rotation += new Vector3(0, 0, 500) * Time.DeltaTime;
            }
            
            if (Input.IsKeyPress(KeyCode.D7))
            {
                Transform.Scale += new Vector3(1, 0, 0) * Time.DeltaTime;
            }
            if (Input.IsKeyPress(KeyCode.D8))
            {
                Transform.Scale += new Vector3(0, 1, 0) * Time.DeltaTime;
            }
            if (Input.IsKeyPress(KeyCode.D9))
            {
                Transform.Scale += new Vector3(0, 0, 1) * Time.DeltaTime;
            }
        }
    }
}