using OpenTK;

namespace DamnEngine
{
    public static class Vector3Extensions
    {
        public static Vector3 Abs(this Vector3 vector) =>
            new(Mathf.Abs(vector.X), Mathf.Abs(vector.Y), Mathf.Abs(vector.Z));

        public static System.Numerics.Vector3 ToNumericsVector3(this Vector3 vector) => new(vector.X, vector.Y, vector.Z);
        public static Vector3 ToVector3(this System.Numerics.Vector3 vector) => new(vector.X, vector.Y, vector.Z);
    }
}