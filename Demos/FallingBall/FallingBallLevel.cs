using System.Collections.Generic;
using System.Linq;
using DamnEngine;
using DamnEngine.Render;
using OpenTK.Mathematics;

namespace FallingBall
{
    public class FallingBallLevel : Component
    {
        private const float SpawnDistance = 200;

        private const float DestroyDistance = -5;

        private readonly List<FallingBallSpringBoard> springBoards = new();
        
        private Transform playerTransform;

        protected override void OnCreate()
        {
            playerTransform = ScenesManager.FindGameObjectByName("Player");
        }

        protected override void OnUpdate()
        {
            while (true)
            {
                var springBoard = springBoards.LastOrDefault()?.Transform;
                if (!springBoard || springBoard.Position.Z - playerTransform.Position.Z >= SpawnDistance)
                    CreateSpringBoard();
                else
                    break;
            }

            while (true)
            {
                var springBoard = springBoards.First().Transform;
                if (springBoard.Position.Z - playerTransform.Position.Z <= DestroyDistance)
                    DestroySpringBoard();
                else
                    return;
            }
        }

        private void CreateSpringBoard()
        {
            var lastSpringBoardNullablePosition = springBoards.LastOrDefault()?.Transform?.Position;
            var lastSpringBoardPosition = lastSpringBoardNullablePosition ?? Vector3.Zero;
            var newSpringBoardPosition = lastSpringBoardPosition + new Vector3(0, -5, 7);
            var springBoardObject = new GameObject($"SpringBoard {newSpringBoardPosition}");
            var springBoard = springBoardObject.AddComponent<FallingBallSpringBoard>();
            springBoards.Add(springBoard);
        }

        private void DestroySpringBoard()
        {
            var springBoard = springBoards.First();
            springBoards.RemoveAt(0);
            springBoard.Destroy();
        }
    }
}