using OpenTK.Mathematics;

namespace DamnEngine
{
    public static class Matrix4Extensions
    {
        public static float Get(this Matrix4 matrix, int index) => matrix[index / 4, index % 4];
    }
}