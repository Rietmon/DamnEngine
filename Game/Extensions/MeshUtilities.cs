using DamnEngine.Render;

namespace DamnEngine
{
    public static class MeshUtilities
    {
        public static RenderTaskData[] GetRenderTasksDatas(this Mesh mesh, Material[] materials)
        {
            var meshesData = mesh.MeshesData;
            var renderTasksDatas = new RenderTaskData[meshesData.Length];
            for (var i = 0; i < renderTasksDatas.Length; i++)
            {
                renderTasksDatas[i] = new RenderTaskData(meshesData[i], mesh.GetSubMeshIndices(i),
                    materials.GetOrInvalid(i), mesh.RuntimeId);
            }

            return renderTasksDatas;
        }
        
        public static GameObject CreateObjectFromMesh(this Mesh mesh, Material material)
        {
            var cube = GameObject.CreateObject(mesh.OriginalMeshName);
            
            var cubeMeshRenderer = cube.AddComponent<MeshRenderer>();
            cubeMeshRenderer.Material = material;
            cubeMeshRenderer.Mesh = mesh;

            return cube;
        }

        public static GameObject CreateObjectsFromMeshes(this Mesh[] meshes, Material[] materials)
        {
            var parent = meshes[0].CreateObjectFromMesh(materials.GetOrInvalid(0));
            for (var i = 1; i < meshes.Length; i++)
            {
                var subMesh = meshes[i].CreateObjectFromMesh(materials.GetOrInvalid(i));
                subMesh.Transform.Parent = parent;
            }

            return parent;
        }
    }
}