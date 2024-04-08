using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelsteEngine
{
    public abstract class VisualNode : Node
    {
        protected VisualNode(Vector3 position, Vector3 rotation, bool inheritTransform, List<Node> children, Node? parent) : base(position, rotation, inheritTransform, children, parent)
        {
        }
        internal abstract void draw();
    }
}
