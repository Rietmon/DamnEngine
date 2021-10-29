using DamnEngine.Render;
using OpenTK;

namespace DamnEngine
{
    public class Renderer : Component, IRenderer
    {
        protected RenderTask RenderTask { get; set; }

        protected void CreateRenderTask(float[] renderTaskData, int[] indices, Material material)
        {
            if (RenderTask)
                RenderTask.Destroy();

            RenderTask = new RenderTask(renderTaskData, indices, material);
            Rendering.renderers.Add(this);
        }

        protected void DeleteRenderTask()
        {
            if (!RenderTask)
                return;
            
            Rendering.renderers.Remove(this);
            RenderTask.Destroy();
            RenderTask = null;
        }

        public virtual void OnPreRendering() { }
        
        public virtual void OnRendering() { }
        
        public virtual void OnPostRendering() { }

        protected override void OnDestroy()
        {
            DeleteRenderTask();
        }
    }
}