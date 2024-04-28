using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

        public override bool checkForCollision(Collider collider)
        {
            if (collider is AabbCollider)
            {
                return CollisionDetector.checkSphereAabbOverlap(this, (AabbCollider)collider);
            }
            else if (collider is SphereCollider)
            {
                return CollisionDetector.checkSphereSphereOverlap((SphereCollider)collider, this);
            }
            else return false;
        }

        public override void resolveCollision(Collider collider, Vector3 originalDirection, bool continueMoving)
        {
            if (collider is SphereCollider)
            {
                float overlap = (radius + (collider as SphereCollider).radius) - Vector3.Distance(collider.position, position);
                position -= originalDirection * overlap;
                Vector3 normal = (collider.position - position).Normalized();
                getNewDirectionAlongNormal(originalDirection, normal);
            }
            else if (collider is AabbCollider)
            {
                Vector3 cMaxBounds = new Vector3(collider.position.X + ((collider as AabbCollider).width / 2), collider.position.Y + ((collider as AabbCollider).height / 2), collider.position.Z + ((collider as AabbCollider).depth / 2));
                Vector3 cMinBounds = new Vector3(collider.position.X - ((collider as AabbCollider).width / 2), collider.position.Y - ((collider as AabbCollider).height / 2), collider.position.Z - ((collider as AabbCollider).depth / 2));
                Vector3 aabbClosestPointToSphere = new Vector3(
                    Math.Max(cMinBounds.X, Math.Min(position.X, cMaxBounds.X)),
                    Math.Max(cMinBounds.X, Math.Min(position.X, cMaxBounds.X)),
                    Math.Max(cMinBounds.X, Math.Min(position.X, cMaxBounds.X))
                );
                Vector3 closestPointOffset = new Vector3(
                    aabbClosestPointToSphere.X - collider.position.X,
                    aabbClosestPointToSphere.Y - collider.position.Y,
                    aabbClosestPointToSphere.Z - collider.position.Z
                );
                Vector3 closestPointOffsetAbs = new Vector3(
                    Math.Abs(closestPointOffset.X),
                    Math.Abs(closestPointOffset.Y),
                    Math.Abs(closestPointOffset.Z)
                );
                float dx = 0;
                float dy = 0;
                float dz = 0;
                if (closestPointOffsetAbs.X >= closestPointOffsetAbs.Y && closestPointOffsetAbs.X >= closestPointOffsetAbs.Z)
                {
                    dx = closestPointOffset.X;
                }
                else if (closestPointOffsetAbs.Y >= closestPointOffsetAbs.X && closestPointOffsetAbs.Y >= closestPointOffsetAbs.Z)
                {
                    dy = closestPointOffsetAbs.Y;
                }
                else
                {
                    dz = closestPointOffsetAbs.Z;
                }
                Vector3 normal = new Vector3(dx, dy, dz);
                float overlap = Vector3.Distance(aabbClosestPointToSphere, position) - radius;
                position += normal * overlap;
                move(getNewDirectionAlongNormal(originalDirection, normal));
            }
        }
    }
}
