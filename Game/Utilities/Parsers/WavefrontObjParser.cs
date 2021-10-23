using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenTK;

namespace DamnEngine.Utilities
{
    public static class WavefrontObjParser
    {
        public static Mesh Parse(string path)
        {
            var lines = File.ReadAllLines(path);
            var name = string.Empty;
            var parsedVertices = new List<Vector3>();
            var parsedUvs = new List<Vector2>();
            var parsedNormals = new List<Vector3>(); // NOT USED NOW
            var parsedFaces = new List<WavefrontObjFace>();
            foreach (var line in lines)
            {
                var arguments = line.Split(' ');
                switch (arguments[0])
                {
                    case "#":
                    case "mtllib":
                        break;
                    case "o":
                        name = arguments[1];
                        break;
                    case "v":
                    {
                        var vertex = new Vector3(float.Parse(arguments[1]), float.Parse(arguments[2]),
                            float.Parse(arguments[3]));
                        parsedVertices.Add(vertex);
                        break;
                    }
                    case "vt":
                    {
                        var uv = new Vector2(float.Parse(arguments[1]), float.Parse(arguments[2]));
                        parsedUvs.Add(uv);
                        break;
                    }
                    case "vn":
                    {
                        var normal = new Vector3(float.Parse(arguments[1]), float.Parse(arguments[2]),
                            float.Parse(arguments[3]));
                        parsedNormals.Add(normal);
                        break;
                    }
                    case "f":
                    {
                        if (arguments.Length != 4)
                        {
                            Debug.LogError(
                                $"[{nameof(WavefrontObjParser)}] ({nameof(Parse)}) Mesh is not triangulated!");
                            return null;
                        }

                        var faceVertices = new WavefrontObjFaceVertex[3];
                        for (var i = 0; i < 3; i++)
                        {
                            var faceVertex = arguments[i + 1];
                            var values = faceVertex.Split('/');
                            var vertex = new WavefrontObjFaceVertex(int.Parse(values[0]) - 1, int.Parse(values[1]) - 1,
                                int.Parse(values[2]) - 1);
                            faceVertices[i] = vertex;
                        }

                        var face = new WavefrontObjFace(faceVertices);
                        parsedFaces.Add(face);
                        break;
                    }
                }
            }

            Vector2[] uvs = null;
            Vector3[] vertices = null;
            var indices = new int[parsedFaces.Count * 3];
            var uvsCount = parsedUvs.Count;
            vertices = new Vector3[uvsCount];
            uvs = new Vector2[uvsCount];

            for (var i = 0; i < parsedFaces.Count; i++)
            {
                var face = parsedFaces[i];
                for (var j = 0; j < 3; j++)
                {
                    var vertex = face.vertices[j];
                    indices[i * 3 + j] = parsedFaces[i].vertices[j].uvIndex;
                    vertices[vertex.uvIndex] = parsedVertices[vertex.vertexIndex];
                    uvs[vertex.uvIndex] = parsedUvs[vertex.uvIndex];
                }
            }

            return new Mesh
            {
                Name = name,
                Vertices = vertices,
                Uv = uvs,
                Indices = indices
            };
        }

        private readonly struct WavefrontObjFace
        {
            public readonly WavefrontObjFaceVertex[] vertices;

            public WavefrontObjFace(WavefrontObjFaceVertex[] vertices)
            {
                this.vertices = vertices;
            }
        }
        
        private struct WavefrontObjFaceVertex
        {
            public int vertexIndex;
            public int uvIndex;
            public int normalIndex;

            public WavefrontObjFaceVertex(int vertexIndex, int uvIndex, int normalIndex)
            {
                this.vertexIndex = vertexIndex;
                this.uvIndex = uvIndex;
                this.normalIndex = normalIndex;
            }
        }
    }
}