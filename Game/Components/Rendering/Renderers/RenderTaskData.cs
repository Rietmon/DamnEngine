namespace DamnEngine.Render
{
    public class RenderTaskData : LowLevelDamnObject
    {
        public float[] Data { get; }
        public int[] Indices { get; }
        public Material Material { get; }
        public uint OwnerId { get; }

        public RenderTaskData(float[] data, int[] indices, Material material, uint ownerId)
        {
            Data = data;
            Indices = indices;
            Material = material;
            OwnerId = ownerId;
        }
    }
}