using DamnEngine;
using OpenTK.Mathematics;

namespace Puzzle
{
    public class PuzzleCamera : Component
    {
        protected override void OnCreate()
        {
            Transform.Position = new Vector3(-5, 5, -5);
            Transform.LocalRotation = new Vector3(-35, 45, 0);
            
            AddComponent<Camera>();
            //AddComponent<FreeFlyCamera>();
        }
    }
}