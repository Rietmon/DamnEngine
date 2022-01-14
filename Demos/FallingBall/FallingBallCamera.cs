using DamnEngine;
using OpenTK.Mathematics;

namespace FallingBall
{
    public class FallingBallCamera : Component
    {
        private readonly Vector3 targetObjectOffset = new Vector3(0, 2, -2);
        
        private Transform targetObjectTransform;
        
        protected override void OnCreate()
        {
            targetObjectTransform = ScenesManager.FindGameObjectByName("Player");
            
            AddComponent<Camera>();
        }

        protected override void OnUpdate()
        {
            Transform.Position = targetObjectTransform.Position + targetObjectOffset;
        }
    }
}