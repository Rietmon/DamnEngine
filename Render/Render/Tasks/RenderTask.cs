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
        
        public RenderTask(Vector3[] vertices, Vector2[] uv, Vector3[] normals, int[] indices, Material material)
        {
            var verticesAndUvs = GetVerticesAndUvs(vertices, uv, normals);
            vertexBufferPointer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferPointer);
            GL.BufferData(BufferTarget.ArrayBuffer, verticesAndUvs.Length * sizeof(float), verticesAndUvs, BufferUsageHint.StaticDraw);

            elementBufferPointer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferPointer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);
            
            vertexArrayPointer = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayPointer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferPointer);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferPointer);
            
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
            material.Use();
            
            GL.BindVertexArray(vertexArrayPointer);
            
            GL.DrawElements(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedInt, 0);
        }

        protected override void OnDestroy()
        {
            GL.DeleteBuffer(vertexBufferPointer);
            GL.DeleteBuffer(elementBufferPointer);
            GL.DeleteVertexArray(vertexArrayPointer);
        }

        private static float[] GetVerticesAndUvs(Vector3[] vertices, Vector2[] uvs, Vector3[] normals)
        {
            var result = new float[vertices.Length * 8];
            for (var i = 0; i < result.Length; i += 8)
            {
                var vertex = vertices[i / 8];
                var uv = uvs[i / 8];
                var normal = normals[i / 8];

                result[i + 0] = vertex.X;
                result[i + 1] = vertex.Y;
                result[i + 2] = vertex.Z;
                result[i + 3] = uv.X;
                result[i + 4] = uv.Y;
                result[i + 5] = normal.X;
                result[i + 6] = normal.Y;
                result[i + 7] = normal.Z;
            }

            return result;
        }
    }
}