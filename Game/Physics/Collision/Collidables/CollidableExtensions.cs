using BepuPhysics.Collidables;

namespace DamnEngine
{
    public static class CollidableExtensions
    {
        public static Box ToBox(this Bounds bounds) => new(bounds.Size.X, bounds.Size.Y, bounds.Size.Z);
    }
}