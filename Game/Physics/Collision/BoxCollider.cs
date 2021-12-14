using BepuPhysics;
using BepuPhysics.Collidables;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public class BoxCollider : Collider
    {
        public Vector3 Center { get; set; } = Vector3.Zero;
        
        public Vector3 Size { get; set; } = Vector3.One;
        
        public override bool IsStaticShapeCreated { get; protected set; }

        public override Bounds Bounds => new(Center, Size * Transform.Scale);

        public override IConvexShape Shape
        {
            get
            {
                var (x, y, z) = Transform.Scale.FromToBepuPosition() * Size.FromToBepuPosition();
                var box = new Box(x, y, z);
                return box;
            }
        }

        public override TypedIndex ShapeIndex => GetShape((Box)Shape);

        public override Vector3 ShapePosition => Transform.Position + Center;
    }
}