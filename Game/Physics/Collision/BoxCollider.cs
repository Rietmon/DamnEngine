using BepuPhysics;
using BepuPhysics.Collidables;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public class BoxCollider : Collider
    {
        public Vector3 Center { get; set; } = Vector3.Zero;
        
        public Vector3 Size { get; set; } = Vector3.One;

        public override Bounds Bounds => new(Center, Size * Transform.Scale);

        private StaticHandle staticHandle;

        private BodyReference bodyReference;

        public void Dynamic()
        {
            var box = Bounds.ToBox();
            var boxShape = GetShape(new Box(1,2,1));
            box.ComputeInertia(1, out var boxInertia);
            var position = Transform.Position.ToNumericsVector3();

            var bodyDescription = BodyDescription.CreateDynamic(position, boxInertia,
                new CollidableDescription(boxShape, 0.1f), new BodyActivityDescription(0.01f));
            bodyDescription.Velocity = new BodyVelocity(new System.Numerics.Vector3(), new System.Numerics.Vector3());
            
            var bodyHandle = Simulation.Bodies.Add(bodyDescription);
            bodyReference = Simulation.Bodies.GetBodyReference(bodyHandle);
        }

        public void Static()
        {
            var box = Bounds.ToBox();
            var boxShape = GetShape(new Box(10, 10, 0));
            var position = Transform.Position.FromToBepuVector3().ToNumericsVector3();

            var staticDescription = new StaticDescription(new System.Numerics.Vector3(0,0,-10), new CollidableDescription(boxShape, 0.1f));
            
            staticHandle = Simulation.Statics.Add(staticDescription);
        }

        protected internal override void OnPostUpdate()
        {
            if (bodyReference.Exists)
            {
                var a = bodyReference.Pose.Position.ToVector3().FromToBepuVector3();
                Transform.Position = a;
            }

            if (Simulation.Statics.GetStaticReference(staticHandle).Exists)
            {
                var b = Simulation.Statics.GetStaticReference(staticHandle);
                Debug.Log(b.Pose.Position.ToVector3().FromToBepuVector3());
            }
        }
    }
}