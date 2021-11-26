using OpenTK.Mathematics;

namespace DamnEngine
{
    public static class Matrix4Extensions
    {
        public static float Get(this Matrix4 matrix, int index) => matrix[index / 4, index % 4];

        public static Matrix4 CreateRotation(Vector3 rotation)
        {
            var matrix = Matrix4.Identity;
            matrix *= Matrix4.CreateRotationX(rotation.X);
            matrix *= Matrix4.CreateRotationY(rotation.Y);
            matrix *= Matrix4.CreateRotationZ(rotation.Z);

            return matrix;
        }

        public static Vector3 GetTRSPosition(this Matrix4 matrix) => matrix.Row3.Xyz;

        public static Vector3 GetTRSRotation(this Matrix4 matrix) => matrix.Row2.Xyz;
        
        public static Vector3 GetTRSScale(this Matrix4 matrix) => new(matrix.Row0.Length, matrix.Row1.Length, matrix.Row2.Length);
    }
}