namespace DamnEngine
{
    public static class Time
    {
        public static float DeltaTime { get; private set; }
        
        public static float AllTime { get; private set; }

        public static void Update(float deltaTime)
        {
            DeltaTime = deltaTime;
            AllTime += deltaTime;
        }
    }
}