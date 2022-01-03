using DamnEngine.Render;

namespace DamnEngine
{
    public class Renderer : Component
    {
        public RenderingLayers RenderingLayer { get; set; } = RenderingLayers.Default;
        
        protected RenderTask RenderTask { get; set; }

        protected void CreateRenderTask(float[] renderTaskData, int[] indices, Material material, uint ownerId)
        {
            if (RenderTask)
                RenderTask.Destroy();
            else
                Rendering.OnRendering += OnRendering;

            RenderTask = RenderTask.Create(renderTaskData, indices, material, ownerId);
        }

        protected void DeleteRenderTask()
        {
            if (!RenderTask)
                return;
            
            RenderTask.Destroy();
            RenderTask = null;
            Rendering.OnRendering -= OnRendering;
        }

        protected virtual void OnRendering() { }

        protected override void OnDestroy()
        {
            DeleteRenderTask();
        }
    }
}