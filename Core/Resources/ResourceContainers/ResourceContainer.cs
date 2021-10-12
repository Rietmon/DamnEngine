namespace DamnEngine.Resources
{
    public class ResourceContainer<T> : DamnObject
    {
        public readonly T resource;

        public int referencesCount;

        public ResourceContainer(T resource) => this.resource = resource;

        public static implicit operator ResourceContainer<T>(T resource) => new ResourceContainer<T>(resource);
    }
}