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
                GameObject.ForEachComponent((component) => component.OnTransformChanged());
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
            get => localRotation;
            set
            {
                localRotation = value;
                GameObject.ForEachComponent((component) => component.OnTransformChanged());
            }
        }
        public Vector3 Scale
        {
            get => localScale;
            set
            {
                localScale = value;
                GameObject.ForEachComponent((component) => component.OnTransformChanged());
            }
        }
        
        private Vector3 localPosition = Vector3.Zero;
        private Vector3 localRotation = Vector3.Zero;
        private Vector3 localScale = Vector3.One;
    }
}