using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelsteEngine
{
    public class CelsteGame : GameWindow
    {
        public CelsteGame(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title }) 
        {
            NodeManager.game = this;
            VSync = VSyncMode.On;
            
        }

        protected override void OnLoad()
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);

            GL.Enable(EnableCap.CullFace);

            base.OnLoad();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (NodeManager.masterNode != null)
            {
                NodeManager.masterNode.update(args.Time);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            foreach (VisualNode node in NodeManager.visualNodes)
            {
                node.draw();
            }
            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
            // We need to update the aspect ratio once the window has been resized.
            NodeManager.activeCamera.aspectRatio = Size.X / (float)Size.Y;
        }
    }
}
