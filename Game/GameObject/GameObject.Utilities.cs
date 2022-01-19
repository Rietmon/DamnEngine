using System;
using System.Collections.Generic;

namespace DamnEngine
{
    public partial class GameObject
    {
        public T AddComponent<T>() where T : Component, new()
        {
            var component = new T
            {
                GameObject = this
            };
            component.ForceRegister(PipelineTiming.Now);

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
            if (component is Transform && !IsDestroying)
                return false;
            
            component.Internal_DestroyFromGameObject();

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
    }
}