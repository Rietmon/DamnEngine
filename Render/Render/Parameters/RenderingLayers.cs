using System;

namespace DamnEngine.Render
{
    [Flags]
    public enum RenderingLayers
    {
        None = 0,
        Default = 1,
        Special = 2,
        All = 0xfffffff
    }
}