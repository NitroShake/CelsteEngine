using CelsteEngine;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VibeoGaem
{
    internal class Enemy : Entity
    {
        MeshInstance mesh;
        Entity target;
        double projectileInterval = 1;
        double projectileTimer = 0;

        public Enemy(Entity target, Vector3 position, Vector3 rotation, Vector3 scale, bool inheritTransform, List<Node> children, Node? parent) : base(position, rotation, scale, inheritTransform, children, parent)
        {
            health = 5;
            this.target = target;
            mesh = new MeshInstance("assets/cone.obj", "assets/enemE.png", new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), false, color: new Color4(255, 255, 255, 255));
            addChild(mesh);
        }

        public override void onUpdate(double deltaTime)
        {
            base.onUpdate(deltaTime);
            projectileTimer += deltaTime;

            Vector3 directionToTarget = target.position - position;
            directionToTarget.Normalize();
            float newRotation = (float)(Math.Atan2(directionToTarget.X, -directionToTarget.Z));
            rotation = new Vector3(0, -newRotation - (float)Math.PI / 2, 0);

            if (projectileTimer > projectileInterval) 
            {
                addChild(new Projectile("testassets/test2.png", directionToTarget, position + (directionToTarget * 1.5f), new Vector3(0, 0, 0), new Vector3(0.5f, 0.5f, 0.5f), false, new List<Node>(), null));
                projectileTimer = 0;
            }

            mesh.position = position;
            mesh.rotation = rotation;
        }
    }
}
