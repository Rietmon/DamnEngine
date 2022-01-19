using System;
using System.Windows.Forms;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace DamnEngine
{
    public static class Application
    {
        public static RenderWindow Window { get; set; }
        
        public static Action OnNextFrameUpdate { get; set; }

        public static void Initialize()
        {
            Window.VSync = VSyncMode.On;
            
            Physics.Initialize();
        }

        public static void Update()
        {
            DamnObjectsFactory.UpdateFactory(PipelineTiming.BeforePreUpdate);
            ScenesManager.CurrentScene.ForEachActiveGameObjectEnabledComponent((component) => component.OnPreUpdate());
            
            DamnObjectsFactory.UpdateFactory(PipelineTiming.BeforeUpdate);
            OnNextFrameUpdate?.Invoke();
            OnNextFrameUpdate = null;
            ScenesManager.CurrentScene.ForEachActiveGameObjectEnabledComponent((component) => component.OnUpdate());
            
            DamnObjectsFactory.UpdateFactory(PipelineTiming.BeforePhysicsUpdate);
            Physics.Update(Time.DeltaTime);
            
            DamnObjectsFactory.UpdateFactory(PipelineTiming.BeforePostUpdate);
            ScenesManager.CurrentScene.ForEachActiveGameObjectEnabledComponent((component) => component.OnPostUpdate());
        }
    }
}