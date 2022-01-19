using System;
using System.Collections.Generic;
using DamnEngine.Serialization;

namespace DamnEngine
{
    [Serializable]
    public sealed partial class GameObject : DamnObject
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

        public Transform Transform { get; private set; }
        
        internal readonly List<Component> components = new();

        private bool isObjectActive = true;

        private GameObject(string name = "GameObject") : base(PipelineTiming.Never)
        {
            Name = name;
        }

        protected override void OnRegister()
        {
            ScenesManager.RegisterGameObject(this);
            Transform = AddComponent<Transform>();
        }

        protected override void OnDestroy()
        {
            while (components.Count != 0)
                RemoveComponent(components[0]);
            ScenesManager.UnregisterGameObject(this);
        }

        public static GameObject CreateObject(string name)
        {
            var gameObject = new GameObject(name);
            gameObject.ForceRegister(PipelineTiming.Now);
            return gameObject;
        }
    }
}