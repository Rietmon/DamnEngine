namespace DamnEngine.Render
{
    public readonly struct RenderParameters
    {
        public readonly RenderingLayers renderingLayers;

        public RenderParameters(RenderingLayers renderingLayers)
        {
            this.renderingLayers = renderingLayers;
        }
    }
}