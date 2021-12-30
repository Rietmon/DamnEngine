using DamnEngine;
using OpenTK.Mathematics;

namespace Puzzle
{
    public static class PuzzleScene
    {
        public static void CreateScene()
        {
            ScenesManager.SetScene(new Scene("Puzzle"));
            
            var camera = new GameObject("MainCamera");
            camera.AddComponent<PuzzleCamera>();
            
            PuzzleLevel.CreateLevel();

            var player = PuzzleLevel.CreateMesh(Vector3.Zero, "Cube.obj", "Grid.png");
            player.AddComponent<PuzzlePlayer>();
        }
    }
}