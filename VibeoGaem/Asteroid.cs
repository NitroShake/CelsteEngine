using CelsteEngine;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenTK.Graphics.OpenGL.GL;

namespace VibeoGaem
{
    class Asteroid : Entity
    {
        MeshInstance mesh;
        Vector3 direction;
        Vector3 meshDeltaRotation;
        public Asteroid(Vector3 position, Vector3 rotation, Vector3 scale, bool inheritTransform, List<Node> children, Node? parent) : base(position, rotation, scale, inheritTransform, children, parent)
        {
            mesh = new MeshInstance("assets/asteroidtest.obj", "assets/asteroiddiffuse.png", new Vector3(5, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), false, new List<Node>(), null, new Color4(255, 255, 255, 255));
            addChild(mesh);
            health = 10;
            Random random = new Random();
            direction = new Vector3((float)(random.NextDouble() - 0.5), 0, (float)(random.NextDouble() - 0.5)) * 1;
            meshDeltaRotation = new Vector3((float)(random.NextDouble() - 0.5), 0, (float)(random.NextDouble() - 0.5)) * 1;
        }

        public override void onUpdate(double deltaTime)
        {
            move(direction * (float)deltaTime);
            mesh.position = position;
            mesh.rotation += meshDeltaRotation * (float)deltaTime;
            base.onUpdate(deltaTime);
        }
    }
}
