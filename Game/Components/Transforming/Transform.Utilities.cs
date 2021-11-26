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

        public Matrix4 ModelMatrix
        {
            get
            {
                var model = Matrix4.Identity;

                if (!Parent)
                {
                    model *= Matrix4.CreateTranslation(Position);
                    model *= Matrix4Extensions.CreateRotation(RotationInRadians);
                    model *= Matrix4.CreateScale(Scale);
                }
                else
                {
                    model *= Matrix4.CreateTranslation(LocalPosition);
                    model *= Matrix4Extensions.CreateRotation(Parent.RotationInRadians);
                    model *= Matrix4.CreateTranslation(-LocalPosition);
                    
                    model *= Matrix4.CreateTranslation(Position);
                    model *= Matrix4Extensions.CreateRotation(RotationInRadians);
                    model *= Matrix4.CreateScale(Scale);
                }

                return model;
            }
        }
        
        public static Vector3 TransformForward(Vector3 rotation)
        {
            var (x, y, _) = rotation * Mathf.Deg2Rad;
            return new Vector3(Mathf.Sin(y) * Mathf.Cos(x),
                Mathf.Sin(x),
                Mathf.Cos(y) * Mathf.Cos(x)).Normalized();
        }
        public static Vector3 TransformBackward(Vector3 rotation) => -TransformForward(rotation);
        public static Vector3 TransformRight(Vector3 rotation) => Vector3.Cross(TransformForward(rotation), Vector3.UnitY).Normalized();
        public static Vector3 TransformLeft(Vector3 rotation) => -TransformRight(rotation);
        public static Vector3 TransformUp(Vector3 rotation) => Vector3.Cross(TransformRight(rotation), TransformForward(rotation)).Normalized(); // TODO: Calculate here Z rotation!!!
        public static Vector3 TransformDown(Vector3 rotation) => -TransformUp(rotation);

        private void CallOnTransformChanged() =>
            GameObject.ForEachComponent((component) => component.OnTransformChanged());
        
        public static implicit operator GameObject(Transform transform) => transform.GameObject;
        public static implicit operator Transform(GameObject gameObject) => gameObject.Transform;
    }
}