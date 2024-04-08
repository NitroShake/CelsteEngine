using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelsteEngine
{
    public class Game : GameWindow
    {

        List<VisualNode> visualNodes;
        Node masterNode; // the parent node that connects to all other nodes. i'm sure there's no data structures this could be compared to /s

        public Game(Node startingNode, int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title }) 
        {
            masterNode = startingNode;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            masterNode.update(args.Time);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            foreach (VisualNode node in visualNodes)
            {
                node.draw();
            }
        }
    }
}
