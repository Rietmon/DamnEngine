using System;
using System.Collections.Generic;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities.Memory;
using OpenTK.Mathematics;
using Rietmon.Extensions;

namespace DamnEngine
{
    // TODO: Make recalculate mesh data!
    public unsafe class MeshCollider : Collider
    {
        private static readonly Dictionary<uint, TrianglesData> cachedTrianglesData = new();
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
                trianglesData = GetTrianglesData();
                var buffer = new Buffer<BepuPhysics.Collidables.Triangle>(trianglesData.TrianglesPointer, trianglesData.TrianglesCount);
                collidableMesh = new BepuPhysics.Collidables.Mesh(buffer, Transform.Scale.ToNumericsVector3(),
                    meshCollidersBufferPool);
                return collidableMesh;
            }
        }
        public override TypedIndex ShapeIndex => GetShape((BepuPhysics.Collidables.Mesh)Shape);
        public override Vector3 ShapePosition => Center + Transform.Position;

        private BepuPhysics.Collidables.Mesh collidableMesh;

        private Mesh mesh;

        private TrianglesData trianglesData;

        private TrianglesData GetTrianglesData()
        {
            if (cachedTrianglesData.TryGetValue(Mesh.RuntimeId, out var trianglesData))
                return trianglesData;
            
            var triangles = Mesh.Triangles;
            var trianglesCount = triangles.Length;
            var trianglesPointerSize = trianglesCount * 12 * 3;
            var trianglesPointer = (BepuPhysics.Collidables.Triangle*)MemoryUtilities.Allocate(trianglesPointerSize);
            for (var i = 0; i < trianglesCount; i++)
            {
                trianglesPointer[i] = new BepuPhysics.Collidables.Triangle(triangles[i].A.FromToBepuPosition().ToNumericsVector3(),
                    triangles[i].B.FromToBepuPosition().ToNumericsVector3(), 
                    triangles[i].C.FromToBepuPosition().ToNumericsVector3());
            }

            trianglesData = new TrianglesData(trianglesCount, trianglesPointer);
            cachedTrianglesData.Add(Mesh.RuntimeId, trianglesData);
            
            return trianglesData;
        }

        internal override void ComputeInertia(float mass, out BodyInertia inertia) =>
            collidableMesh.ComputeOpenInertia(mass, out inertia);

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (trianglesData.IsValid())
            {
                trianglesData.Free();
                trianglesData = default;
            }
        }
    }

    internal readonly unsafe struct TrianglesData
    {
        public int TrianglesCount { get; }
        public int TrianglesPointerSize => TrianglesCount * 12 * 3;
        public BepuPhysics.Collidables.Triangle* TrianglesPointer { get; }

        public TrianglesData(int trianglesCount, BepuPhysics.Collidables.Triangle* trianglesPointer)
        {
            TrianglesCount = trianglesCount;
            TrianglesPointer = trianglesPointer;
        }

        public bool IsValid() => TrianglesCount == 0;

        public void Free()
        {
            MemoryUtilities.Free(TrianglesPointer);
        }
    }
}