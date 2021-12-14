using System;
using System.Collections.Generic;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities.Memory;
using OpenTK.Mathematics;
using Rietmon.Extensions;

namespace DamnEngine
{
    public unsafe class MeshCollider : Collider
    {
        private static readonly BufferPool meshCollidersBufferPool = new();

        public override bool CanCreateShape => mesh;

        public Vector3 Center { get; set; } = Vector3.Zero;

        public Mesh Mesh
        {
            get => mesh;
            set
            {
                if (mesh == value)
                    return;

                mesh = value;
                if (mesh)
                    TryCreateStaticShape();
                else
                    TryRemoveStaticShape();
            }
        }
        
        public override Bounds Bounds  => new(Center, Transform.Scale);

        public override IShape Shape
        {
            get
            {
                var trianglesPointer = CreateTrianglesPointer();
                var buffer = new Buffer<BepuPhysics.Collidables.Triangle>(trianglesPointer, trianglesCount);
                collidableMesh = new BepuPhysics.Collidables.Mesh(buffer, Transform.Scale.ToNumericsVector3(),
                    meshCollidersBufferPool);
                return collidableMesh;
            }
        }
        public override TypedIndex ShapeIndex => GetShape((BepuPhysics.Collidables.Mesh)Shape);
        public override Vector3 ShapePosition => Center + Transform.Position;

        private BepuPhysics.Collidables.Mesh collidableMesh;

        private Mesh mesh;
        
        private int trianglesCount;
        private int trianglesPointerSize;
        private BepuPhysics.Collidables.Triangle* trianglesPointer;

        private BepuPhysics.Collidables.Triangle* CreateTrianglesPointer()
        {
            if (trianglesPointer != null)
                MemoryUtilities.Free(trianglesPointer);

            var triangles = Mesh.Triangles;
            trianglesCount = triangles.Length;
            trianglesPointerSize = trianglesCount * 12 * 3;
            trianglesPointer = (BepuPhysics.Collidables.Triangle*)MemoryUtilities.Allocate(trianglesPointerSize);
            for (var i = 0; i < trianglesCount; i++)
            {
                trianglesPointer[i] = new BepuPhysics.Collidables.Triangle(triangles[i].A.FromToBepuPosition().ToNumericsVector3(),
                    triangles[i].B.FromToBepuPosition().ToNumericsVector3(), 
                    triangles[i].C.FromToBepuPosition().ToNumericsVector3());
            }
            
            return trianglesPointer;
        }

        internal override void ComputeInertia(float mass, out BodyInertia inertia) =>
            collidableMesh.ComputeOpenInertia(mass, out inertia);

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (trianglesPointer != null)
                MemoryUtilities.Free(trianglesPointer);
        }
    }
}