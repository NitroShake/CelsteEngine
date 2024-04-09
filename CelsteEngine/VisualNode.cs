using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelsteEngine
{
    public abstract class VisualNode : Node3D
    {
        protected VisualNode(Vector3 position, Vector3 rotation, Vector3 scale, bool inheritTransform, List<Node> children, Node? parent) : base(position, rotation, scale, inheritTransform, children, parent)
        {
            NodeManager.visualNodes.Add(this);
        }

        internal override void dispose()
        {
            NodeManager.visualNodes.Remove(this);
        }

        internal abstract void draw();
    }
}
