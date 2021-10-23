namespace DamnEngine
{
    public static class ScenesManager
    {
        public static Scene CurrentScene { get; private set; }

        public static void SetScene(Scene scene)
        {
            CurrentScene?.ForEachGameObject((gameObject) =>
            {
                gameObject.Destroy();
            });

            CurrentScene = scene;
        }

        internal static void RegisterGameObject(GameObject gameObject) => CurrentScene.gameObjects.Add(gameObject);

        internal static void UnregisterGameObject(GameObject gameObject) => CurrentScene.gameObjects.Remove(gameObject);
    }
}