using DamnEngine;

namespace Puzzle
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
            PuzzleScene.CreateScene();
        }
    }
}