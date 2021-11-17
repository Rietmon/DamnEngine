using BepuPhysics;
using BepuPhysics.Collidables;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public class RigidBody : Component
    {
        private static Simulation Simulation => Physics.Simulation;
        
        public bool IsBodyCreated { get; private set; }

        public BodyReference BodyReference => Simulation.Bodies.GetBodyReference(bodyHandle);

        public float Mass { get; set; } = 1;
        
        private Collider collider;

        private BodyHandle bodyHandle;

        private bool isPhysicsUpdate;
        
        protected internal override void OnStart()
        {
            if (!VerifyCollider())
                return;
            
            CreateDynamicBody();
        }

        protected internal override void OnEnable()
        {
            if (!VerifyCollider())
                return;
            
            CreateDynamicBody();
        }

        public void AwakeBody() => Simulation.Awakener.AwakenBody(bodyHandle);

        public void ApplyImpulse(Vector3 force)
        {
            AwakeBody();
            BodyReference.ApplyImpulse(force.FromToBepuPosition().ToNumericsVector3(), new System.Numerics.Vector3());
        }

        public void ApplyImpulse(Vector3 force, Vector3 offset)
        {
            AwakeBody();
            BodyReference.ApplyImpulse(force.FromToBepuPosition().ToNumericsVector3(), offset.FromToBepuPosition().ToNumericsVector3());
        }

        private void CreateDynamicBody()
        {
            if (IsBodyCreated)
                return;
            
            if (collider.IsStaticCreated)
                collider.RemoveStaticShape();
            
            collider.Shape.ComputeInertia(Mass, out var bodyInertia);
            var shape = collider.ShapeIndex;
            var bepuPosition = collider.ShapePosition.FromToBepuPosition().ToNumericsVector3();

            var collidableDescription = new CollidableDescription(shape, 0.1f);
            var bodyActivityDescription = new BodyActivityDescription(0.01f);
            
            var bodyDescription = BodyDescription.CreateDynamic(bepuPosition, bodyInertia,
                collidableDescription, bodyActivityDescription);

            bodyHandle = Simulation.Bodies.Add(bodyDescription);
            IsBodyCreated = true;

            Physics.RegisterCollider(bodyHandle, collider);
        }

        private void RemoveDynamicBody()
        {
            if (IsBodyCreated)
            {
                Simulation.Bodies.Remove(bodyHandle);
                Physics.UnregisterCollider(bodyHandle);
                
                bodyHandle = default;
                IsBodyCreated = false;
            }
            
            collider.CreateStaticShape();
        }

        protected internal override void OnPostUpdate()
        {
            if (!IsBodyCreated)
                return;

            isPhysicsUpdate = true;
            
            var position = BodyReference.Pose.Position.ToVector3().FromToBepuPosition();
            var rotation = BodyReference.Pose.Orientation.ToQuaternion().FromToBepuQuaternion().QuaternionToEulerAngles();
            
            Transform.SetTransform(position, rotation, Transform.Scale);
            
            isPhysicsUpdate = false;
        }

        protected internal override void OnTransformChanged()
        {
            if (!IsBodyCreated || isPhysicsUpdate)
                return;
            
            BodyReference.Pose.Position = Transform.Position.FromToBepuPosition().ToNumericsVector3();
            BodyReference.Pose.Orientation = Quaternion.FromEulerAngles(Transform.Rotation / Mathf.Rad2Deg).FromToBepuQuaternion().ToNumericsQuaternion();
        }

        protected internal override void OnDisable()
        {
            if (!VerifyCollider())
                return;
            
            RemoveDynamicBody();
        }

        protected override void OnDestroy()
        {
            if (!VerifyCollider())
                return;
            
            RemoveDynamicBody();
        }

        private bool VerifyCollider()
        {
            if (collider) 
                return true;

            if (TryGetComponent(out collider)) 
                return true;
            
            Debug.LogError($"[{nameof(RigidBody)}] ({nameof(OnStart)}) Unable to find collider at rigidbody object!");
            return false;
        }
    }
}