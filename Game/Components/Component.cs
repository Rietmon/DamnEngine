using System;
using System.ComponentModel;
using DamnEngine.Serialization;

namespace DamnEngine
{
    public abstract partial class Component : DamnObject, ISerializable
    {
        public ISerializationObject SerializationObject => new SerializationComponent(this);

        public string GameObjectName
        {
            get => GameObject.Name;
            set => GameObject.Name = value;
        }

        public bool IsObjectActive
        {
            get => GameObject.IsObjectActive;
            set => GameObject.IsObjectActive = value;
        }
        
        public bool IsComponentEnabled
        {
            get => isComponentEnabled;
            set
            {
                if (isComponentEnabled == value)
                    return;
                isComponentEnabled = value;
                if (isComponentEnabled)
                    OnEnable();
                else
                    OnDisable();
            }
        }
        
        public GameObject GameObject { get; internal set; }

        public Transform Transform => GameObject.Transform;

        private bool isComponentEnabled = true;

        private bool isDestroyingComponent;

        protected Component() : base(PipelineTiming.Never) { }

        protected override void OnRegister()
        {
            Name = $"{GameObjectName}_{GetType()}";
            OnCreate();
            Application.OnNextFrameUpdate += OnStart;
        }
        
        protected internal virtual void OnEnable() { }
        protected internal virtual void OnCreate() { }
        protected internal virtual void OnStart() { }
        protected internal virtual void OnPreUpdate() { }
        protected internal virtual void OnUpdate() { }
        protected internal virtual void OnPostUpdate() { }
        protected internal virtual void OnTransformChanged() { }
        protected internal virtual void OnDisable() { }

        public override void Destroy()
        {
            if (isDestroyingComponent)
                base.Destroy();
        }

        protected override void OnDestroy()
        {
            GameObject = null;
        }
    }
}