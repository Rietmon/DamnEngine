using DamnEngine.Render;
using OpenTK;

namespace DamnEngine
{
    public class Renderer : Component
    {
        protected RenderTask RenderTask { get; set; }

        protected void CreateRenderTask(Vector3[] vertices, Vector2[] uv, int[] indices, Material material)
        {
            if (RenderTask)
                RenderTask.Destroy();

            RenderTask = new RenderTask(vertices, uv, indices, material);
            Rendering.OnPreRendering += OnPreRendering;
            Rendering.OnRendering += OnRendering;
            Rendering.OnRendering += OnPostRendering;
        }

        protected void DeleteRenderTask()
        {
            if (!RenderTask)
                return;
            
            Rendering.OnPreRendering -= OnPreRendering;
            Rendering.OnRendering -= OnRendering;
            Rendering.OnRendering -= OnPostRendering;
            RenderTask.Destroy();
            RenderTask = null;
        }

        protected virtual void OnPreRendering() { }
        
        protected virtual void OnRendering() { }
        
        protected virtual void OnPostRendering() { }

        protected override void OnDestroy()
        {
            RenderTask.Destroy();
        }
    }
}