using System;

namespace DamnEngine
{
    public struct FrustumSide
    {
        public float A { get; set; }
        public float B { get; set; }
        public float C { get; set; }
        public float D { get; set; }

        public FrustumSide(float a, float b, float c, float d)
        {
            var magnitude = (float)Math.Sqrt(a * a + b * b + c * c);
            A = a / magnitude;
            B = b / magnitude;
            C = c / magnitude;
            D = d / magnitude;
        }

        public float this[int index] =>
            index switch
            {
                0 => A,
                1 => B,
                2 => C,
                3 => D,
                _ => default
            };
    }
}