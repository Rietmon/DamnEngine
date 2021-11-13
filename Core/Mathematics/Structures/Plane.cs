using OpenTK.Mathematics;

namespace DamnEngine
{
    public struct Plane
    {
        public Vector3 Normal { get; set; }
        
        public float Distance { get; set; }

        public Plane(Vector3 normal, float distance)
        {
            Normal = normal;
            Distance = distance;
        }
        
        public Plane(Vector3 point, Vector3 normal)
        {
            Normal = normal.Normalized();
            Distance = Vector3.Dot(normal, point);
        }

        public float GetSignedDistanceToPlane(Vector3 point) => Vector3.Dot(Normal, point) - Distance;
    }
}