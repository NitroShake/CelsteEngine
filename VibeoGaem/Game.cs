using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CelsteEngine;
using OpenTK.Mathematics;

namespace VibeoGaem
{
    internal class Game : CelsteGame
    {
        public Game(int width, int height, string title) : base(width, height, title)
        {
            
        }

        protected override void OnLoad()
        {
            /*            NodeManager.masterNode = new MeshInstance("testassets/Grass_Block.obj", "testassets/Grass_Block_TEX.png", new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), false, 
                        new Node[]
                        {
                            new MeshInstance("testassets/testcone.obj", "testassets/test2.png", new Vector3(0, 0, -10), new Vector3(0, 0, 0), new Vector3(0, 0, 0), false, new List<Node>(), null, new Color4(255,255,255,255)),
                            new MeshInstance("testassets/testtorus.obj", "testassets/test3.png", new Vector3(0, 3, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), false, new List<Node>(), null, new Color4(255,255,255,255)),
                            new Camera(new Vector3(0,0,0), new Vector3(0,0,0), new Vector3(0,0,0), false, new List<Node>(), null, 1.25f, true),
                            //new MeshInstance3D(new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), false, new List<Node>(), null, new Color4(111,3,3,255) ), 
                        }.ToList(), 
                        null, new Color4(255, 3, 3, 255));*/

            NodeManager.masterNode = new Node3D(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), false,
                new Node[]
                {
                    new MeshInstance("testassets/testcone.obj", "testassets/test2.png", new Vector3(0, 0, -10), new Vector3(0, 0, 0), new Vector3(0, 0, 0), false, new List<Node>(), null, new Color4(255,255,255,255)),
                    //new AabbCollider(new Vector3(0,0, -10), new Vector3(0,0,0), new Vector3(3, 3, 3), false, new List<Node>(), null),
                    new SphereCollider(new Vector3(0,0, -10), new Vector3(0,0,0), new Vector3(1,1,1), false, new List<Node>(), null ),
                    new MeshInstance("testassets/testtorus.obj", "testassets/test3.png", new Vector3(0, 3, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), false, new List<Node>(), null, new Color4(255,255,255,255)),
                    new Asteroid(new Vector3(5,0,0), new Vector3(0,0,0), new Vector3(2.25f,2.25f,2.25f), false, new List<Node>(), null ),
                    new Player(new Vector3(0,0,0), new Vector3(0,0,0), new Vector3(1.1f,1.1f,1.1f), false, new List<Node>(), null)
                }.ToList());
            base.OnLoad();
        }
    }
}
