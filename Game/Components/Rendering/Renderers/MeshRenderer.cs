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
                CreateRenderTask(Mesh.RenderTaskData, Mesh.Indices, Material, Mesh.Id);
            else
                DeleteRenderTask();
        }

        public override void OnRendering()
        {
            if (!new BoxBounding(Transform.Position, 1).IsOnFrustum(Camera.Main.Frustum, Transform))
                return;
            material.SetMatrix4("transform", Transform.ModelMatrix);
            material.SetMatrix4("view", Rendering.ViewMatrix);
            material.SetMatrix4("projection", Rendering.ProjectionMatrix);
            RenderTask.Draw();
        }
    }
}