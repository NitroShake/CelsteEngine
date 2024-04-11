using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelsteEngine
{
    public class Mesh
    {
        public Mesh(float[] verts, uint[] indices)
        {
            this.vertices = verts;
            this.indices = indices;
        }



        public float[] vertices;
        public uint[] indices;
    }
}
