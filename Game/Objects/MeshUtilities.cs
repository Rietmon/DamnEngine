using DamnEngine.Render;

namespace DamnEngine
{
    public static class MeshUtilities
    {
        public static GameObject CreateObjectFromMesh(this Mesh mesh, Material material)
        {
            var cube = new GameObject(mesh.OriginalMeshName);
            
            var cubeMeshRenderer = cube.AddComponent<MeshRenderer>();
            cubeMeshRenderer.Material = material;
            cubeMeshRenderer.Mesh = mesh;

            return cube;
        }

        public static GameObject CreateObjectsFromMeshes(this Mesh[] meshes, Material[] materials)
        {
            if (Debug.LogErrorAssert(meshes.Length <= materials.Length,
                    $"[{nameof(MeshUtilities)}] ({nameof(CreateObjectsFromMeshes)}) Meshes count is less than materials count!"))
                return null;

            var parent = meshes[0].CreateObjectFromMesh(materials[0]);
            for (var i = 1; i < meshes.Length; i++)
            {
                var subMesh = meshes[i].CreateObjectFromMesh(materials[i]);
                subMesh.Transform.Parent = parent;
            }

            return parent;
        }
    }
}