using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelsteEngine
{
    public class AabbCollider : Collider
    {
        //ordered xyz
        public float width, height, depth;
        public AabbCollider(Vector3 position, Vector3 rotation, Vector3 scale, bool inheritTransform, List<Node> children, Node? parent) : base(position, rotation, scale, inheritTransform, children, parent)
        {
        }

        public override bool checkForCollision(Collider collider)
        {
            if (collider is AabbCollider)
            {
                return CollisionDetector.checkAabbAabbOverlap(this, (AabbCollider)collider);
            }
            else if (collider is SphereCollider)
            {
                return CollisionDetector.checkSphereAabbOverlap((SphereCollider)collider, this);
            }
            else return false;
        }

        public override void resolveCollision(Collider collider)
        {
            throw new NotImplementedException();
        }
    }
}
