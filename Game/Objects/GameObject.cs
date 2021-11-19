using System;
using System.Collections.Generic;
using DamnEngine.Serialization;

namespace DamnEngine
{
    [Serializable]
    public class GameObject : DamnObject, ISerializable
    {
        public ISerializationObject SerializationObject => new SerializationGameObject(this);
        
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
        public Transform Transform { get; set; }
        
        internal readonly List<Component> components = new();

        private bool isObjectActive = true;

        public GameObject(string name = "GameObject")
        {
            Name = name;

            Transform = AddComponent<Transform>();
            
            ScenesManager.RegisterGameObject(this);
        }

        public T AddComponent<T>() where T : Component, new()
        {
            var component = new T
            {
                GameObject = this
            };
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
            if (component == Transform)
                Transform = null;
            
            component.GameObject = null;
            component.Destroy();

            components.Remove(component);
        }

        public void ForEachComponent(Action<Component> componentAction)
        {
            foreach (var component in components)
            {
                componentAction.Invoke(component);
            }
        }

        public void ForEachEnabledComponent(Action<Component> componentAction)
        {
            foreach (var component in components)
            {
                if (component.IsComponentEnabled)
                    componentAction.Invoke(component);
            }
        }

        public void ForEachComponent<T>(Action<T> componentAction)
        {
            foreach (var component in components)
            {
                if (component is T result)
                    componentAction.Invoke(result);
            }
        }

        public void ForEachEnabledComponent<T>(Action<T> componentAction)
        {
            foreach (var component in components)
            {
                if (component.IsComponentEnabled && component is T result)
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