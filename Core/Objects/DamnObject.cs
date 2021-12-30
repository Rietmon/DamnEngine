using DamnEngine.Serialization;

namespace DamnEngine
{
    public abstract class DamnObject : LowLevelDamnObject, ISerializable
    {
        private static uint lastRuntimeId;

        ISerializationObject ISerializable.SerializationObject => new SerializationDamnObject(this);
        
        public virtual string Name { get; set; }
        
        public uint RuntimeId { get; }

        protected DamnObject()
        {
            RuntimeId = lastRuntimeId++;
        }
    }
}