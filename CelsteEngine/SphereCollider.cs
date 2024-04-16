using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelsteEngine
{
    public class SphereCollider : Collider
    {
        public float radius;
        public SphereCollider(Vector3 position, Vector3 rotation, Vector3 scale, bool inheritTransform, List<Node> children, Node? parent) : base(position, rotation, scale, inheritTransform, children, parent)
        {
        }

        public override void checkForCollision(Collider collider)
        {
            throw new NotImplementedException();
        }

        public override void move()
        {
            throw new NotImplementedException();
        }

        public override void onCollision(Collider collider)
        {
            throw new NotImplementedException();
        }

        public override void resolveCollision(Collider collider)
        {
            throw new NotImplementedException();
        }
    }
}
