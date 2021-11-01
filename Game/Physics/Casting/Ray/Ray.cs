using OpenTK;

namespace DamnEngine
{
    public readonly struct Ray
    {
        public Vector3 Position { get; }
        
        public Vector3 Direction { get; }
        
        public float Distance { get; }

        public Ray(Vector3 position = new(), Vector3 direction = new(), float distance = 100)
        {
            Position = position;
            Direction = direction;
            Distance = distance;
        }
    }
}