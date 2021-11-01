using Rietmon.Extensions;

namespace DamnEngine
{
    public abstract class DamnObject : LowLevelDamnObject
    {
        private static uint lastId;
        
        public string Name { get; set; }
        
        public uint Id { get; }

        protected DamnObject()
        {
            Id = lastId++;
        }
    }
}