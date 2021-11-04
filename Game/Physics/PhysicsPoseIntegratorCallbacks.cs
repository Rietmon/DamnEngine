using BepuPhysics;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public struct PhysicsPoseIntegratorCallbacks : IPoseIntegratorCallbacks
    {
        public Vector3 Gravity { get; set; }
        
        public float LinearDamping { get; set; }
        
        public float AngularDamping { get; set; }
        
        public AngularIntegrationMode AngularIntegrationMode => AngularIntegrationMode.Nonconserving;

        private Vector3 gravityDelta;
        private float linearDampingDelta;
        private float angularDampingDelta;

        public void Initialize(Simulation simulation)
        {
            Gravity = new Vector3(0, 0, -10);
        }

        public void PrepareForIntegration(float deltaTime)
        {
            gravityDelta = Gravity * deltaTime;
            linearDampingDelta = Mathf.Pow(Mathf.Clamp(1 - LinearDamping, 0, 1), deltaTime);
            angularDampingDelta = Mathf.Pow(Mathf.Clamp(1 - AngularDamping, 0, 1), deltaTime);
        }

        public void IntegrateVelocity(int bodyIndex, in RigidPose pose, in BodyInertia localInertia, int workerIndex,
            ref BodyVelocity velocity)
        {
            if (localInertia.InverseMass > 0)
            {
                velocity.Linear = (velocity.Linear + gravityDelta.ToNumericsVector3()) * linearDampingDelta;
                velocity.Angular *= angularDampingDelta;
            }
        }
    }
}