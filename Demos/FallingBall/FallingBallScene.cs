using DamnEngine;
using OpenTK.Mathematics;

namespace FallingBall
{
    public static class FallingBallScene
    {
        public static void CreateScene()
        {
            ScenesManager.SetScene(new Scene("FallingBall"));

            var player = GameObject.CreateObject("Player");
            player.AddComponent<FallingBallPlayer>();

            var level = GameObject.CreateObject("Level");
            level.AddComponent<FallingBallLevel>();

            var camera = GameObject.CreateObject("MainCamera");
            camera.AddComponent<FallingBallCamera>();
        }
    }
}