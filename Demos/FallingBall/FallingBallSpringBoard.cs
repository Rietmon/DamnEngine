using DamnEngine;
using DamnEngine.Render;

namespace FallingBall
{
    public class FallingBallSpringBoard : Component
    {
        private MeshRenderer meshRenderer;

        private MeshCollider meshCollider;
        
        protected override void OnCreate()
        {
            var mesh = Mesh.CreateMeshFromFile("FallingBall/SpringBoard.obj");
            var texture = Texture2D.CreateFromFile("Grid.png");
            meshRenderer = AddComponent<MeshRenderer>();
            meshRenderer.Mesh = mesh;
            var material = Material.DefaultMaterial;
            material.SetTexture(0, texture);
            meshRenderer.Material = material;

            meshCollider = AddComponent<MeshCollider>();
            meshCollider.Mesh = mesh;
        }
    }
}