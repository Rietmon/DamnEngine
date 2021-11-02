using BepuPhysics;
using BepuPhysics.Collidables;
using OpenTK;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public class BoxCollider : Collider
    {
        public Vector3 Center { get; set; } = Vector3.Zero;
        
        public Vector3 Size { get; set; } = Vector3.One;

        public override Bounds Bounds => new(Center, Size);

        private StaticHandle staticHandle;

        private BodyReference bodyReference;

        protected internal override void OnStart()
        {
            var box = Bounds.ToBox();
            var boxShape = GetShape(box);
            box.ComputeInertia(1, out var boxInertia);
            var position = Transform.Position.ToNumericsVector3();

            //var staticDescription = new StaticDescription(position, new CollidableDescription(boxShape, 0.1f));
            var bodyDescription = BodyDescription.CreateDynamic(position, boxInertia,
                new CollidableDescription(boxShape, 0.1f), new BodyActivityDescription(0.01f));
            
            //staticHandle = Simulation.Statics.Add(staticDescription);
            var bodyHandle = Simulation.Bodies.Add(bodyDescription);
            bodyReference = Simulation.Bodies.GetBodyReference(bodyHandle);
        }

        protected internal override void OnPostUpdate()
        {
            Transform.Position = bodyReference.Pose.Position.ToVector3();
        }
    }
}