using DamnEngine.Serialization;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public partial class Transform : Component
    {
        public Vector3 Position
        {
            get => position;
            set
            {
                position = value;
                CallOnTransformChanged();
            }
        }
        public Vector3 LocalPosition => Parent ? position - Parent.Position : position;

        public Vector3 Rotation
        {
            get => rotation;
            set
            {
                rotation = value;
                CallOnTransformChanged();
            }
        }
        
        public Vector3 Scale
        {
            get => scale;
            set
            {
                scale = value;
                CallOnTransformChanged();
            }
        }
        public Vector3 LocalScale => Parent ? scale * Parent.Scale : scale;

        [SerializeField] private Vector3 position = Vector3.Zero;
        [SerializeField] private Vector3 rotation = Vector3.Zero;
        [SerializeField] private Vector3 scale = Vector3.One;

        public void SetTransform(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            CallOnTransformChanged();
        }
    }
}