using System.Collections.Generic;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities.Memory;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public static class Physics
    {
        public static Simulation Simulation { get; private set; }
        
        private static readonly Dictionary<CollidableReference, Collider> collidersHandles = new();

        private static BufferPool bufferPool;

        internal static void Initialize()
        {
            bufferPool = new BufferPool();

            Simulation = Simulation.Create(bufferPool, new PhysicsNarrowPhaseCallbacks(),
                new PhysicsPoseIntegratorCallbacks(), new PositionFirstTimestepper());
        }

        internal static void Update(float deltaTime)
        {
            Simulation.Timestep(deltaTime);
        }

        public static RayCastHit RayCast(Vector3 position, Vector3 direction, float maxDistance = float.MaxValue)
        {
            position = position.FromToBepuPosition();
            direction = direction.FromToBepuPosition();
            
            var hitHandler = new HitHandler(maxDistance);
            Simulation.RayCast(position.ToNumericsVector3(), direction.ToNumericsVector3(), maxDistance, ref hitHandler);

            return hitHandler.CastHit;
        }

        public static void RegisterCollider(CollidableReference reference, Collider collider) => collidersHandles.Add(reference, collider);
        public static Collider FindCollider(CollidableReference reference) => collidersHandles[reference];
        public static Collider TryFindCollider(CollidableReference reference) => collidersHandles.TryGetValue(reference, out var collider) ? collider : null;
        public static void UnregisterCollider(CollidableReference reference) => collidersHandles.Remove(reference);

        #region Mathematics Extensions

        internal static Vector3 FromToBepuPosition(this Vector3 vector) => new(vector.X, vector.Z, vector.Y);
        internal static Vector3 FromToBepuRotation(this Vector3 vector) => new(-vector.X, -vector.Y, -vector.Z);
        internal static Quaternion FromToBepuQuaternion(this Quaternion quaternion) => new(quaternion.X, quaternion.Z, quaternion.Y, quaternion.W);
        internal static Quaternion FromToBepuQuaternionNegative(this Quaternion quaternion) => new(-quaternion.X, -quaternion.Z, -quaternion.Y, quaternion.W);
        internal static Vector3 QuaternionToEulerAngles(this Quaternion quaternion) => quaternion.ToEulerAngles().FromToBepuRotation();

        #endregion
    }
}