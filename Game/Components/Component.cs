// ReSharper disable VirtualMemberCallInConstructor
namespace DamnEngine
{
    public abstract class Component : DamnObject
    {
        public GameObject GameObject { get; internal set; }

        public Transform Transform => GameObject.Transform;

        protected Component()
        {
            OnCreate();

            Application.OnNextFrameUpdate += OnStart;
        }

        public T AddComponent<T>() where T : Component => GameObject.AddComponent<T>();
        
        public T GetComponent<T>() => GameObject.GetComponent<T>();
        
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