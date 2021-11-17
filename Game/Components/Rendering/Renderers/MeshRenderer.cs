﻿using DamnEngine.Render;
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

        public bool IsInFrustum => Camera.Main.CubeInFrustum(Transform.Position + mesh.CenteredBounds.Center,
            mesh.CenteredBounds.Size * Transform.Scale);

        private Mesh mesh;
        private Material material;

        private void CreateRenderTask()
        {
            if (Mesh && Material)
                CreateRenderTask(Mesh.RenderTaskData, Mesh.Indices, Material, Mesh.Id);
            else
                DeleteRenderTask();
        }

        protected override void OnRendering()
        {
            if (!IsInFrustum)
                return;
            
            material.SetMatrix4("transform", Transform.ModelMatrix);
            material.SetMatrix4("view", Rendering.ViewMatrix);
            material.SetMatrix4("projection", Rendering.ProjectionMatrix);
            RenderTask.Draw();
        }
    }
}