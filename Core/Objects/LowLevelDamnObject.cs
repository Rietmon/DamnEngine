namespace DamnEngine
{
    public class LowLevelDamnObject
    {
        public virtual void Destroy()
        {
            OnDestroy();
        }
        
        protected virtual void OnDestroy() { }
        
        public static implicit operator bool(LowLevelDamnObject damnObject) => damnObject != null;
    }
}