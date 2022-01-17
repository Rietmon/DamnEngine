using DamnEngine.Render;

namespace DamnEngine
{
    public class Renderer : Component
    {
        public RenderingLayers RenderingLayer { get; set; } = RenderingLayers.Default;
        
        protected RenderTask[] RenderTasks { get; private set; }

        protected void CreateRenderTasks(params RenderTaskData[] renderTaskDatas)
        {
            if (RenderTasks != null)
                DeleteRenderTasks();
            else
                Rendering.OnRendering += OnRendering;

            RenderTasks = new RenderTask[renderTaskDatas.Length];
            for (var i = 0; i < renderTaskDatas.Length; i++)
            {
                var data = renderTaskDatas[i];
                RenderTasks[i] = RenderTask.Create(data.Data, data.Indices, data.Material, data.OwnerId);
            }
        }

        protected void DeleteRenderTasks()
        {
            if (RenderTasks == null)
                return;
            
            foreach (var renderTask in RenderTasks)
                renderTask.Destroy();

            RenderTasks = null;
            Rendering.OnRendering -= OnRendering;
        }

        protected void DrawRenderTasks()
        {
            foreach (var renderTask in RenderTasks)
                renderTask.Draw();
        }

        protected virtual void OnRendering() { }

        protected override void OnDestroy()
        {
            DeleteRenderTasks();
        }
    }
}