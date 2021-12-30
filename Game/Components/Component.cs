using DamnEngine.Serialization;

namespace DamnEngine
{
    public abstract class Component : DamnObject, ISerializable
    {
        public ISerializationObject SerializationObject => new SerializationComponent(this);

        public override string Name
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

        protected Component()
        {
            Application.OnNextFrameUpdate += OnStart;
        }

        public T AddComponent<T>() where T : Component, new() => GameObject.AddComponent<T>();
        
        public T GetComponent<T>() => GameObject.GetComponent<T>();
        
        public T[] GetComponents<T>() => GameObject.GetComponents<T>();

        public bool TryGetComponent<T>(out T component) => GameObject.TryGetComponent(out component);
        
        public void RemoveComponent<T>() => GameObject.RemoveComponent<T>();
        
        public void RemoveComponent<T>(T component) where T : Component => GameObject.RemoveComponent(component);
        
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
            GameObject?.RemoveComponent(this);
            
            base.Destroy();
        }
    }
}