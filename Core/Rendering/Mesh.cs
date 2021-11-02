﻿using OpenTK;
using OpenTK.Mathematics;

namespace DamnEngine
{
    public class Mesh : DamnObject
    {
        public string OriginalMeshName { get; }
        public Vector3[] Vertices { get; set; }
        public Vector2[] Uv { get; set; }
        public Vector3[] Normals { get; set; }
        public int[] Indices { get; set; }

        public float[] RenderTaskData
        {
            get
            {
                if (renderTaskData != null)
                    return renderTaskData;
                
                renderTaskData = new float[Vertices.Length * 8];
                for (var i = 0; i < renderTaskData.Length; i += 8)
                {
                    var vertex = Vertices[i / 8];
                    var uv = Uv[i / 8];
                    var normal = Normals[i / 8];

                    renderTaskData[i + 0] = vertex.X;
                    renderTaskData[i + 1] = vertex.Y;
                    renderTaskData[i + 2] = vertex.Z;
                    renderTaskData[i + 3] = uv.X;
                    renderTaskData[i + 4] = uv.Y;
                    renderTaskData[i + 5] = normal.X;
                    renderTaskData[i + 6] = normal.Y;
                    renderTaskData[i + 7] = normal.Z;
                }

                return renderTaskData;
            }
        }
        
        private float[] renderTaskData;

        public Mesh(string name)
        {
            OriginalMeshName = name;
            Name = name;
        }

        protected override void OnDestroy()
        {
            ResourcesLoader.FreeMesh(OriginalMeshName);
        }
        
        public static Mesh[] CreateFromFile(string meshName)
        {
            return ResourcesLoader.UseMeshes(meshName);
        }
    }
}