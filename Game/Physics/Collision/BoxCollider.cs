using OpenTK;

namespace DamnEngine
{
    public class BoxCollider : Collider
    {
        public Vector3 Center { get; set; } = Vector3.Zero;
        
        public Vector3 Size { get; set; } = Vector3.One;

        public Bounds Bounds => new(Center + Transform.Position, Size * Transform.Scale);

        public override bool IsIntersect(Vector3 position, Vector3 direction, float distance)
        {
            var bounds = Bounds;
            var boundsMin = bounds.Min;
            var boundsMax = bounds.Max;
            var modelMatrix = Transform.TransformMatrix;

            return false;
        }
    }
}