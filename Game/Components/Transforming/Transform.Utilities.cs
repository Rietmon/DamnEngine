using OpenTK.Mathematics;

namespace DamnEngine
{
    public partial class Transform : Component
    {
        public Vector3 Forward => TransformForward(EulerRotation);
        public Vector3 Backward => -Forward;
        public Vector3 Right => TransformRight(EulerRotation);
        public Vector3 Left => -Right;
        public Vector3 Up => TransformUp(EulerRotation);
        public Vector3 Down => -Up;

        public Matrix4 TransformMatrix
        {
            get
            {
                var transform = Matrix4.Identity;
            
                if (!Parent)
                {
                    transform *= Matrix4.CreateFromQuaternion(Rotation);
                    transform *= Matrix4.CreateTranslation(Position);
                    transform *= Matrix4.CreateScale(Scale);
                }
                else
                {
                    transform *= Matrix4.CreateTranslation(Position);
                    transform *= Matrix4.CreateFromQuaternion(Rotation);
                    transform *= Matrix4.CreateScale(Scale);
                }

                return transform;
            }
        }

        private void CallOnTransformChanged() =>
            GameObject.ForEachComponent((component) => component.OnTransformChanged());

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
        public static Vector3 TransformUp(Vector3 rotation) => Vector3.Cross(TransformRight(rotation), TransformForward(rotation)).Normalized();
        public static Vector3 TransformDown(Vector3 rotation) => -TransformUp(rotation);
        
        public static implicit operator GameObject(Transform transform) => transform.GameObject;
        public static implicit operator Transform(GameObject gameObject) => gameObject.Transform;
    }
}