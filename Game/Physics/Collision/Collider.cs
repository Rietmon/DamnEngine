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
        
        public abstract bool IsStaticShapeCreated { get; protected set; }
        
        public abstract Bounds Bounds { get; }
        
        public abstract IConvexShape Shape { get; }
        
        public abstract TypedIndex ShapeIndex { get; }
        
        public abstract Vector3 ShapePosition { get; }

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

        internal virtual void TryCreateStaticShape()
        {
            if (IsStaticShapeCreated)
                return;

            var boxShape = ShapeIndex;
            var bepuPosition = ShapePosition.FromToBepuPosition().ToNumericsVector3();

            var collidableDescription = new CollidableDescription(boxShape, 0.1f);

            var staticDescription = new StaticDescription(bepuPosition, collidableDescription);

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