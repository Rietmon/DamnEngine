using DamnEngine.Serialization;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public partial class Transform : Component
    {
        public Vector3 Position
        {
            get => Parent ? Parent.Position + LocalPosition : localPosition;
            set
            {
                localPosition = Parent ? Parent.Position + value : value;
                CallOnTransformChanged();
            }
        }
        public Vector3 LocalPosition
        {
            get => localPosition;
            set
            {
                localPosition = value;
                CallOnTransformChanged();
            }
        }

        public Vector3 Rotation => Parent ? ModelMatrix.GetTRSRotation() : localRotation;
        public Vector3 LocalRotation
        {
            get => localRotation;
            set
            {
                localRotation = value;
                CallOnTransformChanged();
            }
        }
        
        public Vector3 Scale => Parent ? localScale * Parent.Scale : localScale;
        public Vector3 LocalScale
        {
            get => localScale;
            set
            {
                localScale = value;
                CallOnTransformChanged();
            }
        }

        [SerializeField] private Vector3 localPosition = Vector3.Zero;
        [SerializeField] private Vector3 localRotation = Vector3.Zero;
        [SerializeField] private Vector3 localScale = Vector3.One;
    }
}