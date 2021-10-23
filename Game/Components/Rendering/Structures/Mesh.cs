using OpenTK;

namespace DamnEngine
{
    public class Mesh : DamnObject
    {
        public Vector3[] Vertices { get; set; }

        public Vector2[] Uv { get; set; }

        public int[] Indices { get; set; }
    }
}