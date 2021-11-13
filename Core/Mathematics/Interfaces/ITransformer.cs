using OpenTK.Mathematics;

namespace DamnEngine
{
    public interface ITransformer
    {
        Vector3 Position { get; set; }
        Vector3 LocalPosition { get; set; }
        
        Vector3 Rotation { get; set; }
        Vector3 LocalRotation { get; set; }
        
        Vector3 Scale { get; set; }
        Vector3 LocalScale { get; set; }
        
        Vector3 Forward { get; }
        Vector3 Backward { get; }
        
        Vector3 Up { get; }
        Vector3 Down { get; }
        
        Vector3 Right { get; }
        Vector3 Left { get; }
        
        Matrix4 ModelMatrix { get; }
    }
}