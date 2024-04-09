using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace CelsteEngine
{
    static class AssetManager
    {
        public static Dictionary<string, Mesh> meshes = new();

        /// <summary>
        /// Removes the mesh from the AssetManager dictionary. The mesh will still remain in memory if referenced by other objects.
        /// </summary>
        /// <param name="dir">File path</param>
        public static void unloadMesh(string dir)
        {
            meshes.Remove(dir);
        }


        /// <summary>
        /// Loads the mesh from file, or fetches it if previously loaded. This can also be used to preload meshes before they are needed.
        /// </summary>
        /// <param name="dir">File path</param>
        /// <returns>The requested mesh</returns>
        public static Mesh loadMesh(string dir)
        {
            if (meshes.ContainsKey(dir))
            {
                return meshes[dir];
            } 
            else
            {
                Mesh mesh = loadObj(dir);
                meshes.Add(dir, mesh);
                return mesh;
            }
        }

        static private Mesh loadObj(string dir)
        {
            List<float> vertexes = new List<float>();
            List<uint> indices = new List<uint>();
            StreamReader sr = new StreamReader(dir);
            for (string line = sr.ReadLine(); line != null; line = sr.ReadLine())
            {
                string[] splitLine = line.Split(' ');
                if (splitLine[0] == "v")
                {
                    vertexes.Add(float.Parse(splitLine[1]));
                    vertexes.Add(float.Parse(splitLine[2]));
                    vertexes.Add(float.Parse(splitLine[3]));
                }
                else if (splitLine[0] == "f")
                {
                    for (int i = 1; i < splitLine.Length; i++)
                    {
                        string splitArg = splitLine[i].Split('/')[0];
                        indices.Add(uint.Parse(splitArg));
                    }
                }
            }
            return new Mesh(vertexes.ToArray(), indices.ToArray());
        }

    }
}
