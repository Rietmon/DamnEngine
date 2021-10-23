using System;
using System.Collections.Generic;

namespace DamnEngine
{
    public class Scene
    {
        public string Name { get; }
        
        internal readonly List<GameObject> gameObjects = new();

        public Scene(string name)
        {
            Name = name;
        }

        public GameObject FindGameObject(Predicate<GameObject> condition) => gameObjects.Find(condition);

        public void ForEachGameObject(Action<GameObject> gameObjectAction)
        {
            foreach (var gameObject in gameObjects)
            {
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
    }
}