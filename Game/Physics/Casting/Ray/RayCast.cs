using OpenTK;

namespace DamnEngine
{
    public class RayCast
    {
        public Collider Target { get; }

        public RayCast(Collider target)
        {
            Target = target;
        }
    }
}