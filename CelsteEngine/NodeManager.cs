using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelsteEngine
{
    public static class NodeManager
    {
        public static List<VisualNode> visualNodes = new();
        public static Node3D masterNode;
        public static Camera activeCamera;
    }
}
