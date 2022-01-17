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
            var texture = Texture2D.CreateFromFile("Grid.png");
            meshRenderer = AddComponent<MeshRenderer>();
            meshRenderer.Mesh = mesh;
            var material = Material.DefaultMaterial;
            material.SetTexture(0, texture);
            meshRenderer.Material = material;

            sphereCollider = AddComponent<SphereCollider>();

            rigidBody = AddComponent<RigidBody>();
        }

        protected override void OnUpdate()
        {
            var velocity = rigidBody.BodyReference.Velocity.Linear;
            velocity.X = Mathf.Clamp(velocity.X, -10f, 0);
            rigidBody.BodyReference.Velocity.Linear = velocity;
        }
    }
}