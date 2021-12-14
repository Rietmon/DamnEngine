using BepuPhysics;
using BepuPhysics.Collidables;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public class SphereCollider : Collider
    {
        public Vector3 Center { get; set; } = Vector3.Zero;
        public float Radius { get; set; } = 1;
        public IConvexShape ConvexShape { get; private set; }

        public override Bounds Bounds => new(Center, new Vector3(Radius * Transform.Scale.Max()));

        public override IShape Shape
        {
            get
            {
                ConvexShape = new Sphere(Radius);
                return new Sphere(Radius);
            }
        }
        
        public override TypedIndex ShapeIndex => GetShape((Sphere)Shape);
        
        public override Vector3 ShapePosition => Transform.Position + Center;

        internal override void ComputeInertia(float mass, out BodyInertia inertia) =>
            ConvexShape.ComputeInertia(mass, out inertia);
    }
}