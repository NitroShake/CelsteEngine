using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CelsteEngine
{
    public class GameTest : GameWindow
    {
        public GameTest(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title }) { }

        float[] vertices = {
             0.5f,  0.5f, 0.0f,  // top right
             0.5f, -0.5f, 0.0f,  // bottom right
            -0.5f, -0.5f, 0.0f,  // bottom left
            -0.5f,  0.5f, 0.0f   // top left
        };
        uint[] indices = {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };
        int ElementBufferObject;
        int VertexBufferObject;
        int VertexArrayObject;
        Shader shader;
        Stopwatch stopwatch = new();
        double lastRecordedUpdateTime = 0;
        double updateDeltaTime = 0;

        double lastRecordedDrawTime = 0;
        double drawDeltaTime = 0;

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.3f, 0.1f, 0.3f, 1f);

            VertexBufferObject = GL.GenBuffer();
            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            shader = new Shader("shader.vert", "shader.frag");
            shader.use();

            GL.Enable(EnableCap.DepthTest);

            stopwatch.Start();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            updateDeltaTime = stopwatch.Elapsed.TotalSeconds - lastRecordedUpdateTime;
            lastRecordedUpdateTime = stopwatch.Elapsed.TotalSeconds;

            base.OnUpdateFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            drawDeltaTime = stopwatch.Elapsed.TotalSeconds - lastRecordedDrawTime;
            lastRecordedDrawTime = stopwatch.Elapsed.TotalSeconds;

            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            double timeValue = stopwatch.Elapsed.TotalSeconds;
            float greenValue = (float)Math.Sin(timeValue) / 2.0f + 0.5f;
            int vertexColorLocation = GL.GetUniformLocation(shader.getHandle(), "color");
            GL.Uniform4(vertexColorLocation, 0.0f, greenValue, 0.0f, 1.0f);

            shader.use();

            GL.BindVertexArray(VertexArrayObject);

            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }
    }
}
