using System;
using DamnEngine;

namespace FallingBall
{
    public static class Program
    {
        private static void Main()
        {
            Engine.OnEngineStarted += OnEngineStarted;
            Engine.Run();
        }
        
        private static void OnEngineStarted()
        {
            FallingBallScene.CreateScene();
        }
    }
}