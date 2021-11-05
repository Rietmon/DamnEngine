using BepuPhysics.Collidables;

namespace DamnEngine
{
    public static class BepuPhysicsExtensions
    {
        public static Box ToBox(this Bounds bounds)
        {
            var bepuVector = bounds.Size.FromToBepuVector3();
            return new Box(bepuVector.X, bepuVector.Y, bepuVector.Z);
        }
    }
}