using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelsteEngine
{
    public abstract class Node3D : Node
    {
        public Node3D(Vector3 position, Vector3 rotation, Vector3 scale, bool inheritTransform, List<Node> children, Node? parent) : base(children, parent)
        {
            this.scale = scale;
            this.position = position;
            this.rotation = rotation;
            this.inheritTransform = inheritTransform;
            if (parent is not Node3D && inheritTransform)
            {
                throw new Exception("Node3D cannot inherit transform from a non-Node3D!");
            }
        }
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
        bool inheritTransform;
    }
}
