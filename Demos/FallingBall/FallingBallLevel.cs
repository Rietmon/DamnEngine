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

        private const float DestroyDistance = -20;

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
                if (!springBoard || Mathf.Abs(springBoard.Position.X) - Mathf.Abs(playerTransform.Position.X) <= SpawnDistance)
                    CreateSpringBoard();
                else
                    break;
            }

            while (true)
            {
                var springBoard = springBoards.First().Transform;
                if (Mathf.Abs(springBoard.Position.X) - Mathf.Abs(playerTransform.Position.X) <= DestroyDistance)
                    DestroySpringBoard();
                else
                    return;
            }
        }

        private void CreateSpringBoard()
        {
            var lastSpringBoardNullablePosition = springBoards.LastOrDefault()?.Transform?.Position;
            var lastSpringBoardPosition = lastSpringBoardNullablePosition ?? Vector3.Zero - new Vector3(-7, -5, 0);
            var newSpringBoardPosition = lastSpringBoardPosition + new Vector3(-7, -5, 0);
            
            //Debug.Log($"[{nameof(FallingBallLevel)}] ({nameof(CreateSpringBoard)}) Created SpringBoard at {newSpringBoardPosition}");
            
            var springBoardObject = GameObject.CreateObject($"SpringBoard {newSpringBoardPosition}");
            springBoardObject.Transform.Position = newSpringBoardPosition;
            var springBoard = springBoardObject.AddComponent<FallingBallSpringBoard>();
            springBoards.Add(springBoard);
        }

        private void DestroySpringBoard()
        {
            var springBoard = springBoards.First();
            springBoards.RemoveAt(0);
            springBoard.DestroyGameObject();
            
            //Debug.Log($"[{nameof(FallingBallLevel)}] ({nameof(CreateSpringBoard)}) Destroy SpringBoard at {springBoard.Transform.Position}");
        }
    }
}