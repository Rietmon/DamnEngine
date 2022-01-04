using System.Linq;
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
                UpdateRenderTask();
            }
        }

        public Material Material
        {
            get => materials.FirstOrDefault();
            set => Materials = new[] { value };
        }

        public Material[] Materials
        {
            get => materials;
            set
            {
                materials = value;
                UpdateRenderTask();
            }
        }

        private Mesh mesh;
        private Material[] materials;

        private float meshDiagonalBounds;

        private RenderTask[] renderTasks;

        public bool IsInFrustum() => Camera.Main.CubeInFrustum(Transform.ModelMatrix.GetTRSPosition() + mesh.CenteredBounds.Center,
            meshDiagonalBounds * Transform.Scale);
        
        public bool IsInFrustum(Matrix4 modelMatrix) => 
            Camera.Main.CubeInFrustum(modelMatrix.GetTRSPosition() + mesh.CenteredBounds.Center,
                meshDiagonalBounds * Transform.Scale);

        protected override void OnRendering()
        {
            var renderingLayers = Rendering.RenderParameters.renderingLayers;
            if ((renderingLayers & RenderingLayer) == 0 && (renderingLayers & RenderingLayers.All) != 0)
                return;
            
            var modelMatrix = Transform.ModelMatrix;
            if (!IsInFrustum(modelMatrix))
                return;

            foreach (var material in materials)
            {
                material.SetMatrix4("transform", modelMatrix);
                material.SetMatrix4("view", Rendering.ViewMatrix);
                material.SetMatrix4("projection", Rendering.ProjectionMatrix);
            }
            DrawRenderTasks();
        }

        private void OnMeshChanged()
        {
            if (Mesh.IsValid)
            {
                CreateRenderTasks(Mesh.GetRenderTasksDatas(Materials));
                meshDiagonalBounds = mesh.CenteredBounds.Diagonal;
            }
        }
        
        private void UpdateRenderTask()
        {
            if (Mesh && Material)
                CreateRenderTasks(Mesh.GetRenderTasksDatas(Materials));
            else
                DeleteRenderTasks();
        }
    }
}