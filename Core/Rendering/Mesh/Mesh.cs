using System;
using System.Linq;
using OpenTK.Mathematics;
using Rietmon.Extensions;

namespace DamnEngine
{
    public sealed class Mesh : DamnObject
    {
        public Action OnMeshChanged { get; set; }
        public string OriginalMeshName { get; }

        public bool IsMeshesDataDirty { get; private set; }

        public bool IsValid => Vertices != null && Uv != null && Normals != null && Indices != null;

        public Vector3[] Vertices
        {
            get => vertices;
            set
            {
                if (!VerifyMeshChanges(value, Uv, Normals, Indices, SubMeshDescriptors, IsValid))
                    return;
                
                vertices = value;
                IsMeshesDataDirty = true;
                OnMeshChanged?.Invoke();
            }
        }
        public Vector2[] Uv
        {
            get => uv;
            set
            {
                if (!VerifyMeshChanges(Vertices, value, Normals, Indices, SubMeshDescriptors, IsValid))
                    return;
                
                uv = value;
                IsMeshesDataDirty = true;
                OnMeshChanged?.Invoke();
            }
        }
        public Vector3[] Normals
        {
            get => normals;
            set
            {
                if (!VerifyMeshChanges(Vertices, Uv, value, Indices, SubMeshDescriptors, IsValid))
                    return;
                
                normals = value;
                IsMeshesDataDirty = true;
                OnMeshChanged?.Invoke();
            }
        }
        public int[] Indices
        {
            get => indices;
            set
            {
                if (!VerifyMeshChanges(Vertices, Uv, Normals, value, SubMeshDescriptors, IsValid))
                    return;
                
                indices = value;
                IsMeshesDataDirty = true;
                OnMeshChanged?.Invoke();
            }
        }
        public SubMeshDescriptor[] SubMeshDescriptors
        {
            get => subMeshDescriptors;
            set
            {
                if (!VerifyMeshChanges(Vertices, Uv, Normals, Indices, value, IsValid))
                    return;

                subMeshDescriptors = value;
                IsMeshesDataDirty = true;
                OnMeshChanged?.Invoke();
            }
        }
        public Bounds CenteredBounds { get; private set; }
        
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

        public float[][] MeshesData
        {
            get
            {
                float[] FillRenderTaskData(SubMeshDescriptor subMeshDescriptor)
                {
                    var indicesCount = subMeshDescriptor.EndIndex - subMeshDescriptor.StartIndex + 1;
                    var renderTaskData = new float[indicesCount * 8];
                    for (var i = subMeshDescriptor.StartIndex; i < subMeshDescriptor.EndIndex + 1; i++)
                    {
                        var vertex = Vertices[i];
                        var uv = Uv[i];
                        var normal = Normals[i];
                        renderTaskData[i * 8 + 0] = vertex.X;
                        renderTaskData[i * 8 + 1] = vertex.Y;
                        renderTaskData[i * 8 + 2] = vertex.Z;
                        renderTaskData[i * 8 + 3] = uv.X;
                        renderTaskData[i * 8 + 4] = uv.Y;
                        renderTaskData[i * 8 + 5] = normal.X;
                        renderTaskData[i * 8 + 6] = normal.Y;
                        renderTaskData[i * 8 + 7] = normal.Z;
                    }

                    return renderTaskData;
                }
                
                if (meshesData != null && !IsMeshesDataDirty)
                    return meshesData;

                meshesData = new float[SubMeshesCount][];
                for (var i = 0; i < meshesData.Length; i++)
                {
                    var subMeshDescriptor = GetSubMeshDescriptor(i);
                    meshesData[i] = FillRenderTaskData(subMeshDescriptor);
                }

                IsMeshesDataDirty = false;
                return meshesData;
            }
        }

        public int SubMeshesCount => SubMeshDescriptors?.Length ?? 1;

        private Vector3[] vertices;
        private Vector2[] uv;
        private Vector3[] normals;
        private int[] indices;
        private SubMeshDescriptor[] subMeshDescriptors;
        
        private float[][] meshesData;

        public Mesh(string name) : base(PipelineTiming.Now)
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

        public int[] GetSubMeshIndices(int subMeshIndex)
        {
            var subMeshDescriptor = GetSubMeshDescriptor(subMeshIndex);
            return Indices.CopyFromTo(subMeshDescriptor.StartIndex, subMeshDescriptor.EndIndex);
        }

        public SubMeshDescriptor GetSubMeshDescriptor(int index) => subMeshDescriptors != null && subMeshDescriptors.Length > index
            ? subMeshDescriptors[index]
            : new SubMeshDescriptor(0, indices.Length - 1);
        
        protected override void OnDestroy()
        {
            ResourcesLoader.FreeMesh(OriginalMeshName);
        }
        
        public static Mesh[] CreateMeshesFromFile(string meshName) => ResourcesLoader.UseMeshes(meshName);

        public static Mesh CreateMeshFromFile(string meshName) => CreateMeshesFromFile(meshName).First();

        private static bool VerifyMeshChanges(Vector3[] vertices, Vector2[] uv, Vector3[] normals, int[] indices, SubMeshDescriptor[] subMeshDescriptors, bool isValid)
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