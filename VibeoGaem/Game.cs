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
            NodeManager.masterNode = new MeshInstance(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), false, 
            new Node[]
            {
                //new MeshInstance3D(new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), false, new List<Node>(), null, new Color4(111,3,3,255) ), 
            }.ToList(), 
            null, new Color4(111, 222, 3, 255));
            base.OnLoad();
        }
    }
}
