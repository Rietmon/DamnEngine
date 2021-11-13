namespace DamnEngine
{
    public struct Frustum
    {
        public Plane TopFace { get; set; }
        public Plane BottomFace { get; set; }
        
        public Plane RightFace { get; set; }
        public Plane LeftFace { get; set; }
        
        public Plane FarFace { get; set; }
        public Plane NearFace { get; set; }
    }
}