using DamnEngine;
using DamnEngine.Render;
using OpenTK.Mathematics;

namespace Puzzle
{
    public static class PuzzleLevel
    {
        public const int WorldSize = 5;

        public const int WorldHalfSize = WorldSize / 2;

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

            CreateMesh(new Vector3(2, 10, 3), "Cube.obj", "Grid.png");

            var material = Material.CreateFromShadersFiles("Texture");
            material.SetTexture(0, PuzzleCamera.sec.RenderTexture);
            //material.SetTexture(0, Texture2D.CreateFromFile("Grid.png"));
            var plane = Mesh.CreateMeshFromFile("Plane.obj").CreateObjectFromMesh(material);
            plane.Transform.Position = new Vector3(0, 10, 3);
            plane.Transform.LocalRotation = new Vector3(-90, 0, 0);

            plane.GetComponent<MeshRenderer>().RenderingLayer = RenderingLayers.Special;
        }

        public static GameObject CreateMesh(Vector3 position, string meshName, string textureName)
        {
            var material = Material.CreateFromShadersFiles("Default");
            var material2 = Material.CreateFromShadersFiles("Default");
            material.SetTexture(0, Texture2D.CreateFromFile(textureName));
            material2.SetTexture(0, Texture2D.CreateFromFile(textureName));

            var cube = Mesh.CreateMeshesFromFile(meshName).CreateObjectsFromMeshes(new[] { material, material2 });

            cube.Transform.Position = position;

            return cube;
        }

        private static PuzzleLevelCube CreateLevelCube(Vector3 position) =>
            CreateMesh(position, "PuzzleCube.obj", "Cube.png").AddComponent<PuzzleLevelCube>();
    }
}