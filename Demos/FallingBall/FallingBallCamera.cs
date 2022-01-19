using DamnEngine;
using OpenTK.Mathematics;

namespace FallingBall
{
    public class FallingBallCamera : Component
    {
        private readonly Vector3 targetObjectOffset = new(7.5f, 20, 0);
        
        private Transform targetObjectTransform;
        
        protected override void OnCreate()
        {
            Destroy();
            Transform.LocalRotation = new Vector3(-60, 270, 0);
            targetObjectTransform = ScenesManager.FindGameObjectByName("Player");
            
            AddComponent<Camera>();
        }

        protected override void OnUpdate()
        {
            Transform.Position = targetObjectTransform.Position + targetObjectOffset;
        }
    }
}