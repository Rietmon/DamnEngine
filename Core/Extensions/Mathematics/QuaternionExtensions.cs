using System;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public static class QuaternionExtensions
    {
        public static System.Numerics.Quaternion ToNumericsQuaternion(this Quaternion quaternion) =>
            new System.Numerics.Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        public static Quaternion ToQuaternion(this System.Numerics.Quaternion quaternion) =>
            new(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        // Copied from https://stackoverflow.com/questions/11492299/quaternion-to-euler-angles-algorithm-how-to-convert-to-y-up-and-between-ha/11505219
        public static Vector3 ToEuler(this Quaternion quaternion)
        {
            var pitchYawRoll = new Vector3();

            var sqw = quaternion.W * quaternion.W;
            var sqx = quaternion.X * quaternion.X;
            var sqy = quaternion.Y * quaternion.Y;
            var sqz = quaternion.Z * quaternion.Z;

            // If quaternion is normalised the unit is one, otherwise it is the correction factor
            var unit = sqx + sqy + sqz + sqw;
            var test = quaternion.X * quaternion.Y + quaternion.Z * quaternion.W;

            if (test > 0.4999f * unit)                              // 0.4999f OR 0.5f - EPSILON
            {
                // Singularity at north pole
                pitchYawRoll.Y = 2f * (float)Math.Atan2(quaternion.X, quaternion.W);  // Yaw
                pitchYawRoll.X = Mathf.Pi * 0.5f;                         // Pitch
                pitchYawRoll.Z = 0f;                                // Roll
                return pitchYawRoll;
            }

            if (test < -0.4999f * unit)                        // -0.4999f OR -0.5f + EPSILON
            {
                // Singularity at south pole
                pitchYawRoll.Y = -2f * (float)Math.Atan2(quaternion.X, quaternion.W); // Yaw
                pitchYawRoll.X = -Mathf.Pi * 0.5f;                        // Pitch
                pitchYawRoll.Z = 0f;                                // Roll
                return pitchYawRoll;
            }
            
            pitchYawRoll.Y = (float)Math.Atan2(2f * quaternion.Y * quaternion.W - 2f * quaternion.X * quaternion.Z, sqx - sqy - sqz + sqw);       // Yaw
            pitchYawRoll.X = (float)Math.Asin(2f * test / unit);                                             // Pitch
            pitchYawRoll.Z = (float)Math.Atan2(2f * quaternion.X * quaternion.W - 2f * quaternion.Y * quaternion.Z, -sqx + sqy - sqz + sqw);      // Roll

            return pitchYawRoll;
        }

        public static Quaternion FromEuler(Vector3 euler)
        {
            var quaternion = Quaternion.Identity;
            quaternion *= Quaternion.FromAxisAngle(Vector3.UnitZ, euler.X);
            quaternion *= Quaternion.FromAxisAngle(Vector3.UnitY, euler.Y);
            quaternion *= Quaternion.FromAxisAngle(Vector3.UnitX, euler.Z);
            return quaternion;
        }
    }
}