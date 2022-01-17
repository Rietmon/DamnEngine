using System;
using System.Collections.Generic;
using DamnEngine.Serialization;

namespace DamnEngine
{
    [Serializable]
    public sealed class Scene : DamnObject
    {
        internal List<GameObject> gameObjectsToDestroy = new();
        internal List<GameObject> gameObjectsToCreate = new();
        
        [SerializeField] internal List<GameObject> gameObjects = new();

        public Scene(string name)
        {
            Name = name;
        }

        public GameObject FindGameObject(Predicate<GameObject> condition)
        {
            var gameObject = gameObjects.Find(condition);
            return !gameObject ? gameObjectsToCreate.Find(condition) : gameObject;
        }

        public GameObject FindGameObjectByName(string name) => FindGameObject((gameObject) => gameObject.Name == name);

        public void ForEachGameObject(Action<GameObject> gameObjectAction)
        {
            foreach (var gameObject in gameObjects)
            {
                gameObjectAction.Invoke(gameObject);
            }
        }
        public void ForEachActiveGameObject(Action<GameObject> gameObjectAction)
        {
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.IsObjectActive)
                    gameObjectAction.Invoke(gameObject);
            }
        }

        public void ForEachGameObjectComponent(Action<Component> componentAction)
        {
            ForEachGameObject((gameObject) =>
            {
                gameObject.ForEachComponent(componentAction);
            });
        }

        public void ForEachActiveGameObjectEnabledComponent(Action<Component> componentAction)
        {
            ForEachGameObject((gameObject) =>
            {
                gameObject.ForEachEnabledComponent(componentAction);
            });
        }
    }
}