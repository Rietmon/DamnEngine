using System.Drawing;

namespace DamnEngine.Render
{
    public struct Color32
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        public Color32(byte r = 0, byte g = 0, byte b = 0, byte a = 0)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public static implicit operator Color(Color32 color32) =>
            Color.FromArgb(color32.A, color32.R, color32.G, color32.B);

        public static implicit operator Color32(Color color) =>
            new (color.R, color.G, color.B, color.A);
    }
}