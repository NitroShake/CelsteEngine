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

        public void move(Vector3 distance, float stepSize = 0.1f)
        {
            Vector3 targetPos = distance + position;
            Vector3 step = distance.Normalized() * stepSize;
            Collider collidingObject = null;
            while (collidingObject == null && position != targetPos)
            {
                if (Vector3.Distance(position, targetPos) > stepSize)
                {
                    position = targetPos;
                }
                else
                {
                    position += step;
                }
                foreach (Collider collider in NodeManager.colliderNodes)
                {
                    if (checkForCollision(collider))
                    {
                        collidingObject = collider;
                    }
                }
            }
            if (collidingObject != null)
            {
                resolveCollision(collidingObject);
            }
        }
        
        public abstract bool checkForCollision(Collider collider); //
        public abstract void resolveCollision(Collider collider); //to be called by checkForCollision when the collider hits a solid collider.
    }
}
