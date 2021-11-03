using OpenTK.Mathematics;

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

        public Vector3 Forward => TransformForward(Rotation);
        public Vector3 Backward => -Forward;
        public Vector3 Right => TransformRight(Rotation);
        public Vector3 Left => -Right;
        public Vector3 Up => TransformUp(Rotation);
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

        public static Vector3 TransformForward(Vector3 rotation)
        {
            var rotationInRadians =  rotation * Mathf.Deg2Rad;
            return new Vector3(Mathf.Sin(rotationInRadians.Y) * Mathf.Cos(rotationInRadians.X),
                Mathf.Sin(rotationInRadians.X),
                Mathf.Cos(rotationInRadians.Y) * Mathf.Cos(rotationInRadians.X)).Normalized();
        }
        public static Vector3 TransformBackward(Vector3 rotation) => -TransformForward(rotation);
        public static Vector3 TransformRight(Vector3 rotation) => Vector3.Cross(TransformForward(rotation), Vector3.UnitY).Normalized();
        public static Vector3 TransformLeft(Vector3 rotation) => -TransformRight(rotation);
        public static Vector3 TransformUp(Vector3 rotation) => Vector3.Cross(TransformRight(rotation), TransformForward(rotation)).Normalized();
        public static Vector3 TransformDown(Vector3 rotation) => -TransformUp(rotation);
    }
}