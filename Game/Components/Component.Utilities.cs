namespace DamnEngine
{
    public partial class Component
    {
        public T AddComponent<T>() where T : Component, new() => GameObject.AddComponent<T>();
        
        public T GetComponent<T>() => GameObject.GetComponent<T>();
        
        public T[] GetComponents<T>() => GameObject.GetComponents<T>();

        public bool TryGetComponent<T>(out T component) => GameObject.TryGetComponent(out component);
        
        public void RemoveComponent<T>() => GameObject.RemoveComponent<T>();
        
        public void RemoveComponent<T>(T component) where T : Component => GameObject.RemoveComponent(component);

        public void DestroyGameObject() => GameObject.Destroy();

        internal void Internal_DestroyFromGameObject() => OnDestroy();
    }
}