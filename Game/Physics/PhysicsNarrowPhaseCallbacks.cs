using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using BepuPhysics.Constraints;

namespace DamnEngine
{
    public struct PhysicsNarrowPhaseCallbacks : INarrowPhaseCallbacks
    {
        public SpringSettings ContactSpringiness;

        public void Initialize(Simulation simulation)
        {
            if (ContactSpringiness.AngularFrequency == 0 && ContactSpringiness.TwiceDampingRatio == 0)
                ContactSpringiness = new SpringSettings(30, 0);
        }

        public bool AllowContactGeneration(int workerIndex, CollidableReference a, CollidableReference b) =>
            a.Mobility == CollidableMobility.Dynamic || b.Mobility == CollidableMobility.Dynamic;

        public bool ConfigureContactManifold<TManifold>(int workerIndex, CollidablePair pair, ref TManifold manifold,
            out PairMaterialProperties pairMaterial) where TManifold : struct, IContactManifold<TManifold>
        {
            pairMaterial = new PairMaterialProperties
            {
                FrictionCoefficient = 1f,
                MaximumRecoveryVelocity = 2f,
                SpringSettings = ContactSpringiness
            };
            return true;
        }

        public bool AllowContactGeneration(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB) =>
            true;

        public bool ConfigureContactManifold(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB,
            ref ConvexContactManifold manifold) => true;

        public void Dispose() { }
    }
}