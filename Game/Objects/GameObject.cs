using System;
using System.Collections.Generic;

namespace DamnEngine
{
    public class GameObject : DamnObject
    {
        public bool IsObjectActive
        {
            get => isObjectActive;
            set
            {
                if (isObjectActive == value)
                    return;
                
                isObjectActive = value;
                if (isObjectActive)
                    ForEachComponent((component) => component.OnEnable());
                else
                    ForEachComponent((component) => component.OnDisable());
            }
        }
        public Transform Transform { get; }
        
        private readonly List<Component> components = new();

        private bool isObjectActive;

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
            component.OnCreate();

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

        public T[] GetComponents<T>()
        {
            var result = new List<T>();
            foreach (var component in components)
            {
                if (component is T castedComponent)
                    result.Add(castedComponent);
            }

            return result.ToArray();
        }
        
        public bool TryGetComponent<T>(out T component)
        {
            component = GetComponent<T>();
            return component != null;
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
            while (components.Count != 0)
                RemoveComponent(components[0]);
            ScenesManager.UnregisterGameObject(this);
        }
    }
}