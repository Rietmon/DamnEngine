namespace DamnEngine.Serialization
{
    public interface ISerializable
    {
        ISerializationObject SerializationObject { get; }
    }
}