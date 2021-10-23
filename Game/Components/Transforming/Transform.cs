using System;
using OpenTK;

namespace DamnEngine
{
    public class Transform : Component
    {
        public Vector3 Position
        {
            get => position;
            set
            {
                position = value;
                GameObject.ForEachComponent((component) => component.OnTransformChanged());
            }
        }
        public Vector3 Rotation
        {
            get => rotation;
            set
            {
                rotation = value;
                GameObject.ForEachComponent((component) => component.OnTransformChanged());
            }
        }
        public Vector3 Scale
        {
            get => scale;
            set
            {
                scale = value;
                GameObject.ForEachComponent((component) => component.OnTransformChanged());
            }
        }
        
        public Vector3 Forward
        {
            get
            {
                var rotationInRadians =  Rotation * Mathf.Deg2Rad;
                return new Vector3(Mathf.Sin(rotationInRadians.Y) * Mathf.Cos(rotationInRadians.X),
                    Mathf.Sin(rotationInRadians.X),
                    Mathf.Cos(rotationInRadians.Y) * Mathf.Cos(rotationInRadians.X)).Normalized();
            }
        }
        public Vector3 Back => -Forward;
        public Vector3 Right => Vector3.Cross(Forward, -Vector3.UnitY).Normalized();
        public Vector3 Left => -Right;
        public Vector3 Up => Vector3.Cross(Right, Forward).Normalized();
        public Vector3 Down => -Up;

        public Matrix4 TransformMatrix
        {
            get
            {
                var transform = Matrix4.Identity;
            
                transform *= Matrix4.CreateTranslation(Position);
            
                transform *= Matrix4.CreateRotationX(Rotation.X);
                transform *= Matrix4.CreateRotationY(Rotation.Y);
                transform *= Matrix4.CreateRotationZ(Rotation.Z);
                transform *= Matrix4.CreateScale(Scale);

                return transform;
            }
        }
        
        private Vector3 position = Vector3.Zero;
        private Vector3 rotation = Vector3.Zero;
        private Vector3 scale = Vector3.One;
    }
}