using System;
using System.Collections.Generic;
using System.Reflection;
using Tiny;

namespace DamnEngine.Serialization
{
    public static class SerializationManager
    {
        public static string Serialize(DamnObject damnObject)
        {
            var serializationDamnObject = new SerializationDamnObject(damnObject);
            return serializationDamnObject.Encode();
        }

        public static DamnObject Deserialize(SerializationDamnObject serializationDamnObject)
        {
            return serializationDamnObject.Deserialize();
        }
        
        internal static FieldInfo[] GetAllSerializableFields(this Type type)
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default);
            var result = new List<FieldInfo>();
            foreach (var field in fields)
            {
                if (field.IsPublic || field.GetCustomAttribute(typeof(SerializeFieldAttribute)) != null)
                {
                    result.Add(field);
                }
            }

            return result.ToArray();
        }
    }
}