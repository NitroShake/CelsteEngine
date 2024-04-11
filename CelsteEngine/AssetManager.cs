using OpenTK.Graphics.OpenGL4;
using StbImageSharp;
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
        public static Dictionary<string, int> textures = new();

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
                        indices.Add(uint.Parse(splitArg) - 1);
                    }
                }
            }
            return new Mesh(vertexes.ToArray(), indices.ToArray());
        }



        public static int loadTexture(string dir)
        {
            if (meshes.ContainsKey(dir))
            {
                return textures[dir];
            }
            else
            {
                int texture = initTexture(dir);
                textures.Add(dir, texture);
                return texture;
            }
        }

        public static void deleteTexture(string dir)
        {
            GL.DeleteTexture(textures[dir]);
            textures.Remove(dir);
        }

        private static int initTexture(string dir)
        {
            int handle = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, handle);

            //flip the image because STB loads differently from OpenGL
            StbImage.stbi_set_flip_vertically_on_load(1);

            //Load image
            ImageResult image = ImageResult.FromStream(File.OpenRead(dir), ColorComponents.RedGreenBlueAlpha);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);

            //texture params
            //texture wrap + repeat
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            //texture scaling
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            //texture mipmaps
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            return handle;
        }
    }
}
