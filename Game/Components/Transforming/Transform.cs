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
                return localRotation * Parent.Rotation;
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
        
        public Vector3 EulerAngles
        {
            get
            {
                var eulerAngles = localRotation.ToEulerAngles() * Mathf.Rad2Deg;
                for (var i = 0; i < 3; i++)
                {
                    var angle = eulerAngles[i];
                    if (angle < 0)
                    {
                        angle = 180 + (180 - Mathf.Abs(angle));
                        eulerAngles[i] = angle;
                    }
                }

                return eulerAngles;
            }
            set
            {
                for (var i = 0; i < 3; i++)
                {
                    var angle = value[i];
                    if (angle > 180)
                    {
                        angle = -180 + (angle - 180);
                        value[i] = angle;
                    }
                }
                Rotation = Quaternion.FromEulerAngles(value * Mathf.Deg2Rad);
                var euler = Rotation.ToEulerAngles() * Mathf.Rad2Deg;
                Application.Window.Title = $"X: {euler.X:##.000} Y: {euler.Y:##.000} Z: {euler.Z:##.000}";
                GameObject.ForEachComponent((component) => component.OnTransformChanged());
            }
        }
        
        private Vector3 localPosition = Vector3.Zero;
        private Quaternion localRotation = Quaternion.Identity;
        private Vector3 localScale = Vector3.One;
    }
}