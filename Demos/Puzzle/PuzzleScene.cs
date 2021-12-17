using DamnEngine;

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

            var player = PuzzleLevel.CreateCube(PuzzleLevel.StartPlayerPoint, "Grid.png");
            player.AddComponent<PuzzlePlayer>();
        }
    }
}