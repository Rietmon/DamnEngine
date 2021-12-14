using BepuPhysics;
using BepuPhysics.Collidables;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public class BoxCollider : Collider
    {
        public Vector3 Center { get; set; } = Vector3.Zero;
        
        public Vector3 Size { get; set; } = Vector3.One;
        
        public IConvexShape ConvexShape { get; private set; }

        public override Bounds Bounds => new(Center, Size * Transform.Scale);

        public override IShape Shape
        {
            get
            {
                var (x, y, z) = Transform.Scale.FromToBepuPosition() * Size.FromToBepuPosition();
                ConvexShape = new Box(x, y, z);
                return ConvexShape;
            }
        }

        public override TypedIndex ShapeIndex => GetShape((Box)Shape);

        public override Vector3 ShapePosition => Transform.Position + Center;

        internal override void ComputeInertia(float mass, out BodyInertia inertia) =>
            ConvexShape.ComputeInertia(mass, out inertia);
    }
}