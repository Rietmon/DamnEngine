using OpenTK.Mathematics;

namespace DamnEngine
{
    public partial class Transform : Component, ITransformer
    {
        public Vector3 Position
        {
            get
            {
                if (!Parent)
                    return localPosition;
                return Parent.Position + localPosition;
            }
            set
            {
                SetPositionWithoutNotify(value);
                CallOnTransformChanged();
            }
        }
        public Vector3 LocalPosition
        {
            get => localPosition;
            set
            {
                if (!Parent)
                    Position = value;
                Position = Parent.LocalPosition + value;
            }
        }
        
        public Vector3 Rotation
        {
            get
            {
                if (!Parent)
                    return localRotation;
                return localRotation + Parent.Rotation;
            }
            set
            {
                SetRotationWithoutNotify(value);
                CallOnTransformChanged();
            }
        }
        public Vector3 LocalRotation
        {
            get => localRotation;
            set
            {
                if (!Parent)
                    Rotation = value;
                Rotation = Parent.LocalRotation + value; // TODO: REMAKE IT
            }
        }
        
        public Vector3 Scale
        {
            get => localScale;
            set
            {
                SetScaleWithoutNotify(value);
                CallOnTransformChanged();
            }
        }
        public Vector3 LocalScale
        {
            get => localScale;
            set
            {
                if (!Parent)
                    Scale = value;
                Scale = Parent.LocalScale * value;
            }
        }

        private Vector3 localPosition = Vector3.Zero;
        private Vector3 localRotation = Vector3.Zero;
        private Vector3 localScale = Vector3.One;

        internal void SetPositionWithoutNotify(Vector3 position)
        {
            localPosition = position;
            RecalculateChildesPosition();
        }

        internal void SetRotationWithoutNotify(Vector3 rotation)
        {
            localRotation = rotation;
            RecalculateChildesRotation();
        }

        internal void SetScaleWithoutNotify(Vector3 scale)
        {
            localScale = scale;
            RecalculateChildesScale();
        }

        public void SetTransform(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            SetPositionWithoutNotify(position);
            SetRotationWithoutNotify(rotation);
            SetScaleWithoutNotify(scale);
            CallOnTransformChanged();
        }
    }
}