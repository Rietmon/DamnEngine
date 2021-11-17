using System.Collections.Generic;
using OpenTK.Mathematics;

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
            GetGreaterArray(out var greaterCount, out var greaterArrayIndex);
            
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
                    var index = faceVertex.GetIndexByArray(greaterArrayIndex);

                    meshVertices[index] = vertices[faceVertex.vertexIndex];
                    meshUv[index] = uv[faceVertex.uvIndex];
                    meshNormals[index] = normals[faceVertex.normalIndex];
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
            return mesh;
        }

        private void GetGreaterArray(out int greaterCount, out int greaterArrayIndex)
        {
            greaterCount = vertices.Count;
            greaterArrayIndex = 0;
            if (greaterCount < uv.Count)
            {
                greaterCount = uv.Count;
                greaterArrayIndex = 1;
            }

            if (greaterCount < normals.Count)
            {
                greaterCount = normals.Count;
                greaterArrayIndex = 2;
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
        public readonly int vertexIndex;
        public readonly int uvIndex;
        public readonly int normalIndex;

        public WavefrontObjFaceVertex(int vertexIndex, int uvIndex, int normalIndex)
        {
            this.vertexIndex = vertexIndex - 1;
            this.uvIndex = uvIndex - 1;
            this.normalIndex = normalIndex - 1;
        }

        public int GetIndexByArray(int arrayIndex)
        {
            return arrayIndex switch
            {
                0 => vertexIndex,
                1 => uvIndex,
                2 => normalIndex,
                _ => -1
            };
        }
    }
}