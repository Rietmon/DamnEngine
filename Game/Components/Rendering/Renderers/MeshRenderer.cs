using DamnEngine.Render;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public class MeshRenderer : Renderer
    {
        public Mesh Mesh
        {
            get => mesh;
            set
            {
                if (mesh)
                    mesh.OnMeshChanged -= OnMeshChanged;
                
                mesh = value;
                mesh.OnMeshChanged += OnMeshChanged;
                meshDiagonalBounds = mesh.CenteredBounds.Diagonal;
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

        private float meshDiagonalBounds;

        public bool IsInFrustum() => Camera.Main.CubeInFrustum(Transform.ModelMatrix.GetTRSPosition() + mesh.CenteredBounds.Center,
            meshDiagonalBounds * Transform.Scale);
        
        public bool IsInFrustum(Matrix4 modelMatrix) => 
            Camera.Main.CubeInFrustum(modelMatrix.GetTRSPosition() + mesh.CenteredBounds.Center,
                meshDiagonalBounds * Transform.Scale);

        private void CreateRenderTask()
        {
            if (Mesh && Material)
                CreateRenderTask(Mesh.RenderTaskData, Mesh.Indices, Material, Mesh.RuntimeId);
            else
                DeleteRenderTask();
        }

        protected override void OnRendering()
        {
            var renderingLayers = Rendering.RenderParameters.renderingLayers;
            if ((renderingLayers & RenderingLayer) == 0 && (renderingLayers & RenderingLayers.All) != 0)
                return;
            
            var modelMatrix = Transform.ModelMatrix;
            if (!IsInFrustum(modelMatrix))
                return;
            
            material.SetMatrix4("transform", modelMatrix);
            material.SetMatrix4("view", Rendering.ViewMatrix);
            material.SetMatrix4("projection", Rendering.ProjectionMatrix);
            RenderTask.Draw();
        }

        private void OnMeshChanged()
        {
            if (Mesh.IsValid)
            {
                CreateRenderTask(Mesh.RenderTaskData, Mesh.Indices, Material, Mesh.RuntimeId);
                meshDiagonalBounds = mesh.CenteredBounds.Diagonal;
            }
        }
    }
}