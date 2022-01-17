using System;
using System.Collections.Generic;
using DamnEngine.Serialization;

namespace DamnEngine
{
    [Serializable]
    public sealed class GameObject : DamnObject, ISerializable
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
        
        public bool IsObjectDestroying { get; private set; }
        
        public Transform Transform { get; }
        
        internal readonly List<Component> components = new();

        private bool isObjectActive = true;

        public GameObject(string name = "GameObject")
        {
            Name = name;

            Transform = AddComponent<Transform>();
            
            ScenesManager.OrderObjectToRegister(this);
        }

        public T AddComponent<T>() where T : Component, new()
        {
            var component = new T
            {
#if DEBUG
                CachedGameObjectName = Name,
#endif
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
        
        public bool RemoveComponent<T>() => RemoveComponent((Component)(object)GetComponent<T>());

        public bool RemoveComponent<T>(T component) where T : Component
        {
            if (component is Transform && !IsObjectDestroying)
                return false;
            
            component.GameObject = null;
            component.Destroy();

            components.Remove(component);

            return true;
        }

        public void ForEachComponent(Action<Component> componentAction)
        {
            for (var i = 0; i < components.Count; i++)
            {
                componentAction.Invoke(components[i]);
            }
        }

        public void ForEachEnabledComponent(Action<Component> componentAction)
        {
            for (var i = 0; i < components.Count; i++)
            {
                var component = components[i];
                if (component.IsComponentEnabled)
                    componentAction.Invoke(component);
            }
        }

        public void ForEachComponent<T>(Action<T> componentAction)
        {
            for (var i = 0; i < components.Count; i++)
            {
                var component = components[i];
                if (component is T result)
                    componentAction.Invoke(result);
            }
        }

        public void ForEachEnabledComponent<T>(Action<T> componentAction)
        {
            for (var i = 0; i < components.Count; i++)
            {
                var component = components[i];
                if (component.IsComponentEnabled && component is T result)
                    componentAction.Invoke(result);
            }
        }

        public override void Destroy()
        {
            IsObjectDestroying = true;
            ScenesManager.MarkObjectToDestroy(this);
        }

        internal void Internal_Destroy()
        {
            base.Destroy();
        }

        protected override void OnDestroy()
        {
            while (components.Count != 0)
                RemoveComponent(components[0]);
        }
    }
}