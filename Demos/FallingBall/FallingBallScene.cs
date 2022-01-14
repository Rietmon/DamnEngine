using DamnEngine;
using OpenTK.Mathematics;

namespace FallingBall
{
    public static class FallingBallScene
    {
        public static void CreateScene()
        {
            ScenesManager.SetScene(new Scene("FallingBall"));

            var player = new GameObject("Player");
            player.AddComponent<FallingBallPlayer>();

            var camera = new GameObject("MainCamera");
            camera.AddComponent<FallingBallCamera>();
        }
    }
}