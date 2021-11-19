using System.Collections.Generic;
using System.Linq;
using Rietmon.Extensions;

namespace DamnEngine.Serialization
{
    public class SerializationGameObject : SerializationObject<GameObject>
    {
        public string Name { get; set; }
        
        public bool IsObjectActive { get; set; }
        
        public SerializationComponent[] Components { get; set; }

        public SerializationGameObject(GameObject gameObject) : base(gameObject)
        {
            Name = gameObject.Name;
            IsObjectActive = gameObject.IsObjectActive;
            Components = gameObject.components.SmartCast((component) => (SerializationComponent)component.SerializationObject).ToArray();
        }

        public override GameObject Deserialize()
        {
            throw new System.NotImplementedException();
        }
    }
}