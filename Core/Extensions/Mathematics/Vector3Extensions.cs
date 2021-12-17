using System;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public static class Vector3Extensions
    {
        public static Vector3 Abs(this Vector3 vector) =>
            new(Mathf.Abs(vector.X), Mathf.Abs(vector.Y), Mathf.Abs(vector.Z));

        public static float Max(this Vector3 vector) => Mathf.Max(vector.X, vector.Y, vector.Z);

        public static Vector3 MoveTowards(Vector3 current, Vector3 target, float maxDelta)
        {
            var num1 = target.X - current.X;
            var num2 = target.Y - current.Y;
            var num3 = target.Z - current.Z;
            var d = (float)(num1 * (double)num1 + num2 * (double)num2 + num3 * (double)num3);
            if (d == 0.0 || maxDelta >= 0.0 && d <= maxDelta * (double)maxDelta)
                return target;
            var num4 = Mathf.Sqrt(d);
            return new Vector3(current.X + num1 / num4 * maxDelta, current.Y + num2 / num4 * maxDelta,
                current.Z + num3 / num4 * maxDelta);
        }

        public static System.Numerics.Vector3 ToNumericsVector3(this Vector3 vector) => new(vector.X, vector.Y, vector.Z);
        public static Vector3 ToVector3(this System.Numerics.Vector3 vector) => new(vector.X, vector.Y, vector.Z);
    }
}