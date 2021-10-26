using DamnEngine.Render;

namespace DamnEngine
{
    public class MeshRenderer : Renderer
    {
        public Mesh Mesh
        {
            get => mesh;
            set
            {
                mesh = value;
                CreateRenderTask();
            }
        }

        public Material Material
        {
            get => material;
            set
            {
                material = value;
                CreateRenderTask();
            }
        }

        private Mesh mesh;
        private Material material;

        private void CreateRenderTask()
        {
            if (Mesh && Material)
                CreateRenderTask(Mesh.Vertices, Mesh.Uv, Mesh.Normals, Mesh.Indices, Material);
            else
                DeleteRenderTask();
        }

        protected override void OnRendering()
        {
            material.SetMatrix4("transform", Transform.TransformMatrix);
            material.SetMatrix4("view", Rendering.ViewMatrix);
            material.SetMatrix4("projection", Rendering.ProjectionMatrix);
            RenderTask.Draw();
        }
    }
}