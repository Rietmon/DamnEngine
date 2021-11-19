namespace DamnEngine.Serialization
{
    public interface ISerializationObject 
    {
        string Serialize();
    }
    
    public interface ISerializationObject<T> : ISerializationObject
    {
        T Deserialize();
    }
}