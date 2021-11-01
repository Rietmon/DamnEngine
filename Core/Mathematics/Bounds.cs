using OpenTK;

namespace DamnEngine
{
    public struct Bounds
    {
        public Vector3 Min => Center - Size / 2;

        public Vector3 Max => Center + Size / 2;
        
        public Vector3 Center { get; set; }
        
        public Vector3 Size { get; set; }

        public Bounds(Vector3 center, Vector3 size)
        {
            Center = center;
            Size = size;
        }
    }
}