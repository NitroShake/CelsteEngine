using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelsteEngine
{
    public abstract class Collider : Node3D
    {
        protected Collider(Vector3 position, Vector3 rotation, Vector3 scale, bool inheritTransform, List<Node> children, Node? parent) : base(position, rotation, scale, inheritTransform, children, parent)
        {
        }

        bool isSolid;

        public abstract void move();
        
        public abstract void checkForCollision(Collider collider); //
        public abstract void resolveCollision(Collider collider); //to be called by checkForCollision when the collider hits a solid collider.
        public abstract void onCollision(Collider collider); //to be called by checkForCollision when the collider hits a non-solid collider.

        public abstract void getClosestPoint(Vector3 target);
    }
}
