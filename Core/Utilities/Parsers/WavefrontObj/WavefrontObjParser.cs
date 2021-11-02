using System.IO;
using OpenTK;
using OpenTK.Mathematics;

namespace DamnEngine.Utilities
{
    public static class WavefrontObjParser
    {
        public static Mesh[] ParseObj(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var obj = ParseObj(lines);
            var meshes = obj.ToMeshes();
            return meshes;
        }
        
        private static WavefrontObj ParseObj(string[] lines)
        {
            var obj = new WavefrontObj();
            WavefrontObjMesh mesh = null;
            foreach (var line in lines)
            {
                var arguments = line.Split(' ');
                switch (arguments[0])
                {
                    case "#":
                        continue;
                    case "mtllib":
                        mesh = new WavefrontObjMesh
                        {
                            materialName = arguments[1]
                        };
                        obj.meshes.Add(mesh);
                        break;
                    case "o":
                        mesh.name = arguments[1];
                        break;
                    case "v":
                    {
                        mesh.vertices.Add(new Vector3(arguments[1].ToFloat(), arguments[2].ToFloat(), arguments[3].ToFloat()));
                        break;
                    }
                    case "vt":
                    {
                        mesh.uv.Add(new Vector2(arguments[1].ToFloat(), arguments[2].ToFloat()));
                        break;
                    }
                    case "vn":
                    {
                        mesh.normals.Add(new Vector3(arguments[1].ToFloat(), arguments[2].ToFloat(), arguments[3].ToFloat()));
                        break;
                    }
                    case "f":
                    {
                        if (arguments.Length != 4)
                        {
                            Debug.LogError(
                                $"[{nameof(WavefrontObjParser)}] ({nameof(ParseObj)}) Mesh {mesh.name} is not triangulated!");
                            return null;
                        }

                        var faceVertices = new WavefrontObjFaceVertex[3];
                        for (var i = 0; i < 3; i++)
                        {
                            var faceArguments = arguments[i + 1].Split('/');
                            faceVertices[i] = new WavefrontObjFaceVertex(faceArguments[0].ToInt(),
                                faceArguments[1].ToInt(), faceArguments[2].ToInt());
                        }
                        
                        mesh.faces.Add(new WavefrontObjFace(faceVertices));
                        break;
                    }
                }
            }

            return obj;
        }

        private static float ToFloat(this string str) => float.Parse(str);

        private static int ToInt(this string str) => int.Parse(str);
    }
}