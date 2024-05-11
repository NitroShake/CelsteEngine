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
        public int id = 0;
        public List<int> ignoreIds = new();
        protected Collider(Vector3 position, Vector3 rotation, Vector3 scale, bool inheritTransform, List<Node> children, Node? parent) : base(position, rotation, scale, inheritTransform, children, parent)
        {
            NodeManager.colliderNodes.Add(this);
        }

        public override void dispose()
        {
            NodeManager.colliderNodes.Remove(this);
            base.dispose();
        }

        bool isSolid;

        public void move(Vector3 distance, float stepSize = 0.01f)
        {
            Vector3 targetPos = distance + position;
            Vector3 step = distance.Normalized() * stepSize;
            Collider collidingObject = null;
            int stepsTaken = 0;
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
                    if (collider != this && !NodeManager.game.nodesToRemove.Contains(collider) && !ignoreIds.Contains(collider.id) && checkForCollision(collider))
                    {
                        collidingObject = collider;
                    }
                }
                stepsTaken++;
            }
            if (collidingObject != null)
            {
                Console.WriteLine("collider");
                resolveCollision(collidingObject, targetPos - position, stepsTaken > 1 && position != targetPos);
            }
        }

        public Vector3 getNewDirectionAlongNormal(Vector3 oldDirection, Vector3 normal)
        {
            Vector3 vec = oldDirection + (normal * Vector3.Dot(oldDirection, normal));
            //vec = vec.Normalized() * Vector3.Distance(new Vector3(0, 0, 0), oldDirection);
            return vec;
        }

        public abstract bool checkForCollision(Collider collider); //
        public abstract void resolveCollision(Collider collider, Vector3 originalDirection, bool continueMoving); //to be called by checkForCollision when the collider hits a solid collider.
    }
}
