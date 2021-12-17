﻿using System.Collections.Generic;
using DamnEngine;
using OpenTK.Mathematics;

namespace Puzzle
{
    public class PuzzlePlayer : Component
    {
        private PuzzleLevelCube TargetMovingCube => PuzzleLevel.GetCube((int)targetMovingPosition.X, (int)targetMovingPosition.Z);
        
        private readonly Dictionary<KeyCode, Vector3> inputDirections = new()
        {
            { KeyCode.W, Vector3.UnitZ },
            { KeyCode.S, -Vector3.UnitZ },
            { KeyCode.A, Vector3.UnitX },
            { KeyCode.D, -Vector3.UnitX }
        };

        private float movingSpeed = 5;

        private bool isMoving;
        private Vector3 targetMovingPosition;

        protected override void OnCreate()
        {
            Transform.Position = PuzzleLevel.StartPlayerPoint;
        }

        protected override void OnUpdate()
        {
            if (!isMoving)
                UpdateInput();
            else
                UpdateMoving();
        }

        private void UpdateInput()
        {
            foreach (var direction in inputDirections)
            {
                if (Input.IsKeyPress(direction.Key))
                {
                    var playerPosition = Transform.Position;
                    var targetCubePosition = (Vector3i)(playerPosition + direction.Value);
                    var cube = PuzzleLevel.GetCube(targetCubePosition.X, targetCubePosition.Z);
                    if (cube)
                    {
                        isMoving = true;
                        targetMovingPosition = targetCubePosition;
                        return;
                    }
                }
            }
        }

        private void UpdateMoving()
        {
            Transform.Position = Vector3Extensions.MoveTowards(Transform.Position, targetMovingPosition, movingSpeed * Time.DeltaTime);
            if (Transform.Position == targetMovingPosition)
            {
                isMoving = false;
                TargetMovingCube.Paint();
            }
        }
    }
}