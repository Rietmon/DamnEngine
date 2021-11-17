using BepuPhysics;
using BepuPhysics.Collidables;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public class BoxCollider : Collider
    {
        public bool IsStaticShapeCreated { get; private set; }
        public Vector3 Center { get; set; } = Vector3.Zero;
        
        public Vector3 Size { get; set; } = Vector3.One;
        
        public override bool IsStaticCreated { get; protected set; }

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

        public override TypedIndex ShapeIndex
        {
            get
            {
                var boxShape = GetShape((Box)Shape);
                return boxShape;
            }
        }

        public override Vector3 ShapePosition => Transform.Position + Center;

        protected internal override void OnCreate()
        {
            CreateStaticShape();
        }

        protected internal override void OnEnable()
        {
            CreateStaticShape();
        }

        internal override void CreateStaticShape()
        {
            if (IsStaticCreated || IsStaticShapeCreated)
                return;
            
            IsStaticCreated = true;

            var boxShape = ShapeIndex;
            var bepuPosition = ShapePosition.FromToBepuPosition().ToNumericsVector3();

            var collidableDescription = new CollidableDescription(boxShape, 0.1f);

            var staticDescription = new StaticDescription(bepuPosition, collidableDescription);

            staticHandle = Simulation.Statics.Add(staticDescription);

            Physics.RegisterCollider(staticHandle, this);
            
            IsStaticShapeCreated = true;
        }

        internal override void RemoveStaticShape()
        {
            if (!IsStaticCreated)
                return;
            
            IsStaticCreated = false;
            
            if (IsStaticShapeCreated)
            {
                Simulation.Statics.Remove(staticHandle);
                Physics.UnregisterCollider(staticHandle);
                
                staticHandle = default;
                IsStaticShapeCreated = false;
            }
        }

        protected internal override void OnDisable()
        {
            RemoveStaticShape();
        }

        protected override void OnDestroy()
        {
            RemoveStaticShape();
        }
    }
}