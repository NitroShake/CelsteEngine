using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;

namespace CelsteEngine
{
    //This class detects and resolves collisions between similar or different shapes.
    //It exists to reduce code duplication.
    static class CollisionDetector
    {
        public static bool checkPointSphereOverlap(Vector3 point, SphereCollider c)
        {
            float distance = Vector3.Distance(c.position, point);
            return distance < c.radius;
        }

        public static bool checkPointAabbOverlap(Vector3 point, AabbCollider c)
        {
            Vector3 cMaxBounds = new Vector3(c.position.X + (c.width / 2), c.position.Y + (c.height / 2), c.position.Z + (c.depth / 2));
            Vector3 cMinBounds = new Vector3(c.position.X - (c.width / 2), c.position.Y - (c.height / 2), c.position.Z - (c.depth / 2));
            return (
              point.X >= cMinBounds.X &&
              point.X <= cMaxBounds.X &&
              point.Y >= cMinBounds.Y &&
              point.Y <= cMaxBounds.Y &&
              point.Z >= cMinBounds.Z &&
              point.Z <= cMaxBounds.Z
            );
        }

        public static bool checkSphereAabbOverlap(SphereCollider sc, AabbCollider aabbc)
        {
            Vector3 cMaxBounds = new Vector3(aabbc.position.X + (aabbc.width / 2), aabbc.position.Y + (aabbc.height / 2), aabbc.position.Z + (aabbc.depth / 2));
            Vector3 cMinBounds = new Vector3(aabbc.position.X - (aabbc.width / 2), aabbc.position.Y - (aabbc.height / 2), aabbc.position.Z - (aabbc.depth / 2));
            Vector3 aabbClosestPointToSphere = new Vector3(
                Math.Max(cMinBounds.X, Math.Min(sc.position.X, cMaxBounds.X)),
                Math.Max(cMinBounds.X, Math.Min(sc.position.X, cMaxBounds.X)),
                Math.Max(cMinBounds.X, Math.Min(sc.position.X, cMaxBounds.X))
            );
            return checkPointSphereOverlap(aabbClosestPointToSphere, sc);
        }

        public static bool checkAabbAabbOverlap(AabbCollider c1, AabbCollider c2)
        {
            Vector3 c1MaxBounds = new Vector3(c1.position.X + (c1.width / 2), c1.position.Y + (c1.height / 2), c1.position.Z + (c1.depth / 2));
            Vector3 c1MinBounds = new Vector3(c1.position.X - (c1.width / 2), c1.position.Y - (c1.height / 2), c1.position.Z - (c1.depth / 2));
            Vector3 c2MaxBounds = new Vector3(c2.position.X + (c2.width / 2), c2.position.Y + (c2.height / 2), c2.position.Z + (c2.depth / 2));
            Vector3 c2MinBounds = new Vector3(c2.position.X + (c2.width / 2), c2.position.Y + (c2.height / 2), c2.position.Z + (c2.depth / 2));

            return (
                c1MinBounds.X <= c2MaxBounds.X &&
                c1MaxBounds.X >= c2MinBounds.X &&
                c1MinBounds.Y <= c2MaxBounds.Y &&
                c1MaxBounds.Y >= c2MinBounds.Y &&
                c1MinBounds.Z <= c2MaxBounds.Z &&
                c1MaxBounds.Z >= c2MinBounds.Z
            );
        }

        public static bool checkSphereSphereOverlap(SphereCollider c1, SphereCollider c2)
        {
            float distance = Vector3.Distance(c1.position, c2.position);
            return distance < c1.radius + c2.radius;
        }

        //May go unused. Theory: convert sphere's position to OBB's axis, then test the same way as AABB
        static void handleObbSphere()
        {

        }
    }
}
