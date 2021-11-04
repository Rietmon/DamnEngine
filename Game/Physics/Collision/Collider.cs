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
        
        public bool IsStatic { get; protected set; }
        
        public abstract Bounds Bounds { get; }
        
        public abstract IConvexShape Shape { get; }
        
        public abstract TypedIndex ShapeIndex { get; }
        
        public abstract Vector3 ShapePosition { get; }

        public StaticReference StaticReference => Simulation.Statics.GetStaticReference(staticHandle);

        protected StaticHandle staticHandle;

        internal abstract void CreateStaticShape();
        
        internal abstract void RemoveStaticShape();

        protected TypedIndex GetShape<T>(T shape) where T : unmanaged, IShape
        {
            if (registeredShapes.TryGetValue(shape, out var shapeIndex))
                return shapeIndex;

            shapeIndex = Simulation.Shapes.Add(shape);
            registeredShapes.Add(shape, shapeIndex);
            return shapeIndex;
        }
    }
}