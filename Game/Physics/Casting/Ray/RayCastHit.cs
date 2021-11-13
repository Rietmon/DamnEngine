using BepuPhysics.Collidables;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public struct RayCastHit
    {
        public Vector3 Normal { get; set; }
        public float Distance { get; set; }
        public bool Hit { get; set; }
        public CollidableReference Collidable { get; set; }

        public Collider Collider => Physics.FindCollider(Collidable);

        public GameObject GameObject => Collider.GameObject;
    }
}