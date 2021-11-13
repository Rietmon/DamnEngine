using System.Collections.Generic;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public partial class Transform
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
                localPosition = value;
                CallOnTransformChanged();
                RecalculateChildesPosition();
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
                localRotation = value;
                CallOnTransformChanged();
            }
        }
        public Vector3 Scale
        {
            get => localScale;
            set
            {
                localScale = value;
                CallOnTransformChanged();
            }
        }

        private Vector3 localPosition = Vector3.Zero;
        private Vector3 localRotation = Vector3.Zero;
        private Vector3 localScale = Vector3.One;
    }
}