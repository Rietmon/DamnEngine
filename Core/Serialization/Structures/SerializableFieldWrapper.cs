using System.Reflection;

namespace DamnEngine.Serialization
{
    public class SerializableFieldWrapper
    {
        public FieldInfo FieldInfo { get; }
        
        public PropertyInfo PropertyInfo { get; }
        
        public string Name => FieldInfo != null ? FieldInfo.Name : PropertyInfo.Name;

        public object GetValue(object owner) => FieldInfo != null ? FieldInfo.GetValue(owner) : PropertyInfo.GetValue(owner);
        
        public SerializableFieldWrapper(FieldInfo fieldInfo) => FieldInfo = fieldInfo;
        
        public SerializableFieldWrapper(PropertyInfo propertyInfo) => PropertyInfo = propertyInfo;

        public static implicit operator SerializableFieldWrapper(FieldInfo fieldInfo) => new(fieldInfo);
        
        public static implicit operator SerializableFieldWrapper(PropertyInfo propertyInfo) => new(propertyInfo);
    }
}