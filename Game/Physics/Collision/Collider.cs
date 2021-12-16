using System.Collections.Generic;
using BepuPhysics;
using BepuPhysics.Collidables;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public abstract class Collider : Component
    {
        private static readonly Dictionary<IShape, TypedIndex> registeredShapes = new();

        protected static Simulation Simulation => Physics.Simulation;

        public virtual bool CanCreateShape => true;

        public abstract Bounds Bounds { get; }
        
        public abstract IShape Shape { get; }
        
        public abstract TypedIndex ShapeIndex { get; }
        
        public abstract Vector3 ShapePosition { get; }
        
        public bool IsStaticShapeCreated { get; protected set; }

        public StaticReference StaticReference => Simulation.Statics.GetStaticReference(staticHandle);

        protected StaticHandle staticHandle;

        protected internal override void OnCreate()
        {
            TryCreateStaticShape();
        }

        protected internal override void OnEnable()
        {
            TryCreateStaticShape();
        }

        public void UpdateColliderTransform()
        {
            if (!IsStaticShapeCreated)
                return;
            
            StaticReference.Pose.Position = Transform.Position.FromToBepuPosition().ToNumericsVector3();
            StaticReference.Pose.Orientation = Transform.RotationInRadians.RotationToBepuQuaternion().ToNumericsQuaternion();
            StaticReference.UpdateBounds();
        }

        internal abstract void ComputeInertia(float mass, out BodyInertia inertia);

        internal virtual void TryCreateStaticShape()
        {
            if (IsStaticShapeCreated || !CanCreateShape)
                return;

            var shape = ShapeIndex;
            var bepuPosition = ShapePosition.FromToBepuPosition().ToNumericsVector3();
            var bepuRotation = Transform.RotationInRadians.RotationToBepuQuaternion().ToNumericsQuaternion();

            var collidableDescription = new CollidableDescription(shape, 0.1f);

            var staticDescription = new StaticDescription(bepuPosition, bepuRotation, collidableDescription);

            staticHandle = Simulation.Statics.Add(staticDescription);

            Physics.RegisterCollider(staticHandle, this);
            
            IsStaticShapeCreated = true;
        }

        internal virtual void TryRemoveStaticShape()
        {
            if (!IsStaticShapeCreated)
                return;
            
            Simulation.Statics.Remove(staticHandle);
            Physics.UnregisterCollider(staticHandle);

            staticHandle = default;

            IsStaticShapeCreated = false;
        }

        protected TypedIndex GetShape<T>(T shape) where T : unmanaged, IShape
        {
            if (registeredShapes.TryGetValue(shape, out var shapeIndex))
                return shapeIndex;

            shapeIndex = Simulation.Shapes.Add(shape);
            registeredShapes.Add(shape, shapeIndex);
            return shapeIndex;
        }

        protected internal override void OnTransformChanged()
        {
            if (!IsStaticShapeCreated)
                return;
            
            UpdateColliderTransform();
        }

        protected internal override void OnDisable()
        {
            TryRemoveStaticShape();
        }

        protected override void OnDestroy()
        {
            TryRemoveStaticShape();
        }
    }
}