namespace DamnEngine
{
    public abstract class ResourceContainer : LowLevelDamnObject
    {
        public abstract object Resource { get; }

        public int referencesCount;

        public T GetResource<T>() => (T)Resource;
    }
    
    public class ResourceContainer<T> : ResourceContainer
    {
        public override object Resource => resource;
        
        private readonly T resource;

        public ResourceContainer(T resource) => this.resource = resource;

        public static implicit operator ResourceContainer<T>(T resource) => new(resource);
    }
}