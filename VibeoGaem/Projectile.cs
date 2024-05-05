using CelsteEngine;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeoGaem
{
    internal class Projectile : SphereCollider
    {
        Vector3 direction;
        MeshInstance mesh;

        public Projectile(Vector3 direction, Vector3 position, Vector3 rotation, Vector3 scale, bool inheritTransform, List<Node> children, Node? parent) : base(position, rotation, scale, inheritTransform, children, parent)
        {
            mesh = new MeshInstance("testassets/testcone.obj", "testassets/test2.png", position, rotation, scale, false, new List<Node>(), null, new Color4(255, 255, 255, 255));
            this.children = new Node[]
            {
                mesh
            }.ToList();
            this.direction = direction.Normalized() * 0.01f;
        }

        public override void onUpdate(double deltaTime)
        {
            move(direction);
            mesh.position = position;
            mesh.rotation = rotation;
            base.onUpdate(deltaTime);
        }

        public override void resolveCollision(Collider collider, Vector3 originalDirection, bool continueMoving)
        {
            if (collider is Entity && collider is not Player)
            {
                Entity entity = (Entity)collider;
                entity.takeDamage(1);
                NodeManager.game.queueDeleteNode(this);
            }
            else
            {
                base.resolveCollision(collider, originalDirection, continueMoving);
            }
        }
    }
}
