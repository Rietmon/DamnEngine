using System.Numerics;
using System.Runtime.CompilerServices;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Trees;
using BepuUtilities.Memory;

namespace DamnEngine
{
    public struct HitHandler : IRayHitHandler
    {
        public RayCastHit CastHit => castHit;
        
        private RayCastHit castHit;

        public HitHandler(float maxDistance)
        {
            castHit = new RayCastHit
            {
                Distance = maxDistance
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AllowTest(CollidableReference collidable) => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AllowTest(CollidableReference collidable, int childIndex) => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnRayHit(in RayData ray, ref float maxDistance, float distance, in Vector3 normal, CollidableReference collidable, int childIndex)
        {
            maxDistance = distance;
            if (distance < castHit.Distance)
            {
                castHit.Normal = normal.ToVector3().FromToBepuPosition();
                castHit.Distance = distance;
                castHit.Collidable = collidable;
                castHit.Hit = true;
            }
        }
    }
}