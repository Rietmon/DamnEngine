namespace DamnEngine
{
    public class ShaderData : DamnObject
    {
        public string VertexShaderName { get; }
        public string FragmentShaderName { get; }
        public string VertexShaderCode { get; }
        public string FragmentShaderCode { get; }
        
        public ShaderData(string vertexShaderName, string fragmentShaderName, string vertexShaderCode, string fragmentShaderCode) : base(PipelineTiming.Now)
        {
            VertexShaderName = vertexShaderName;
            FragmentShaderName = fragmentShaderName;
            VertexShaderCode = vertexShaderCode;
            FragmentShaderCode = fragmentShaderCode;
        }
    }
}