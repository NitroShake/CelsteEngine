using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelsteEngine
{
    public struct CollisionInfo
    {
        Vector3 point;
        Vector3 normal;

        public CollisionInfo(Vector3 point, Vector3 normal)
        {
            this.point = point;
            this.normal = normal;
        }
    }
}
