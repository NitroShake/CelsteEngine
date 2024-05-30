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
        protected Mesh mesh;
        protected Shader shader;
        protected int texture;
        protected Color4 color;
        protected int vao;
        protected int vbo;
        protected int ebo;

        public MeshInstance(string meshDir, string textureDir, Vector3 position, Vector3 rotation, Vector3 scale, bool inheritTransform, List<Node> children = null, Node? parent = null, Color4? color = null) : base(position, rotation, scale, inheritTransform, children, parent)
        {
            if (color != null)
            {
                this.color = (Color4)color;
            }
            else
            {
                color = new Color4(255, 255, 255, 255);
            }
            mesh = AssetManager.loadMesh(meshDir);
            texture = AssetManager.loadTexture(textureDir);
            vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();
            ebo = GL.GenBuffer();

            shader = loadShader();
            shader.use();

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, mesh.vertices.Length * sizeof(float), mesh.vertices, BufferUsageHint.DynamicDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, mesh.indices.Length * sizeof(uint), mesh.indices, BufferUsageHint.DynamicDraw);

            int vertexPos = shader.getAttributeLocation("aPosition");
            GL.EnableVertexAttribArray(vertexPos);
            GL.VertexAttribPointer(vertexPos, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            var texCoordLocation = shader.getAttributeLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            var normalPos = shader.getAttributeLocation("aNormal");
            GL.EnableVertexAttribArray(normalPos);
            GL.VertexAttribPointer(normalPos, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));
        }

        internal override void draw()
        {
            GL.StencilMask(0x00);
            GL.BindVertexArray(vao);
            shader.use();
            var model = Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(rotation)) * Matrix4.CreateTranslation(position);
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", NodeManager.activeCamera.getViewMatrix());
            shader.SetMatrix4("projection", NodeManager.activeCamera.getProjectionMatrix());
            GL.Uniform4(GL.GetUniformLocation(shader.getHandle(), "color"), color);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.DrawElements(PrimitiveType.Triangles, mesh.indices.Length, DrawElementsType.UnsignedInt, 0);

            /*            GL.ClearStencil(0);
                        GL.Clear(ClearBufferMask.StencilBufferBit);
                        GL.StencilMask(0);

                        // Render the mesh into the stencil buffer.
                        GL.Enable(EnableCap.StencilTest);
                        GL.StencilFunc(StencilFunction.Always, 1, -1);
                        GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
                        GL.DrawElements(PrimitiveType.Triangles, mesh.indices.Length, DrawElementsType.UnsignedInt, 0);

                        // Render the thick wireframe version.
                        GL.StencilFunc(StencilFunction.Notequal, 1, -1);
                        GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
                        GL.LineWidth(10);
                        GL.PolygonMode(MaterialFace.Front, PolygonMode.Line);
                        GL.DrawElements(PrimitiveType.Triangles, mesh.indices.Length, DrawElementsType.UnsignedInt, 0);*/

        }

        Mesh loadMesh(string meshDir)
        {
            return AssetManager.loadMesh(meshDir);
            return new Mesh(
                new float[] {        // Position             Texture coordinates
                    0.5f,  0.5f, 0.0f,      1.0f, 1.0f, // top right
                    0.5f, -0.5f, 0.0f,      1.0f, 0.0f, // bottom right
                    -0.5f, -0.5f, 0.0f,     0.0f, 0.0f, // bottom left
                    -0.5f,  0.5f, 0.0f,     0.0f, 1.0f  // top left
                }, 
                new uint[] {
                    0, 1, 3,
                    1, 2, 3
                }
            );  
        }

        protected virtual Shader loadShader()
        {
            return AssetManager.loadShader("engineassets/shader.vert", "engineassets/shader.frag");
        }
    }
}
