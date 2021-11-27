﻿using DamnEngine.Render;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

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

        public bool IsInFrustum
        {
            get
            {
                var modelMatrix = Transform.ModelMatrix;
                return Camera.Main.CubeInFrustum(modelMatrix.GetTRSPosition() + mesh.CenteredBounds.Center,
                    mesh.CenteredBounds.Diagonal * Transform.Scale);
            }
        }

        private Mesh mesh;
        private Material material;

        private void CreateRenderTask()
        {
            if (Mesh && Material)
                CreateRenderTask(Mesh.RenderTaskData, Mesh.Indices, Material, Mesh.RuntimeId);
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

        private void OnMeshChanged()
        {
            if (Mesh.IsValid)
                CreateRenderTask(Mesh.RenderTaskData, Mesh.Indices, Material, Mesh.RuntimeId);
        }
    }
}