using DamnEngine;
using DamnEngine.Render;
using OpenTK.Mathematics;

namespace FallingBall
{
    public class FallingBallPlayer : Component
    {
        private MeshRenderer meshRenderer;
        private SphereCollider sphereCollider;
        private RigidBody rigidBody;
        
        protected override void OnCreate()
        {
            Transform.Position = Vector3.UnitY * 10;
            
            var mesh = Mesh.CreateMeshFromFile("Default/Sphere.obj");
            meshRenderer = AddComponent<MeshRenderer>();
            meshRenderer.Mesh = mesh;
            meshRenderer.Material = Material.DefaultMaterial;

            sphereCollider = AddComponent<SphereCollider>();

            rigidBody = AddComponent<RigidBody>();
        }
    }
}