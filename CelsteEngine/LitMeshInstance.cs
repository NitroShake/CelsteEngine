using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelsteEngine
{
    public class LitMeshInstance : MeshInstance
    {
        public LitMeshInstance(string meshDir, string textureDir, Vector3 position, Vector3 rotation, Vector3 scale, bool inheritTransform, List<Node> children = null, Node? parent = null, Color4? color = null) : base(meshDir, textureDir, position, rotation, scale, inheritTransform, children, parent, color)
        {

        }

        internal override void draw()
        {
            GL.StencilMask(0x00);
            GL.BindVertexArray(vao);
            shader.Use();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            int handle = shader.getHandle();
            var model = Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(rotation)) * Matrix4.CreateTranslation(position);
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", NodeManager.activeCamera.GetViewMatrix());
            shader.SetMatrix4("projection", NodeManager.activeCamera.GetProjectionMatrix());
            GL.Uniform3(GL.GetUniformLocation(handle, "viewPos"), NodeManager.activeCamera.position);

            GL.Uniform1(GL.GetUniformLocation(handle, "material.diffuse"), 0);
            GL.Uniform1(GL.GetUniformLocation(handle, "material.specular"), 0);
            GL.Uniform3(GL.GetUniformLocation(handle, "material.specular"), new Vector3(0.15f, 0.15f, 0.15f));
            GL.Uniform1(GL.GetUniformLocation(handle, "material.shininess"), 5f);

            //GL.Uniform3(GL.GetUniformLocation(handle, "material.direction"), NodeManager.activeLight.getDirection());
            GL.Uniform3(GL.GetUniformLocation(handle, "light.direction"), NodeManager.activeLight.getDirection());
            GL.Uniform3(GL.GetUniformLocation(handle, "light.ambient"), new Vector3(NodeManager.activeLight.ambient));
            GL.Uniform3(GL.GetUniformLocation(handle, "light.diffuse"), new Vector3(NodeManager.activeLight.diffuse));
            GL.Uniform3(GL.GetUniformLocation(handle, "light.specular"), new Vector3(NodeManager.activeLight.specular));


            GL.DrawElements(PrimitiveType.Triangles, mesh.indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        protected override Shader loadShader()
        {
            return new Shader("engineassets/shader.vert", "engineassets/litshader.frag");
        }
    }
}
