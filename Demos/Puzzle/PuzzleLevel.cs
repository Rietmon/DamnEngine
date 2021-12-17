using System.Linq;
using DamnEngine;
using DamnEngine.Render;
using OpenTK.Mathematics;

namespace Puzzle
{
    public static class PuzzleLevel
    {
        public const int WorldSize = 5;

        public const int WorldHalfSize = 2;

        private static PuzzleLevelCube[,] levelCubes;

        public static PuzzleLevelCube GetCube(int x, int z)
        {
            x += WorldHalfSize;
            z += WorldHalfSize;

            if (x < 0 || z < 0 ||
                levelCubes.GetLength(0) <= x || levelCubes.GetLength(1) <= z)
                return null;

            return levelCubes[x, z];
        }

        public static void CreateLevel()
        {
            levelCubes = new PuzzleLevelCube[WorldSize, WorldSize];
            for (var x = -WorldHalfSize; x <= WorldHalfSize; x++)
            {
                for (var z = -WorldHalfSize; z <= WorldHalfSize; z++)
                {
                    levelCubes[x + WorldHalfSize, z + WorldHalfSize] = CreateLevelCube(new Vector3(x, 0, z));
                }
            }
        }

        public static GameObject CreateCube(Vector3 position, string textureName)
        {
            var cube = new GameObject("Cube");
            
            cube.Transform.Position = position;
            
            var cubeMeshRenderer = cube.AddComponent<MeshRenderer>();
            var material = Material.CreateFromShadersFiles("Default");
            material.SetTexture(0, Texture2D.CreateFromFile(textureName));
            cubeMeshRenderer.Material = material;
            cubeMeshRenderer.Mesh = Mesh.CreateMeshFromFile("Cube.obj");

            return cube;
        }

        private static PuzzleLevelCube CreateLevelCube(Vector3 position) =>
            CreateCube(position, "Cube.png").AddComponent<PuzzleLevelCube>();
    }
}