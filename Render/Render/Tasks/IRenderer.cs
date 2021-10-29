namespace DamnEngine.Render
{
    public interface IRenderer
    {
        void OnPreRendering();
        void OnRendering();
        void OnPostRendering();
    }
}