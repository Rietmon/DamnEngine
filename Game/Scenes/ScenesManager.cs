using System;

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
        
        public static GameObject FindGameObject(Predicate<GameObject> condition) => CurrentScene.FindGameObject(condition);
        public static GameObject FindGameObjectByName(string name) => CurrentScene.FindGameObjectByName(name);
        public static T FindGameObjectByType<T>() =>
            CurrentScene.FindGameObject((gameObject) => gameObject.GetComponent<T>() != null).GetComponent<T>();

        internal static void RegisterGameObject(GameObject gameObject) => CurrentScene.gameObjects.Add(gameObject);

        internal static void UnregisterGameObject(GameObject gameObject) => CurrentScene.gameObjects.Remove(gameObject);
    }
}