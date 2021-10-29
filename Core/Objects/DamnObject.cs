using Rietmon.Extensions;

namespace DamnEngine
{
    public abstract class DamnObject
    {
        public string Name { get; set; }

        private bool isDestroyed;

        private static bool Compare(DamnObject left, DamnObject right)
        {
            return left == right;
        }
        
        public virtual void Destroy()
        {
            isDestroyed = true;
            OnDestroy();
        }
        
        protected virtual void OnDestroy() { }

        ~DamnObject()
        {
            if (!isDestroyed)
                Destroy();
        }
        
        public static implicit operator bool(DamnObject damnObject) => !Compare(damnObject, null);
    }
}