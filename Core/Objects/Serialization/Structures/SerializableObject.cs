using System;
using Rietmon.Extensions;

namespace DamnEngine.Serialization
{
    [Serializable]
    public class SerializableObject : LowLevelDamnObject
    {
        public object Object { get; }
        
        public Type ObjectType { get; }

        public SerializableObject(object obj)
        {
            Object = obj;
            ObjectType = obj.GetType();
        }

        public object GetFieldValue(string name)
        {
            var fieldInfo = ObjectType.GetFieldByName(name);
            return fieldInfo != null ? fieldInfo.GetValue(Object) : null;
        }

        public void SetFieldValue(string name, object value)
        {
            var fieldInfo = ObjectType.GetFieldByName(name);
            if (fieldInfo != null)
                fieldInfo.SetValue(Object, value);
        }

        public object GetPropertyValue(string name)
        {
            var propertyInfo = ObjectType.GetPropertyByName(name);
            return propertyInfo != null ? propertyInfo.GetValue(Object) : null;
        }

        public void SetPropertyValue(string name, object value)
        {
            var propertyInfo = ObjectType.GetPropertyByName(name);
            if (propertyInfo != null)
                propertyInfo.SetValue(Object, value);
        }
    }
}