using DamnEngine.Render;
using OpenTK;

namespace DamnEngine
{
    public class Camera : Component
    {
        public static Camera Instance { get; private set; }

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

        private float fov;
        private float aspectRatio;
        private float near;
        private float far;

        protected internal override void OnCreate()
        {
            Instance = this;

            Rendering.OnPreRendering += OnWillRenderingFrame;
        }

        public void SetData(float fov, float aspectRatio, float near, float far)
        {
            this.fov = fov;
            this.aspectRatio = aspectRatio;
            this.near = near;
            this.far = far;
            UpdateProjectionMatrix();
        }

        private void OnWillRenderingFrame()
        {
            UpdateViewMatrix();
        }

        private void UpdateProjectionMatrix() => Rendering.ProjectionMatrix = 
            Matrix4.CreatePerspectiveFieldOfView(Fov, AspectRatio, Near, Far);

        private void UpdateViewMatrix() => Rendering.ViewMatrix =
            Matrix4.LookAt(Transform.Position, Transform.Position + Transform.Forward, Transform.Up);
    }
}