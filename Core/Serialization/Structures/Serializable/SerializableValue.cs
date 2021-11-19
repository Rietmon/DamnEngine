using System;
using Tiny;

namespace DamnEngine.Serialization
{
    [Serializable]
    public class SerializableValue : LowLevelDamnObject
    {
        public string Type { get; set; }
        
        public object Value { get; set; }

        public SerializableValue(object value)
        {
            Type = value.GetType().FullName;
            Value = value;
        }

        public SerializableValue(string type, string value)
        {
            Type = type;
            var systemType = System.Type.GetType(type);
            if (systemType != null)
                Value = value.Decode(systemType);
        }
    }
}