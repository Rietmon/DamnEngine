using DamnEngine.Render;
using OpenTK.Mathematics;

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
            
            fov = 0.7853982f;
            var windowSize = Application.Window.Bounds.Max;
            aspectRatio = (float)windowSize.X / (float)windowSize.Y;
            near = 0.01f;
            far = 2000f;
            
            UpdateProjectionMatrix();

            Rendering.OnPreRendering += OnPreRendering;
        }
        
        private void OnPreRendering()
        {
            UpdateViewMatrix();
        }

        private void UpdateProjectionMatrix() => Rendering.ProjectionMatrix = 
            Matrix4.CreatePerspectiveFieldOfView(Fov, AspectRatio, Near, Far);

        private void UpdateViewMatrix() => Rendering.ViewMatrix =
            Matrix4.LookAt(Transform.Position, Transform.Position + Transform.Forward, Transform.Up);

    }
}