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
            meshRenderer = AddComponent<MeshRenderer>();
            meshRenderer.Mesh = mesh;
            meshRenderer.Material = Material.DefaultMaterial;

            meshCollider = AddComponent<MeshCollider>();
            meshCollider.Mesh = mesh;
        }
    }
}