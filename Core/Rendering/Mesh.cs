using System;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public class Mesh : DamnObject
    {
        public Action OnMeshChanged { get; set; }
        public string OriginalMeshName { get; }

        public bool IsValid => Vertices != null && Uv != null && Normals != null && Indices != null;

        public Vector3[] Vertices
        {
            get => vertices;
            set
            {
                if (!VerifyMeshChanges(value, Uv, Normals, Indices, IsValid))
                    return;
                
                vertices = value;
                renderTaskDataIsDirty = true;
                OnMeshChanged?.Invoke();
            }
        }
        public Vector2[] Uv
        {
            get => uv;
            set
            {
                if (!VerifyMeshChanges(Vertices, value, Normals, Indices, IsValid))
                    return;
                
                uv = value;
                renderTaskDataIsDirty = true;
                OnMeshChanged?.Invoke();
            }
        }
        public Vector3[] Normals
        {
            get => normals;
            set
            {
                if (!VerifyMeshChanges(Vertices, Uv, value, Indices, IsValid))
                    return;
                
                normals = value;
                renderTaskDataIsDirty = true;
                OnMeshChanged?.Invoke();
            }
        }
        public int[] Indices
        {
            get => indices;
            set
            {
                if (!VerifyMeshChanges(Vertices, Uv, Normals, value, IsValid))
                    return;
                
                indices = value;
                renderTaskDataIsDirty = true;
                OnMeshChanged?.Invoke();
            }
        }
        public Triangle[] Triangles
        {
            get
            {
                var triangles = new Triangle[Indices.Length / 3];
                for (var i = 0; i < Indices.Length; i += 3)
                {
                    var a = Vertices[Indices[i + 0]];
                    var b = Vertices[Indices[i + 1]];
                    var c = Vertices[Indices[i + 2]];

                    triangles[i / 3] = new Triangle(a, b, c);
                }

                return triangles;
            }
        }

        public Bounds CenteredBounds { get; private set; }

        public float[] RenderTaskData
        {
            get
            {
                if (renderTaskData != null && !renderTaskDataIsDirty)
                    return renderTaskData;
                
                renderTaskData = new float[Vertices.Length * 8];
                for (var i = 0; i < renderTaskData.Length; i += 8)
                {
                    var vertex = Vertices[i / 8];
                    var uv = Uv[i / 8];
                    var normal = Normals[i / 8];

                    renderTaskData[i + 0] = vertex.X;
                    renderTaskData[i + 1] = vertex.Y;
                    renderTaskData[i + 2] = vertex.Z;
                    renderTaskData[i + 3] = uv.X;
                    renderTaskData[i + 4] = uv.Y;
                    renderTaskData[i + 5] = normal.X;
                    renderTaskData[i + 6] = normal.Y;
                    renderTaskData[i + 7] = normal.Z;
                }

                renderTaskDataIsDirty = false;
                return renderTaskData;
            }
        }

        private Vector3[] vertices;
        private Vector2[] uv;
        private Vector3[] normals;
        private int[] indices;
        
        private float[] renderTaskData;

        private bool renderTaskDataIsDirty;

        public Mesh(string name)
        {
            OriginalMeshName = name;
            Name = name;
        }

        public void UpdateBounds()
        {
            var max = new Vector3();

            for (var i = 0; i < Vertices.Length; i++)
            {
                var vertex = Vertices[i].Abs();
                if (max.X < vertex.X)
                    max.X = vertex.X;
                if (max.Y < vertex.Y)
                    max.Y = vertex.Y;
                if (max.Z < vertex.Z)
                    max.Z = vertex.Z;
            }

            CenteredBounds = new Bounds(Vector3.Zero, max);
        }

        public void UpdateNormals()
        {
            static Vector3 GetFaceNormal(Vector3 p1, Vector3 p2, Vector3 p3) 
            {
                var a = p3 - p2;
                var b = p1 - p2;
                return Vector3.Normalize(Vector3.Cross(a, b));
            }

            var normals = new Vector3[Vertices.Length];
            for (var i = 0; i < Indices.Length; i += 3)
            {
                var p1 = Vertices[Indices[i + 0]];
                var p2 = Vertices[Indices[i + 1]];
                var p3 = Vertices[Indices[i + 2]];
                var normal = GetFaceNormal(p1, p2, p3);
                normals[Indices[i + 0]] += normal;
                normals[Indices[i + 1]] += normal;
                normals[Indices[i + 2]] += normal;
            }

            for (var i = 0; i < normals.Length; i++)
                normals[i] = normals[i].Normalized();

            Normals = normals;
        }
        
        protected override void OnDestroy()
        {
            ResourcesLoader.FreeMesh(OriginalMeshName);
        }
        
        public static Mesh[] CreateFromFile(string meshName)
        {
            return ResourcesLoader.UseMeshes(meshName);
        }

        private static bool VerifyMeshChanges(Vector3[] vertices, Vector2[] uv, Vector3[] normals, int[] indices, bool isValid)
        {
            if (!isValid)
                return true;
            
            if (vertices.Length != uv.Length || uv.Length != normals.Length)
            {
                Debug.LogError($"[{nameof(Mesh)}] ({nameof(VerifyMeshChanges)}) Vertices, Uv and Normals need to have same array lengths!");
                return false;
            }

            if (indices.Length % 3 != 0)
            {
                Debug.LogError($"[{nameof(Mesh)}] ({nameof(VerifyMeshChanges)}) Mesh must to be triangulated!");
                return false;
            }

            return true;
        }
    }
}