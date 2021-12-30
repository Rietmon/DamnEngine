using DamnEngine.Render;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public partial class Camera
    {
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
        
        public bool PointInFrustum(Vector3 point)
        {
            for (var i = 0; i < 6; i++)
            {
                if (Frustum[i].A * point.X + Frustum[i].B * point.Y + Frustum[i].C * point.Z + Frustum[i].D <= 0)
                    return false;
            }

            return true;
        }

        public bool SphereInFrustum(Vector3 point, float radius)
        {
            for (var i = 0; i < 6; i++)
            {
                if (Frustum[i].A * point.X + Frustum[i].B * point.Y + Frustum[i].C * point.Z + Frustum[i].D <= -radius)
                    return false;
            }

            return true;
        }

        public bool CubeInFrustum(Vector3 point, Vector3 size)
        {
            for (int i = 0; i < 6; i++)
            {
                if (Frustum[i].A * (point.X - size.X) + Frustum[i].B * (point.Y - size.Y) +
                    Frustum[i].C * (point.Z - size.Z) + Frustum[i].D > 0)
                    continue;
                if (Frustum[i].A * (point.X + size.X) + Frustum[i].B * (point.Y - size.Y) +
                    Frustum[i].C * (point.Z - size.Z) + Frustum[i].D > 0)
                    continue;
                if (Frustum[i].A * (point.X - size.X) + Frustum[i].B * (point.Y + size.Y) +
                    Frustum[i].C * (point.Z - size.Z) + Frustum[i].D > 0)
                    continue;
                if (Frustum[i].A * (point.X + size.X) + Frustum[i].B * (point.Y + size.Y) +
                    Frustum[i].C * (point.Z - size.Z) + Frustum[i].D > 0)
                    continue;
                if (Frustum[i].A * (point.X - size.X) + Frustum[i].B * (point.Y - size.Y) +
                    Frustum[i].C * (point.Z + size.Z) + Frustum[i].D > 0)
                    continue;
                if (Frustum[i].A * (point.X + size.X) + Frustum[i].B * (point.Y - size.Y) +
                    Frustum[i].C * (point.Z + size.Z) + Frustum[i].D > 0)
                    continue;
                if (Frustum[i].A * (point.X - size.X) + Frustum[i].B * (point.Y + size.Y) +
                    Frustum[i].C * (point.Z + size.Z) + Frustum[i].D > 0)
                    continue;
                if (Frustum[i].A * (point.X + size.X) + Frustum[i].B * (point.Y + size.Y) +
                    Frustum[i].C * (point.Z + size.Z) + Frustum[i].D > 0)
                    continue;

                return false;
            }

            return true;
        }

        public void SetData(float fov, float aspectRatio, float near, float far)
        {
            this.fov = fov;
            this.aspectRatio = aspectRatio;
            this.near = near;
            this.far = far;
            
            UpdateProjectionMatrix();
            UpdateFrustum();
        }

        private void UpdateProjectionMatrix() => Rendering.ProjectionMatrix =
            Matrix4.CreatePerspectiveFieldOfView(Fov, AspectRatio, Near, Far);

        private void UpdateViewMatrix() => Rendering.ViewMatrix =
            Matrix4.LookAt(Transform.Position, Transform.Position + Transform.Forward, Transform.Up);
        
        private void UpdateFrustum()
        {
            var projectionMatrix = Rendering.ProjectionMatrix;
                var viewMatrix = Rendering.ViewMatrix;
                var clip = new float[16];

                clip[0] = viewMatrix.Get(0) * projectionMatrix.Get(0) + viewMatrix.Get(1) * projectionMatrix.Get(4) +
                          viewMatrix.Get(2) * projectionMatrix.Get(8) + viewMatrix.Get(3) * projectionMatrix.Get(12);
                clip[1] = viewMatrix.Get(0) * projectionMatrix.Get(1) + viewMatrix.Get(1) * projectionMatrix.Get(5) +
                          viewMatrix.Get(2) * projectionMatrix.Get(9) + viewMatrix.Get(3) * projectionMatrix.Get(13);
                clip[2] = viewMatrix.Get(0) * projectionMatrix.Get(2) + viewMatrix.Get(1) * projectionMatrix.Get(6) +
                          viewMatrix.Get(2) * projectionMatrix.Get(10) + viewMatrix.Get(3) * projectionMatrix.Get(14);
                clip[3] = viewMatrix.Get(0) * projectionMatrix.Get(3) + viewMatrix.Get(1) * projectionMatrix.Get(7) +
                          viewMatrix.Get(2) * projectionMatrix.Get(11) + viewMatrix.Get(3) * projectionMatrix.Get(1);
                clip[4] = viewMatrix.Get(4) * projectionMatrix.Get(0) + viewMatrix.Get(5) * projectionMatrix.Get(4) +
                          viewMatrix.Get(6) * projectionMatrix.Get(8) + viewMatrix.Get(7) * projectionMatrix.Get(12);
                clip[5] = viewMatrix.Get(4) * projectionMatrix.Get(1) + viewMatrix.Get(5) * projectionMatrix.Get(5) +
                          viewMatrix.Get(6) * projectionMatrix.Get(9) + viewMatrix.Get(7) * projectionMatrix.Get(13);
                clip[6] = viewMatrix.Get(4) * projectionMatrix.Get(2) + viewMatrix.Get(5) * projectionMatrix.Get(6) +
                          viewMatrix.Get(6) * projectionMatrix.Get(10) + viewMatrix.Get(7) * projectionMatrix.Get(14);
                clip[7] = viewMatrix.Get(4) * projectionMatrix.Get(3) + viewMatrix.Get(5) * projectionMatrix.Get(7) +
                          viewMatrix.Get(6) * projectionMatrix.Get(11) + viewMatrix.Get(7) * projectionMatrix.Get(1);
                clip[8] = viewMatrix.Get(8) * projectionMatrix.Get(0) + viewMatrix.Get(9) * projectionMatrix.Get(4) +
                          viewMatrix.Get(10) * projectionMatrix.Get(8) + viewMatrix.Get(11) * projectionMatrix.Get(12);
                clip[9] = viewMatrix.Get(8) * projectionMatrix.Get(1) + viewMatrix.Get(9) * projectionMatrix.Get(5) +
                          viewMatrix.Get(10) * projectionMatrix.Get(9) + viewMatrix.Get(11) * projectionMatrix.Get(13);
                clip[10] = viewMatrix.Get(8) * projectionMatrix.Get(2) + viewMatrix.Get(9) * projectionMatrix.Get(6) +
                           viewMatrix.Get(10) * projectionMatrix.Get(10) + viewMatrix.Get(11) * projectionMatrix.Get(14);
                clip[11] = viewMatrix.Get(8) * projectionMatrix.Get(3) + viewMatrix.Get(9) * projectionMatrix.Get(7) +
                           viewMatrix.Get(10) * projectionMatrix.Get(11) + viewMatrix.Get(11) * projectionMatrix.Get(15);
                clip[12] = viewMatrix.Get(12) * projectionMatrix.Get(0) + viewMatrix.Get(13) * projectionMatrix.Get(4) +
                           viewMatrix.Get(14) * projectionMatrix.Get(8) + viewMatrix.Get(15) * projectionMatrix.Get(12);
                clip[13] = viewMatrix.Get(12) * projectionMatrix.Get(1) + viewMatrix.Get(13) * projectionMatrix.Get(5) +
                           viewMatrix.Get(14) * projectionMatrix.Get(9) + viewMatrix.Get(15) * projectionMatrix.Get(13);
                clip[14] = viewMatrix.Get(12) * projectionMatrix.Get(2) + viewMatrix.Get(13) * projectionMatrix.Get(6) +
                           viewMatrix.Get(14) * projectionMatrix.Get(10) + viewMatrix.Get(15) * projectionMatrix.Get(14);
                clip[15] = viewMatrix.Get(12) * projectionMatrix.Get(3) + viewMatrix.Get(13) * projectionMatrix.Get(7) +
                           viewMatrix.Get(14) * projectionMatrix.Get(11) + viewMatrix.Get(15) * projectionMatrix.Get(15);

                Frustum = new Frustum
                {
                    Right = new FrustumSide(clip[3] - clip[0], clip[7] - clip[4], clip[11] - clip[8], clip[15] - clip[12]),
                    Left = new FrustumSide(clip[3] + clip[0], clip[7] + clip[4], clip[11] + clip[8], clip[15] + clip[12]),
                    Bottom = new FrustumSide(clip[3] + clip[1], clip[7] + clip[5], clip[11] + clip[9], clip[15] + clip[13]),
                    Top = new FrustumSide(clip[3] - clip[1], clip[7] - clip[5], clip[11] - clip[9], clip[15] - clip[13]),
                    Back = new FrustumSide(clip[3] - clip[2], clip[7] - clip[6], clip[11] - clip[10], clip[15] - clip[14]),
                    Front = new FrustumSide(clip[3] + clip[2], clip[7] + clip[6], clip[11] + clip[10], clip[15] + clip[14])
                };
        }
    }
}