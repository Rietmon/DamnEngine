using System;
using System.Collections.Generic;
using System.Reflection;

namespace DamnEngine.Serialization
{
    public class SerializationDamnObject : SerializationObject<DamnObject>
    {
        public Dictionary<string, SerializableValue> SerializableFields { get; } = new();
        
        public SerializationDamnObject(DamnObject damnObject) : base(damnObject)
        {
            var type = objectToSerialize.GetType();
            var fieldsToSerialize = GetAllSerializableFieldsAndProperties(type);
            foreach (var fieldToSerialize in fieldsToSerialize)
            {
                var value = fieldToSerialize.GetValue(damnObject);
                if (value is null or LowLevelDamnObject)
                    continue;
                
                this[fieldToSerialize.Name] = fieldToSerialize.GetValue(damnObject);
            }
        }

        public T Deserialize<T>() where T : DamnObject => (T)Deserialize();

        public override DamnObject Deserialize()
        {
            return null;
        }
        
        private static SerializableFieldWrapper[] GetAllSerializableFieldsAndProperties(Type type)
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default);
            var result = new List<SerializableFieldWrapper>();
            foreach (var field in fields)
            {
                if (field.IsPublic || field.GetCustomAttribute(typeof(SerializeFieldAttribute)) != null)
                {
                    result.Add(field);
                }
            }

            return result.ToArray();
        }

        protected object this[string fieldName]
        {
            get => SerializableFields.TryGetValue(fieldName, out var field) ? field.Value : null;
            set => SerializableFields.AddOrChange(fieldName, new SerializableValue(value));
        }
    }
}