using DamnEngine.Render;
using OpenTK;

namespace DamnEngine
{
    public class Renderer : Component
    {
        protected RenderTask RenderTask { get; set; }

        protected void CreateRenderTask(float[] renderTaskData, int[] indices, Material material, uint ownerId)
        {
            if (RenderTask)
                RenderTask.Destroy();

            RenderTask = RenderTask.Create(renderTaskData, indices, material, ownerId);
            Rendering.OnRendering += OnRendering;
        }

        protected void DeleteRenderTask()
        {
            if (!RenderTask)
                return;
            
            RenderTask.Destroy();
            RenderTask = null;
            Rendering.OnRendering -= OnRendering;
        }
        
        public virtual void OnRendering() { }

        protected override void OnDestroy()
        {
            DeleteRenderTask();
        }
    }
}