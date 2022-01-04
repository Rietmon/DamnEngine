namespace DamnEngine
{
    public readonly struct SubMeshDescriptor
    {
        public int StartIndex { get; }
        
        public int EndIndex { get; }

        public SubMeshDescriptor(int startIndex, int endIndex)
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
        }
    }
}