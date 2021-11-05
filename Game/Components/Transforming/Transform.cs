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
        public Quaternion Rotation
        {
            get
            {
                if (!Parent)
                    return localRotation;
                return Parent.Rotation + localRotation;
            }
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
        
        public Vector3 EulerRotation
        {
            get => localRotation.ToEuler();
            set
            {
                Rotation = new Quaternion().FromEuler(value);
                GameObject.ForEachComponent((component) => component.OnTransformChanged());
            }
        }
        
        private Vector3 localPosition = Vector3.Zero;
        private Quaternion localRotation = Quaternion.Identity;
        private Vector3 localScale = Vector3.One;
    }
}