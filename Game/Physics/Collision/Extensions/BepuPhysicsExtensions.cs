using BepuPhysics.Collidables;

namespace DamnEngine
{
    public static class BepuPhysicsExtensions
    {
        public static Box ToBox(this Bounds bounds)
        {
            var bepuVector = bounds.Size.FromToBepuPosition();
            return new Box(bepuVector.X, bepuVector.Y, bepuVector.Z);
        }
    }
}