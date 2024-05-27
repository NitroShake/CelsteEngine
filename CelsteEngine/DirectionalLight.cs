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
        public float ambient;
        public float diffuse;
        public float specular;
        public DirectionalLight(bool startActive, float specular, float diffuse, float ambient, Vector3 rotation, List<Node> children = null, Node? parent = null) : base(new Vector3(), rotation, new Vector3(), false, children, parent)
        {
            this.ambient = ambient;
            this.diffuse = diffuse;
            this.specular = specular;
            if (startActive)
            {
                NodeManager.activeLight = this;
            }
        }

        public Vector3 getDirection()
        {
            return Quaternion.FromEulerAngles(rotation) * new Vector3(0,0,1);
        }
    }
}
