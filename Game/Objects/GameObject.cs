using System;
using System.Collections.Generic;

namespace DamnEngine
{
    public class GameObject : DamnObject
    {
        public Transform Transform { get; }
        
        private readonly List<Component> components = new();

        public GameObject(string name = "GameObject")
        {
            Name = name;

            Transform = AddComponent<Transform>();
            
            ScenesManager.RegisterGameObject(this);
        }

        public T AddComponent<T>() where T : Component
        {
            var component = Activator.CreateInstance<T>();
            component.GameObject = this;

            components.Add(component);

            return component;
        }

        public T GetComponent<T>()
        {
            foreach (var component in components)
            {
                if (component is T result)
                    return result;
            }

            return default;
        }

        public void RemoveComponent<T>() => RemoveComponent((Component)(object)GetComponent<T>());

        public void RemoveComponent<T>(T component) where T : Component
        {
            component.GameObject = null;
            component.Destroy();

            components.Remove(component);
        }

        internal void ForEachComponent(Action<Component> componentAction)
        {
            foreach (var component in components)
            {
                componentAction.Invoke(component);
            }
        }

        internal void ForEachComponent<T>(Action<T> componentAction)
        {
            foreach (var component in components)
            {
                if (component is T result)
                    componentAction.Invoke(result);
            }
        }

        protected override void OnDestroy()
        {
            ScenesManager.UnregisterGameObject(this);
        }
    }
}