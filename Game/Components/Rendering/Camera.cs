using System;
using DamnEngine.Render;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public class Camera : Component
    {
        public static Camera Main { get; private set; }

        public float Fov
        {
            get => fov;
            set
            {
                fov = value;
                UpdateProjectionMatrix();
            }
        }
        public float AspectRatio
        {
            get => aspectRatio;
            set
            {
                aspectRatio = value;
                UpdateProjectionMatrix();
            }
        }
        public float Near
        {
            get => near;
            set
            {
                near = value; 
                UpdateProjectionMatrix();
            }
        }
        public float Far
        {
            get => far;
            set
            {
                far = value; 
                UpdateProjectionMatrix();
            }
        }

        public Frustum Frustum
        {
            get
            {
                var halfVerticalSide = Far * Mathf.Tan(Fov / 2);
                var halfHorizontalSide = halfVerticalSide * AspectRatio;
                var forwardMultiplyFar = Transform.Forward * Far;

                return new Frustum
                {
                    NearFace = new Plane(Transform.Position + Transform.Forward * Near,
                        Transform.Forward),
                    FarFace = new Plane(Transform.Position + forwardMultiplyFar,
                        Transform.Backward),

                    RightFace = new Plane(Transform.Position,
                        Vector3.Cross(Transform.Up, forwardMultiplyFar + Transform.Right * halfHorizontalSide)),
                    LeftFace = new Plane(Transform.Position,
                        Vector3.Cross(forwardMultiplyFar - Transform.Right * halfHorizontalSide, Transform.Up)),

                    TopFace = new Plane(Transform.Position,
                        Vector3.Cross(Transform.Right, forwardMultiplyFar - Transform.Up * halfVerticalSide)),
                    BottomFace = new Plane(Transform.Position,
                        Vector3.Cross(forwardMultiplyFar + Transform.Up * halfVerticalSide, Transform.Right))
                };
            }
        }

        private float fov;
        private float aspectRatio;
        private float near;
        private float far;

        protected internal override void OnCreate()
        {
            Main = this;
            
            fov = 0.7853982f;
            var windowSize = Application.Window.Bounds.Max;
            aspectRatio = (float)windowSize.X / (float)windowSize.Y;
            near = 0.01f;
            far = 2000f;
            
            UpdateProjectionMatrix();
        }

        protected internal override void OnTransformChanged()
        {
            UpdateViewMatrix();
        }

        private void UpdateProjectionMatrix() => Rendering.ProjectionMatrix = 
            Matrix4.CreatePerspectiveFieldOfView(Fov, AspectRatio, Near, Far);

        private void UpdateViewMatrix() => Rendering.ViewMatrix =
            Matrix4.LookAt(Transform.Position, Transform.Position + Transform.Forward, Transform.Up);
    }
}