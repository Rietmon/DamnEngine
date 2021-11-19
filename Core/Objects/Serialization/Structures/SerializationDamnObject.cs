using System;
using System.Collections.Generic;
using Tiny;

namespace DamnEngine.Serialization
{
    [Serializable]
    public class SerializationDamnObject : LowLevelDamnObject, ISerializationObject<DamnObject>
    {
        public string ObjectType { get; set; }

        public Dictionary<string, SerializableValue> SerializableFields { get; set; } = new();

        public SerializationDamnObject(DamnObject damnObject)
        {
            var type = damnObject.GetType();

            ObjectType = type.FullName;
            var fieldsToSerialize = type.GetAllSerializableFields();
            foreach (var fieldToSerialize in fieldsToSerialize)
            {
                SerializableFields.Add(fieldToSerialize.Name, new SerializableValue(fieldToSerialize.GetValue(damnObject)));
            }
        }

        public string Serialize() => this.Encode();

        public T Deserialize<T>() where T : DamnObject => (T)Deserialize();

        public DamnObject Deserialize()
        {
            var type = Type.GetType(ObjectType);
            if (type == null)
                return null;

            if (type.IsAssignableFrom(typeof(LowLevelDamnObject)))
                return null;
            
            var damnObject = (DamnObject)Activator.CreateInstance(type);
            var serializableObject = new SerializableObject(damnObject);
            foreach (var (key, value) in SerializableFields)
            {
                serializableObject.SetFieldValue(key, value.GetValue());
            }

            return damnObject;
        }

        public object this[string fieldName]
        {
            get => SerializableFields.TryGetValue(fieldName, out var field) ? field.GetValue() : null;
            set => SerializableFields.AddOrChange(fieldName, new SerializableValue(value));
        }
    }
}