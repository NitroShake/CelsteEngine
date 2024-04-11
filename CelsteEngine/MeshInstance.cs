using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelsteEngine
{
    public class MeshInstance : VisualNode
    {
        Mesh mesh;
        Shader shader;
        Color4 color;
        int vao;
        int vbo;
        int ebo;

        public MeshInstance(string meshDir, Vector3 position, Vector3 rotation, Vector3 scale, bool inheritTransform, List<Node> children, Node? parent, Color4 color) : base(position, rotation, scale, inheritTransform, children, parent)
        {
            this.color = color;
            mesh = loadMesh(meshDir);
            vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();
            ebo = GL.GenBuffer();

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, mesh.vertices.Length * sizeof(float), mesh.vertices, BufferUsageHint.DynamicDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, mesh.indices.Length * sizeof(uint), mesh.indices, BufferUsageHint.DynamicDraw);

            shader = loadShader();
            shader.Use();

            int vertexPos = shader.getAttributeLocation("aPosition");
            GL.EnableVertexAttribArray(vertexPos);
            GL.VertexAttribPointer(vertexPos, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        }

        internal override void draw()
        {
            GL.BindVertexArray(vao);
            var model = Matrix4.Identity;
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", NodeManager.activeCamera.GetViewMatrix());
            shader.SetMatrix4("projection", NodeManager.activeCamera.GetProjectionMatrix());
            GL.Uniform4(GL.GetUniformLocation(shader.getHandle(), "color"), color);
            GL.DrawElements(PrimitiveType.Triangles, mesh.indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        Mesh loadMesh(string meshDir)
        {
            return AssetManager.loadMesh(meshDir);
        }

        Shader loadShader()
        {
            return new Shader("testassets/shader.vert", "testassets/shader.frag");
        }
    }
}
