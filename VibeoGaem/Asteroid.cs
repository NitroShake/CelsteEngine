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
        public Asteroid(Vector3 position, Vector3 rotation, Vector3 scale, bool inheritTransform, List<Node> children, Node? parent) : base(position, rotation, new Vector3(2.25f, 2.25f, 2.25f), inheritTransform, children, parent)
        {
            health = 5;
            mesh = new LitMeshInstance("assets/asteroidtest.obj", "assets/asteroiddiffuse.png", new Vector3(5, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), false, new List<Node>(), null, new Color4(255, 255, 255, 255));
            addChild(mesh);
            Random random = new Random();
            direction = new Vector3((float)(random.NextDouble() - 0.5), 0, (float)(random.NextDouble() - 0.5)).Normalized();
            meshDeltaRotation = new Vector3((float)(random.NextDouble() - 0.5), 0, (float)(random.NextDouble() - 0.5)) * 1;
        }

        public override void onUpdate(double deltaTime)
        {
            move(direction * (float)deltaTime);
            mesh.position = position;
            mesh.rotation += meshDeltaRotation * (float)deltaTime;
            base.onUpdate(deltaTime);
        }

        public override void resolveCollision(Collider collider, Vector3 originalDirection, bool continueMoving)
        {
            if (collider is Player)
            {
                ((Player)collider).takeDamage(1, null);
            }
            if (collider is not Projectile)
            {
                base.resolveCollision(collider, originalDirection, continueMoving);
            }
        }

        public override void takeDamage(float damage, Entity dealer)
        {
            base.takeDamage(damage, dealer);
            if (dealer is Player && health <= 0)
            {
                ((Player)dealer).addScore(50);
            }
        }
    }
}
