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

        private GameObject() : base(PipelineTiming.Never) { }

        protected override void OnRegister()
        {
            ScenesManager.RegisterGameObject(this);
            Transform = AddComponent<Transform>();
        }

        public override void Destroy()
        {
            base.Destroy();
            while (components.Count != 0)
                RemoveComponent(components[0]);
        }

        protected override void OnDestroy()
        {
            ScenesManager.UnregisterGameObject(this);
        }

        public static GameObject CreateObject(string name)
        {
            var gameObject = new GameObject
            {
                Name = name
            };
            gameObject.ForceRegister(PipelineTiming.Now);
            return gameObject;
        }
    }
}