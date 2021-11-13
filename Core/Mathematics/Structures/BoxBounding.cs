using OpenTK.Mathematics;

namespace DamnEngine
{
    public struct BoxBounding : IBounding
    {
        public Vector3 Center { get; set; }
        
        public float Size { get; set; }

        public BoxBounding(Vector3 center, float size)
        {
            Center = center;
            Size = size;
        }
        
        public bool IsOnForwardPlan(Plane plane)
        {
            var r = (Mathf.Abs(plane.Normal.X) + Mathf.Abs(plane.Normal.Y) + Mathf.Abs(plane.Normal.Z)) * Size;
            return -r < plane.GetSignedDistanceToPlane(Center);
        }

        public bool IsOnFrustum(Frustum frustum, ITransformer transformer)
        {
            var globalCenter = (transformer.ModelMatrix * new Vector4(Center, 1)).Xyz;

            var forward = transformer.Forward * Size;
            var up = transformer.Up * Size;
            var right = transformer.Left * Size;

            var newIi = Mathf.Abs(Vector3.Dot(new Vector3(1, 0, 0), right)) +
                        Mathf.Abs(Vector3.Dot(new Vector3(1, 0, 0), up)) +
                        Mathf.Abs(Vector3.Dot(new Vector3(1, 0, 0), forward));

            var newIj = Mathf.Abs(Vector3.Dot(new Vector3(0, 1, 0), right)) +
                        Mathf.Abs(Vector3.Dot(new Vector3(0, 1, 0), up)) +
                        Mathf.Abs(Vector3.Dot(new Vector3(0, 1, 0), forward));

            var newIk = Mathf.Abs(Vector3.Dot(new Vector3(0, 0, 1), right)) +
                        Mathf.Abs(Vector3.Dot(new Vector3(0, 0, 1), up)) +
                        Mathf.Abs(Vector3.Dot(new Vector3(0, 0, 1), forward));

            var box = new BoxBounding(globalCenter, Mathf.Max(newIi, newIj, newIk));
            
            return (box.IsOnForwardPlan(frustum.LeftFace) &&
                    box.IsOnForwardPlan(frustum.RightFace) &&
                    box.IsOnForwardPlan(frustum.TopFace) &&
                    box.IsOnForwardPlan(frustum.BottomFace) &&
                    box.IsOnForwardPlan(frustum.NearFace) &&
                    box.IsOnForwardPlan(frustum.FarFace));
        }
    }
}