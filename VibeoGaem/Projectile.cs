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
        Entity owner;

        public Projectile(Color4 color, float speed, Vector3 direction, Vector3 position, Vector3 rotation, Vector3 scale, bool inheritTransform, List<Node> children, Entity? parent) : base(position, rotation, scale, inheritTransform, children, parent)
        {
            id = 1;
            ignoreIds.Add(id);
            mesh = new MeshInstance("assets/projectile.obj", "assets/plain.png", position, rotation, scale, false, new List<Node>(), null, color);
            this.children = new Node[]
            {
                mesh
            }.ToList();
            this.direction = direction.Normalized() * speed;
            owner = parent;
        }

        public override void onUpdate(double deltaTime)
        {
            move(direction * (float)deltaTime);
            mesh.position = position;
            mesh.rotation = rotation;
            base.onUpdate(deltaTime);
        }

        public override void resolveCollision(Collider collider, Vector3 originalDirection, bool continueMoving)
        {
            if (collider is Entity && collider != owner)
            {
                Entity entity = (Entity)collider;
                entity.takeDamage(1, owner);
                if (owner is Player)
                {
                    ((Player)owner).addScore(10);
                }
                NodeManager.game.queueDeleteNode(this);
            }
            else if (collider is Projectile)
            {

            }
            else
            {
                base.resolveCollision(collider, originalDirection, continueMoving);
            }
        }
    }
}
