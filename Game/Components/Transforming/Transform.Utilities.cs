using OpenTK.Mathematics;

namespace DamnEngine
{
    public partial class Transform : Component
    {
        public Vector3 Forward => Rotation * Vector3.UnitZ;
        public Vector3 Backward => -Forward;
        public Vector3 Right => Rotation * Vector3.UnitX;
        public Vector3 Left => -Right;
        public Vector3 Up => Rotation * Vector3.UnitY;
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
        
        public static implicit operator GameObject(Transform transform) => transform.GameObject;
        public static implicit operator Transform(GameObject gameObject) => gameObject.Transform;
    }
}