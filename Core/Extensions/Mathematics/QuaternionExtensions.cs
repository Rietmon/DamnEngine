using OpenTK.Mathematics;

namespace DamnEngine
{
    public static class QuaternionExtensions
    {
        public static System.Numerics.Quaternion ToNumericsQuaternion(this Quaternion quaternion) =>
            new(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        public static Quaternion ToQuaternion(this System.Numerics.Quaternion quaternion) =>
            new(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
    }
}