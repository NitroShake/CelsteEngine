using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelsteEngine
{
    public class DirectionalLight : Node3D
    {
        public DirectionalLight(bool startActive, Vector3 position, Vector3 rotation, Vector3 scale, bool inheritTransform, List<Node> children = null, Node? parent = null) : base(position, rotation, scale, inheritTransform, children, parent)
        {
            if (startActive)
            {
                NodeManager.activeLight = this;
            }
        }
    }
}
