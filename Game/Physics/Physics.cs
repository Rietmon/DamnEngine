using BepuPhysics;
using BepuUtilities.Memory;

namespace DamnEngine
{
    public static class Physics
    {
        internal static Simulation Simulation { get; private set; }

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
    }
}