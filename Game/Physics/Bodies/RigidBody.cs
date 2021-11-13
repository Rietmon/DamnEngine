using System;
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

        protected internal override void OnPostUpdate()
        {
            Transform.Position = BodyReference.Pose.Position.ToVector3().FromToBepuPosition();
            Transform.Rotation = BodyReference.Pose.Orientation.ToQuaternion().FromToBepuQuaternion().QuaternionToEulerAngles();
        }

        private void CreateDynamicBody()
        {
            if (IsBodyCreated)
                return;
            
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