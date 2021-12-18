using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public partial class Transform
    {
        public Transform Parent
        {
            get => parent;
            set
            {
                if (parent == this || parent == value)
                    return;

                var oldParent = parent;
                parent = value;
                OnChangeParent(oldParent, value);
            }
        }

        public int ChildesCount => childes.Count;

        private readonly List<Transform> childes = new();

        private Transform parent;

        public Transform GetChild(int index = 0) => childes[index];
        
        public Transform[] GetChildes() => childes.ToArray();

        public T AddComponentToChild<T>(int childIndex = 0) where T : Component, new() => childes[childIndex].AddComponent<T>();
        public T GetComponentInChild<T>(int childIndex = 0) => childes[childIndex].GetComponent<T>();
        public T[] GetComponentsInChild<T>(int childIndex = 0) => childes[childIndex].GetComponents<T>();
        public bool TryGetComponentInChild<T>(out T component, int childIndex = 0) => childes[childIndex].TryGetComponent(out component);
        public void RemoveComponentFromChild<T>(int childIndex = 0) => childes[childIndex].RemoveComponent<T>();
        public void RemoveComponentFromChild<T>(T component, int childIndex) where T : Component => childes[childIndex].RemoveComponent(component);

        public List<Transform> GetAllChildes(bool includeRoot = false)
        {
            var result = new List<Transform>();
            
            void GetAllSubChildes(Transform start)
            {
                for (var i = 0; i < start.ChildesCount; i++)
                {
                    result.Add(start.GetChild(i));
                    GetAllSubChildes(start.GetChild(i));
                }
            }

            if (includeRoot)
                result.Add(this);
            
            GetAllSubChildes(this);

            return result;
        }

        public void ForEachChild(Action<Transform> action, bool includeRoot = false)
        {
            var childes = GetAllChildes(includeRoot);
            foreach (var child in childes)
                action.Invoke(child);
        }

        private void OnChangeParent(Transform oldParent, Transform newParent)
        {
            if (oldParent)
            {
                oldParent.childes.Remove(this);
                oldParent.CallOnTransformChanged();
            }

            if (newParent)
            {
                newParent.childes.Add(this);
                newParent.CallOnTransformChanged();
            }
            
            CallOnTransformChanged();
        }
    }
}