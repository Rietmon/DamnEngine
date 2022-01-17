using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace DamnEngine.Render
{
    public class RenderTask : LowLevelDamnObject
    {
        private static readonly Dictionary<uint, RenderTask> cachedRenderTasks = new();

        private readonly int vertexArrayPointer;
        private readonly int vertexBufferPointer;
        private readonly int elementBufferPointer;
        private readonly Material material;
        private readonly int indicesCount;
        private readonly uint ownerId;
        
        private RenderTask(int vertexArrayPointer, int vertexBufferPointer, int elementBufferPointer, Material material, int indicesCount, uint ownerId)
        {
            this.vertexArrayPointer = vertexArrayPointer;
            this.vertexBufferPointer = vertexBufferPointer;
            this.elementBufferPointer = elementBufferPointer;
            this.material = material;
            this.indicesCount = indicesCount;
            this.ownerId = ownerId;
        }

        public void Draw()
        {
            GL.BindVertexArray(vertexArrayPointer);
            
            material.Use();
            
            GL.DrawElements(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedInt, 0);

#if ENABLE_STATISTICS
            Statistics.TotalFacesDrawled += (uint)(indicesCount / 3);
#endif
}

        public RenderTask Copy(Material overrideMaterial = null) => new(vertexArrayPointer, vertexBufferPointer, elementBufferPointer, overrideMaterial ?? material,
            indicesCount, ownerId);

        // TODO: Now this deleting render task for all references. Ill fix it in near future <3
        protected override void OnDestroy()
        {
            // GL.DeleteBuffer(vertexBufferPointer);
            // GL.DeleteBuffer(elementBufferPointer);
            // GL.DeleteVertexArray(vertexArrayPointer);
            // cachedRenderTasks.Remove(ownerId);
        }

        public static RenderTask Create(float[] renderTaskData, int[] indices, Material material, uint ownerId)
        {
            if (cachedRenderTasks.TryGetValue(ownerId, out var renderTask))
                return renderTask.Copy(material);
            
            var vertexArrayPointer = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayPointer);
            
            var vertexBufferPointer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferPointer);
            GL.BufferData(BufferTarget.ArrayBuffer, renderTaskData.Length * sizeof(float), renderTaskData, BufferUsageHint.StaticDraw);

            var elementBufferPointer = GL.GenBuffer();
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

            renderTask = new RenderTask(vertexArrayPointer, vertexBufferPointer, elementBufferPointer, material,
                indices.Length, ownerId);
            cachedRenderTasks.Add(ownerId, renderTask);

            return renderTask;
        }
    }
}