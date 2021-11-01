using OpenTK;

namespace DamnEngine
{
    public abstract class Collider : Component
    {
        protected internal override void OnCreate()
        {
            Physics.RegisterCollider(this);
        }

        public abstract bool IsIntersect(Vector3 position, Vector3 direction, float distance);
        
        protected override void OnDestroy()
        {
            Physics.UnregisterCollider(this);
        }
    }
}