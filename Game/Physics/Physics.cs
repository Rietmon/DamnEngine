﻿using BepuPhysics;
using BepuUtilities.Memory;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public static class Physics
    {
        public static Simulation Simulation { get; private set; }

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

        internal static Vector3 FromToBepuPosition(this Vector3 vector) => new(vector.X, vector.Z, vector.Y);
        internal static Quaternion FromToBepuQuaternion(this Quaternion quaternion) => new(-quaternion.X, -quaternion.Z, -quaternion.Y, quaternion.W);
    }
}