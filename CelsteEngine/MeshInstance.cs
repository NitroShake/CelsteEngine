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
        int texture;
        Color4 color;
        int vao;
        int vbo;
        int ebo;

        public MeshInstance(string meshDir, string textureDir, Vector3 position, Vector3 rotation, Vector3 scale, bool inheritTransform, List<Node> children, Node? parent, Color4 color) : base(position, rotation, scale, inheritTransform, children, parent)
        {
            this.color = color;
            mesh = loadMesh(meshDir);
            texture = AssetManager.loadTexture(textureDir);
            vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();
            ebo = GL.GenBuffer();

            shader = loadShader();
            shader.Use();

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, mesh.vertices.Length * sizeof(float), mesh.vertices, BufferUsageHint.DynamicDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, mesh.indices.Length * sizeof(uint), mesh.indices, BufferUsageHint.DynamicDraw);

            int vertexPos = shader.getAttributeLocation("aPosition");
            GL.EnableVertexAttribArray(vertexPos);
            GL.VertexAttribPointer(vertexPos, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = shader.getAttributeLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            //GL.Uniform1(GL.GetUniformLocation(shader.getHandle(), "texture0"), 0);
        }

        internal override void draw()
        {
            GL.BindVertexArray(vao);
            var model = Matrix4.CreateTranslation(position);
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", NodeManager.activeCamera.GetViewMatrix());
            shader.SetMatrix4("projection", NodeManager.activeCamera.GetProjectionMatrix());
            GL.Uniform4(GL.GetUniformLocation(shader.getHandle(), "color"), color);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.DrawElements(PrimitiveType.Triangles, mesh.indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        Mesh loadMesh(string meshDir)
        {
            return AssetManager.loadMesh(meshDir);
            return new Mesh(new float[] {        // Position             Texture coordinates
        0.5f,  0.5f, 0.0f,      1.0f, 1.0f, // top right
        0.5f, -0.5f, 0.0f,      1.0f, 0.0f, // bottom right
        -0.5f, -0.5f, 0.0f,     0.0f, 0.0f, // bottom left
        -0.5f,  0.5f, 0.0f,     0.0f, 1.0f  // top left
            }, new uint[] { 0, 1, 3,
        1, 2, 3});

            
        }

        Shader loadShader()
        {
            return new Shader("testassets/shader.vert", "testassets/shader.frag");
        }
    }
}
