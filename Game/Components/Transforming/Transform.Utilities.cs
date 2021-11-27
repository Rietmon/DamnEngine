using OpenTK.Mathematics;

namespace DamnEngine
{
    public partial class Transform
    {
        public Vector3 Forward => TransformForward(Rotation);
        public Vector3 Backward => -Forward;
        public Vector3 Right => TransformRight(Rotation);
        public Vector3 Left => -Right;
        public Vector3 Up => TransformUp(Rotation);
        public Vector3 Down => -Up;

        public Vector3 RotationInRadians => Rotation * Mathf.Deg2Rad;
        public Vector3 LocalRotationInRadians => LocalRotation * Mathf.Deg2Rad;

        public Matrix4 WorldMatrix
        {
            get
            {
                var matrix = Matrix4.Identity;
                matrix *= Matrix4Extensions.CreateRotation(LocalRotationInRadians);
                matrix *= Matrix4.CreateTranslation(Position);
                matrix *= Matrix4.CreateScale(Scale);
                return matrix;
            }
        }

        public Matrix4 LocalMatrix
        {
            get
            {
                var matrix = Matrix4.Identity;
                matrix *= Matrix4Extensions.CreateRotation(LocalRotationInRadians);
                matrix *= Matrix4.CreateTranslation(LocalPosition);
                matrix *= Matrix4Extensions.CreateRotation(Parent.LocalRotationInRadians);
                matrix *= Matrix4.CreateTranslation(-LocalPosition);
                matrix *= Matrix4.CreateScale(LocalScale);
                return matrix;
            }
        }

        public Matrix4 LocalToWorldMatrix
        {
            get
            {
                var matrix = Matrix4.Identity;
                matrix *= Matrix4Extensions.CreateRotation(LocalRotationInRadians);
                matrix *= Matrix4.CreateTranslation(LocalPosition);
                matrix *= Matrix4Extensions.CreateRotation(Parent.LocalRotationInRadians);
                matrix *= Matrix4.CreateTranslation(-LocalPosition);
                matrix *= Matrix4.CreateTranslation(Position);
                matrix *= Matrix4.CreateScale(LocalScale);
                return matrix;
            }
        }

        public Matrix4 ModelMatrix => Parent ? LocalToWorldMatrix : WorldMatrix;
        
        public void SetTransform(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            worldPosition = position;
            localRotation = rotation;
            localScale = scale;
            CallOnTransformChanged();
        }
        
        public static Vector3 TransformForward(Vector3 rotation)
        {
            var (x, y, _) = rotation * Mathf.Deg2Rad;
            return new Vector3(Mathf.Sin(y) * Mathf.Cos(x),
                Mathf.Sin(x),
                Mathf.Cos(y) * Mathf.Cos(x)).Normalized();
        }

        private void CallOnTransformChanged() =>
            GameObject.ForEachComponent((component) => component.OnTransformChanged());
        
        public static Vector3 TransformBackward(Vector3 rotation) => -TransformForward(rotation);
        public static Vector3 TransformRight(Vector3 rotation) => Vector3.Cross(TransformForward(rotation), Vector3.UnitY).Normalized();
        public static Vector3 TransformLeft(Vector3 rotation) => -TransformRight(rotation);
        public static Vector3 TransformUp(Vector3 rotation) => Vector3.Cross(TransformRight(rotation), TransformForward(rotation)).Normalized(); // TODO: Calculate here Z rotation!!!
        public static Vector3 TransformDown(Vector3 rotation) => -TransformUp(rotation);
        
        public static implicit operator GameObject(Transform transform) => transform.GameObject;
        public static implicit operator Transform(GameObject gameObject) => gameObject.Transform;
    }
}