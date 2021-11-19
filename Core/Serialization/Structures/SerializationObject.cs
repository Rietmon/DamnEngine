using System;
using System.Collections.Generic;
using Tiny;

namespace DamnEngine.Serialization
{
    [Serializable]
    public abstract class SerializationObject<T>: LowLevelDamnObject, ISerializationObject
    {
        public string ObjectType { get; set; }
        
        [NonSerialized] protected T objectToSerialize;

        public SerializationObject(T gameObject)
        {
            objectToSerialize = gameObject;
            ObjectType = gameObject.GetType().FullName;
        }

        public string Serialize() => this.Encode(true);

        public abstract T Deserialize();
    }
}