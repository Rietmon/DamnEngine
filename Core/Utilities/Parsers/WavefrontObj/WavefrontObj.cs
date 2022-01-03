using System.Collections.Generic;
using OpenTK.Mathematics;
using Rietmon.Extensions;

namespace DamnEngine.Utilities
{
    public class WavefrontObj
    {
        public readonly List<WavefrontObjMesh> meshes = new();

        public Mesh[] ToMeshes()
        {
            var result = new Mesh[meshes.Count];
            for (var i = 0; i < meshes.Count; i++)
                result[i] = meshes[i].ToMesh();

            return result;
        }
    }

    public class WavefrontObjMesh
    {
        public string materialName;
        public string name;
        public readonly List<Vector3> vertices = new();
        public readonly List<Vector2> uv = new();
        public readonly List<Vector3> normals = new();
        public readonly List<WavefrontObjFace> faces = new();

        public Mesh ToMesh()
        {
            var newVertices = new List<Vector3>(vertices);
            var originalVertices = new List<int>();
            var newUv = new List<Vector2>(uv);
            var originalUv = new List<int>();
            var newNormals = new List<Vector3>(normals);
            var originalNormals = new List<int>();
            for (var i = 0; i < faces.Count; i++)
            {
                var faceVertices = faces[i].faceVertices;
                FixFaceData(0, faceVertices, newVertices, originalVertices);
                FixFaceData(1, faceVertices, newUv, originalUv);
                FixFaceData(2, faceVertices, newNormals, originalNormals);
            }

            ListExtensions.GetGreaterList(out var greaterCount, out var greaterArrayIndex, newVertices, newUv, newNormals);

            var meshVertices = new Vector3[greaterCount];
            var meshUv = new Vector2[greaterCount];
            var meshNormals = new Vector3[greaterCount];
            var meshIndices = new int[faces.Count * 3];

            for (var i = 0; i < faces.Count; i++)
            {
                var face = faces[i];
                for (var j = 0; j < 3; j++)
                {
                    var faceVertex = face.faceVertices[j];
                    var index = faceVertex[greaterArrayIndex];
                    meshVertices[index] = newVertices[faceVertex.vertexIndex];
                    meshUv[index] = newUv[faceVertex.uvIndex];
                    meshNormals[index] = newNormals[faceVertex.normalIndex];
                    meshIndices[i * 3 + j] = index;
                }
            }
            
            var mesh = new Mesh(name)
            {
                Vertices = meshVertices,
                Uv = meshUv, 
                Normals = meshNormals,
                Indices = meshIndices
            };
            mesh.UpdateBounds();
            mesh.UpdateNormals();
            return mesh;
        }

        private static void FixFaceData<T>(int type, WavefrontObjFaceVertex[] faceVertices, List<T> newList, List<int> originalList)
        {
            for (var j = 0; j < faceVertices.Length; j++)
            {
                var index = faceVertices[j][type];
                if (originalList.Contains(index))
                {
                    newList.Add(newList[index]);
                    faceVertices[j][type] = newList.Count - 1;
                }
                else
                {
                    originalList.Add(index);
                }
            }
        }
    }

    public class WavefrontObjFace
    {
        public readonly WavefrontObjFaceVertex[] faceVertices;

        public WavefrontObjFace(WavefrontObjFaceVertex[] faceVertices)
        {
            this.faceVertices = faceVertices;
        }
    }

    public class WavefrontObjFaceVertex
    {
        public int vertexIndex;
        public int uvIndex;
        public int normalIndex;

        public WavefrontObjFaceVertex(int vertexIndex, int uvIndex, int normalIndex)
        {
            this.vertexIndex = vertexIndex - 1;
            this.uvIndex = uvIndex - 1;
            this.normalIndex = normalIndex - 1;
        }

        public int this[int index]
        {
            get
            {
                return index switch
                {
                    0 => vertexIndex,
                    1 => uvIndex,
                    2 => normalIndex,
                    _ => -1
                };
            }
            set
            {
                switch (index)
                {
                    case 0: vertexIndex = value;
                        return;
                    case 1: uvIndex = value;
                        return;
                    case 2: normalIndex = value;
                        return;
                }
            }
        }
    }
}