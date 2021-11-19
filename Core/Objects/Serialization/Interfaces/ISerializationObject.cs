namespace DamnEngine.Serialization
{
    public interface ISerializationObject { }
    
    public interface ISerializationObject<T> : ISerializationObject
    {
        string Serialize();
        
        T Deserialize();
    }
}