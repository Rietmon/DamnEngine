using BepuPhysics;
using BepuPhysics.Collidables;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public class SphereCollider : Collider
    {
        public Vector3 Center { get; set; } = Vector3.Zero;
        public float Radius { get; set; } = 1;
        public override bool IsStaticShapeCreated { get; protected set; }

        public override Bounds Bounds => new(Center, new Vector3(Radius * Transform.Scale.Max()));

        public override IConvexShape Shape => new Sphere(Radius);
        public override TypedIndex ShapeIndex => GetShape((Sphere)Shape);
        
        public override Vector3 ShapePosition => Transform.Position + Center;
    }
}