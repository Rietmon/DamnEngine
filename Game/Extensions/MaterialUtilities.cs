using DamnEngine.Render;

namespace DamnEngine
{
    public static class MaterialUtilities
    {
        public static Material GetOrInvalid(this Material[] materials, int index)
        {
            if (materials.Length > index)
                return materials[index];
            
            Debug.LogWarning($"[{nameof(MaterialUtilities)}] ({nameof(GetOrInvalid)}) Got invalid material!");
            return Material.InvalidMaterial;
        }
    }
}