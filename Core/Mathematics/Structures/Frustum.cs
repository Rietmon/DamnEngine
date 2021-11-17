namespace DamnEngine
{
    public struct Frustum
    {
        public FrustumSide Right { get; set; }
        public FrustumSide Left { get; set; }
        public FrustumSide Bottom { get; set; }
        public FrustumSide Top { get; set; }
        public FrustumSide Front { get; set; }
        public FrustumSide Back { get; set; }

        public FrustumSide this[int index] =>
            index switch
            {
                0 => Right,
                1 => Left,
                2 => Bottom,
                3 => Top,
                4 => Front,
                5 => Back,
                _ => default
            };
    }
}