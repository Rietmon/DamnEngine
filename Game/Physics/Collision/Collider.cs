using System.Collections.Generic;
using BepuPhysics;
using BepuPhysics.Collidables;

namespace DamnEngine
{
    public abstract class Collider : Component
    {
        private static readonly Dictionary<IShape, TypedIndex> registeredShapes = new();

        protected static Simulation Simulation => Physics.Simulation;
        
        public abstract Bounds Bounds { get; }

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