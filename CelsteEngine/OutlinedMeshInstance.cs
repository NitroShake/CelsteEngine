using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using static OpenTK.Graphics.OpenGL.GL;

namespace CelsteEngine
{
    public class OutlinedMeshInstance : MeshInstance
    {
        Color4 outlinecolor;
        Shader outlineShader;
        public OutlinedMeshInstance(Color4 outlineColor, string meshDir, string textureDir, Vector3 position, Vector3 rotation, Vector3 scale, bool inheritTransform, List<Node> children = null, Node? parent = null, Color4? color = null) : base(meshDir, textureDir, position, rotation, scale, inheritTransform, children, parent, color)
        {
            this.outlinecolor = outlineColor;
            outlineShader = new Shader("engineassets/shader.vert", "engineassets/outline.frag");
        }

        internal override void draw()
        {
            shader.Use();
            GL.StencilFunc(StencilFunction.Always, 1, 0xFF);
            GL.StencilMask(0xFF);

            GL.BindVertexArray(vao);
            var model = Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(rotation)) * Matrix4.CreateTranslation(position);
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", NodeManager.activeCamera.GetViewMatrix());
            shader.SetMatrix4("projection", NodeManager.activeCamera.GetProjectionMatrix());
            GL.Uniform4(GL.GetUniformLocation(shader.getHandle(), "color"), color);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.DrawElements(PrimitiveType.Triangles, mesh.indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            outlineShader.Use();
            GL.StencilFunc(StencilFunction.Notequal, 1, 0xFF);
            GL.StencilMask(0x00);
            GL.Disable(EnableCap.DepthTest);
            GL.BindTexture(TextureTarget.Texture2D, -1);
            float outlineScale = 1.1f;
            model = Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(rotation)) * Matrix4.CreateScale(outlineScale) * Matrix4.CreateTranslation(position);
            outlineShader.SetMatrix4("model", model);
            outlineShader.SetMatrix4("view", NodeManager.activeCamera.GetViewMatrix());
            outlineShader.SetMatrix4("projection", NodeManager.activeCamera.GetProjectionMatrix());
            GL.Uniform4(GL.GetUniformLocation(outlineShader.getHandle(), "color"), outlinecolor);
            GL.DrawElements(PrimitiveType.Triangles, mesh.indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.StencilMask(0xFF);
            GL.StencilFunc(StencilFunction.Always, 0, 0xFF);
            GL.Clear(ClearBufferMask.StencilBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
        }
    }
}
