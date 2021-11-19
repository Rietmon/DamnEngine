using System;
using Tiny;

namespace DamnEngine.Serialization
{
    [Serializable]
    public class SerializableValue : LowLevelDamnObject
    {
        public string Type { get; set; }
        
        public string Value { get; set; }

        public SerializableValue(object value)
        {
            Type = value.GetType().FullName;
            Value = value.Encode();
        }

        public object GetValue()
        {
            var type = System.Type.GetType(Type);
            return Value.Decode(type);
        }
    }
}