using System.Threading.Tasks;
using DamnEngine;
using OpenTK.Mathematics;
using Rietmon.Extensions;

namespace Puzzle
{
    public static class PuzzleScene
    {
        public static void CreateScene()
        {
            ScenesManager.SetScene(new Scene("Puzzle"));
            
            var camera = GameObject.CreateObject("MainCamera");
            camera.AddComponent<PuzzleCamera>();
            
            PuzzleLevel.CreateLevel();

            var player = PuzzleLevel.CreateMesh(Vector3.Zero, "Default/Cube.obj", "Grid.png");
            player.AddComponent<PuzzlePlayer>();
        }
    }
}