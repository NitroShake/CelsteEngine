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
    public static class AssetManager
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

        /// <summary>
        /// this loads an obj file. wowie!!!!!!
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        static private Mesh loadObj(string dir)
        {
            List<float> vertexes = new List<float>();
            List<float> texCoords = new List<float>();
            List<float> normals = new List<float>();
            List<string[]> faceDetails = new List<string[]>();
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
                    //add to facedetails to be processed later
                    faceDetails.Add(splitLine[1..]);
                }
                else if (splitLine[0] == "vt")
                {
                    texCoords.Add(float.Parse(splitLine[1]));
                    texCoords.Add(float.Parse(splitLine[2]));
                }
                else if (splitLine[0] == "vn")
                {
                    normals.Add(float.Parse(splitLine[1]));
                    normals.Add(float.Parse(splitLine[2]));
                    normals.Add(float.Parse(splitLine[3]));
                }
            }
            List<float> vertexData = new();
            List<uint> indices = new List<uint>();

            //get final VBO data
            //this is complex so bear with me here:
            //OBJs can have different texture coordinates per vertex *and* per face
            //OpenGL's approach is to have one texture coordinate per vertex, however
            //therefore, this algorithm duplicates vertex data if they have different texture coordinates, and adjusts element data as necessary.
            foreach (string[] face in faceDetails)
            {
                for (int i = 0; i < face.Length; i++)
                {
                    List<float> potentialVertexData = new List<float>();
                    string[] splitFace = face[i].Split('/');
                    int vertexIndex = int.Parse(splitFace[0]) - 1;
                    int textureCoordIndex = int.Parse(splitFace[1]) - 1;
                    int normalIndex = int.Parse(splitFace[2]) - 1;
                    potentialVertexData.AddRange(vertexes.GetRange(vertexIndex * 3, 3));
                    potentialVertexData.AddRange(texCoords.GetRange(textureCoordIndex * 2, 2));
                    potentialVertexData.AddRange(normals.GetRange(normalIndex * 3, 3));

                    bool vertexDataAlreadyUsed = false;
                    uint vertexStartingIndex = 0;
                    for (int j = 0; j < vertexData.Count; j+=8)
                    {
                        if (potentialVertexData[0] == vertexData[j]
                            && potentialVertexData[1] == vertexData[j+1]
                            && potentialVertexData[2] == vertexData[j+2]
                            && potentialVertexData[3] == vertexData[j+3]
                            && potentialVertexData[4] == vertexData[j+4]
                            && potentialVertexData[5] == vertexData[j+5]
                            && potentialVertexData[6] == vertexData[j+6]
                            && potentialVertexData[7] == vertexData[j+7])
                        {
                            vertexDataAlreadyUsed = true;
                            vertexStartingIndex = (uint)j;
                        }
                    }
                    if (!vertexDataAlreadyUsed)
                    {
                        vertexStartingIndex = (uint)vertexData.Count;
                        vertexData.AddRange(potentialVertexData);
                    }

                    indices.Add(vertexStartingIndex / 8);
                }

            }


            return new Mesh(vertexData.ToArray(), indices.ToArray());
        }



        public static int loadTexture(string dir)
        {
            if (textures.ContainsKey(dir))
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
