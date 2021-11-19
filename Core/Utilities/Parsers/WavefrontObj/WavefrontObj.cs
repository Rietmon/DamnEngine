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
        public List<Vector3> vertices = new();
        public List<Vector2> uv = new();
        public List<Vector3> normals = new();
        public List<WavefrontObjFace> faces = new();

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
                for (var j = 0; j < faceVertices.Length; j++)
                {
                    var vertexIndex = faceVertices[j].vertexIndex;
                    if (originalVertices.Contains(vertexIndex))
                    {
                        newVertices.Add(vertices[vertexIndex]);
                        faceVertices[j].vertexIndex = newVertices.Count - 1;
                    }
                    else
                    {
                        originalVertices.Add(vertexIndex);
                    }
                    
                    var uvIndex = faceVertices[j].uvIndex;
                    if (originalUv.Contains(uvIndex))
                    {
                        newUv.Add(uv[uvIndex]);
                        faceVertices[j].uvIndex = newUv.Count - 1;
                    }
                    else
                    {
                        originalUv.Add(uvIndex);
                    }
                    
                    var normalIndex = faceVertices[j].normalIndex;
                    if (originalNormals.Contains(normalIndex))
                    {
                        newNormals.Add(normals[normalIndex]);
                        faceVertices[j].normalIndex = newNormals.Count - 1;
                    }
                    else
                    {
                        originalNormals.Add(normalIndex);
                    }
                }
            }

            var subWavefrontObj = new WavefrontObjMesh()
            {
                name = name,
                materialName = materialName,
                vertices = newVertices,
                uv = newUv,
                normals = newNormals,
                faces = faces
            };
            
            subWavefrontObj.GetGreaterArray(out var greaterCount, out var greaterArrayIndex);
            
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
        public int vertexIndex;
        public int uvIndex;
        public int normalIndex;

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