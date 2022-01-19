namespace DamnEngine
{
    public enum PipelineTiming
    {
        Now,
        OnBeginFrame,
        BeforePreUpdate,
        BeforeUpdate,
        BeforePhysicsUpdate,
        BeforePostUpdate,
        OnBeginRender,
        OnPreRendering,
        OnRendering,
        OnPostRendering,
        OnInputUpdate,
        OnEndFrame,
        Never,
    }
}