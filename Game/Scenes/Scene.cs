using System;
using System.Collections.Generic;
using DamnEngine.Serialization;

namespace DamnEngine
{
    [Serializable]
    public sealed class Scene : DamnObject
    {
        [SerializeField] internal List<GameObject> gameObjects = new();

        public Scene(string name) : base(PipelineTiming.Now)
        {
            Name = name;
        }

        public GameObject FindGameObject(Predicate<GameObject> condition)
        {
            var gameObject = gameObjects.Find(condition);
            return gameObject;
        }

        public GameObject FindGameObjectByName(string name) => FindGameObject((gameObject) => gameObject.Name == name);

        public void ForEachGameObject(Action<GameObject> gameObjectAction)
        {
            for (var i = 0; i < gameObjects.Count; i++)
            {
                var gameObject = gameObjects[i];
                gameObjectAction.Invoke(gameObject);
            }
        }
        
        public void ForEachActiveGameObject(Action<GameObject> gameObjectAction)
        {
            for (var i = 0; i < gameObjects.Count; i++)
            {
                var gameObject = gameObjects[i];
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