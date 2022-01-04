#if ENABLE_STATISTICS
namespace DamnEngine
{
    public static class Statistics
    {
        public static uint TotalFacesDrawled { get; set; }
        
        public static uint TotalMeshesDrawled { get; set; }

        public static void Clear()
        {
            TotalFacesDrawled = 0;
            TotalMeshesDrawled = 0;
        }
    }
}
#endif