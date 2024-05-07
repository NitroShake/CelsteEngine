using CelsteEngine;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeoGaem
{
    class Entity : SphereCollider
    {
        public float health;


        public Entity(Vector3 position, Vector3 rotation, Vector3 scale, bool inheritTransform, List<Node> children, Node? parent) : base(position, rotation, scale, inheritTransform, children, parent)
        {

        }

        public void takeDamage(float damage)
        {
            health -= damage;
            if (health <= 0)
            {
                NodeManager.game.queueDeleteNode(this);
            }
        }

        public override void onUpdate(double deltaTime)
        {
            base.onUpdate(deltaTime);
        }
    }
}
