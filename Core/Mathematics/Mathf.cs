using System;
using System.Linq;

namespace DamnEngine
{
    public static class Mathf
    {
        public const float Deg2Rad = 0.01745329f;
        
        public const float Rad2Deg = 57.29578f;
        
        public const float Pi = 3.14f;

        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }

        public static float Abs(float value) => Math.Abs(value);

        public static float Sin(float value) => (float)Math.Sin(value);
        
        public static float Cos(float value) => (float)Math.Cos(value);
        
        public static float Tan(float value) => (float)Math.Tan(value);

        public static int Max(params int[] values) => values.Max();
        
        public static float Max(params float[] values) => values.Max();
        
        public static int Min(params int[] values) => values.Min();
        
        public static float Min(params float[] values) => values.Min();

        public static float Pow(float x, float y) => (float)Math.Pow(x, y);

        public static float Sqrt(float x) => (float)Math.Sqrt(x);

        public static void Swap(ref float first, ref float second) => (first, second) = (second, first);
    }
}