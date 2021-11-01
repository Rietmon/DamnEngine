using OpenTK;

namespace DamnEngine
{
    public static class Vector3Extensions
    {
        public static Vector3 Abs(this Vector3 vector) =>
            new(Mathf.Abs(vector.X), Mathf.Abs(vector.Y), Mathf.Abs(vector.Z));
    }
}