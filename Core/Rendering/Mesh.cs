using OpenTK;

namespace DamnEngine
{
    public class Mesh : DamnObject
    {
        public string OriginalMeshName { get; }
        public Vector3[] Vertices { get; set; }
        public Vector2[] Uv { get; set; }
        public Vector3[] Normals { get; set; }
        public int[] Indices { get; set; }

        public Mesh(string name)
        {
            OriginalMeshName = name;
        }

        protected override void OnDestroy()
        {
            ResourcesLoader.FreeMesh(OriginalMeshName);
        }
        
        public static Mesh CreateFromFile(string meshName)
        {
            return ResourcesLoader.UseMesh(meshName);
        }
    }
}