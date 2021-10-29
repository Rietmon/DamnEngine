using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace DamnEngine.Render
{
    public class RenderTask : DamnObject
    {
        private readonly int vertexArrayPointer;
        private readonly int vertexBufferPointer;
        private readonly int elementBufferPointer;
        private readonly Material material;
        private readonly int indicesCount;
        
        public RenderTask(float[] renderTaskData, int[] indices, Material material)
        {
            vertexArrayPointer = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayPointer);
            
            vertexBufferPointer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferPointer);
            GL.BufferData(BufferTarget.ArrayBuffer, renderTaskData.Length * sizeof(float), renderTaskData, BufferUsageHint.StaticDraw);

            elementBufferPointer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferPointer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            
            var vertexLocation = material.GetAttributeLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            material.Use();

            var texturesPointers = material.TexturesPointers;
            for (var i = 0; i < texturesPointers.Length; i++)
            {
                var textureCoordinatesLocation = material.GetAttributeLocation($"aTexCoord{texturesPointers[i]}");
                GL.EnableVertexAttribArray(textureCoordinatesLocation);
                GL.VertexAttribPointer(textureCoordinatesLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            }
            
            var normalLocation = material.GetAttributeLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));

            this.material = material;
            indicesCount = indices.Length;
        }

        public void Draw()
        {
            GL.BindVertexArray(vertexArrayPointer);
            
            material.Use();
            
            GL.DrawElements(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedInt, 0);
        }

        protected override void OnDestroy()
        {
            GL.DeleteBuffer(vertexBufferPointer);
            GL.DeleteBuffer(elementBufferPointer);
            GL.DeleteVertexArray(vertexArrayPointer);
        }
    }
}