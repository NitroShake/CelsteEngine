using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CelsteEngine;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace VibeoGaem
{
    internal class Game : CelsteGame
    {
        public Game(int width, int height, string title) : base(width, height, title)
        {
        }

        protected override void OnLoad()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.StencilTest);
            GL.StencilFunc(StencilFunction.Notequal, 1, 0xFF);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);

            NodeManager.masterNode = new GameWorld();
            base.OnLoad();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }
    }
}
