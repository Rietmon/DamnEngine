using System;
using System.Collections.Generic;

namespace DamnEngine
{
    public static class Physics
    {
        internal static readonly List<Collider> colliders = new();

        public static void RegisterCollider(Collider collider)
        {
            colliders.Add(collider);
        }

        public static void UnregisterCollider(Collider collider)
        {
            colliders.Remove(collider);
        }
    }
}