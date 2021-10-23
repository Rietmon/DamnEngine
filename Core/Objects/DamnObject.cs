namespace DamnEngine
{
    public abstract class DamnObject
    {
        public string Name { get; set; }
        
        private static bool Compare(DamnObject left, DamnObject right)
        {
            return left == right;
        }
        
        public virtual void Destroy()
        {
            OnDestroy();
        }
        
        protected virtual void OnDestroy() { }
        
        public static implicit operator bool(DamnObject damnObject) => !Compare(damnObject, null);
    }
}