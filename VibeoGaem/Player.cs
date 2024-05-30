using CelsteEngine;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeoGaem
{
    internal class Player : Entity
    {
        public double score;
        MeshInstance mesh;
        double invulnerableTimer = 1;
        double projectileInterval = 0.33;
        double projectileTimer = 0;

        public Player(Vector3 position, Vector3 rotation, Vector3 scale, bool inheritTransform, List<Node> children, Node? parent) : base(position, rotation, scale, inheritTransform, children, parent)
        {
            health = 10;
            mesh = new OutlinedMeshInstance(new Color4(100, 210, 255, 255), "assets/cone2.obj", "assets/ship.png", new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), false, color: new Color4(255, 255, 255, 255));
            addChild(mesh);
            addChild(new PlayerCamera(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), false, new List<Node>(), this, 1.25f, true, this));
            this.scale = new Vector3(1, 1, 1);
        }

        public override void onUpdate(double deltaTime)
        {
            invulnerableTimer -= deltaTime;
            projectileTimer += deltaTime;
            float speed = 5f * (float)deltaTime;
            var input = NodeManager.game.KeyboardState;

            if (input.IsKeyDown(Keys.LeftShift))
            {
                //speed *= 2;
            }

            Vector3 movement = new Vector3(0, 0, 0);
            if (input.IsKeyDown(Keys.A))
            {
                movement.Z += speed; //Forward 
            }

            if (input.IsKeyDown(Keys.D))
            {
                movement.Z -= speed; //Backwards
            }

            if (input.IsKeyDown(Keys.W))
            {
                movement.X -= speed; //Left
            }

            if (input.IsKeyDown(Keys.S))
            {
                movement.X += speed; //Right
            }

            move(movement);
            base.onUpdate(deltaTime);

            rotation = handleMouseInput();

            mesh.position = position;
            mesh.rotation = rotation;
        }

        Vector3 handleMouseInput()
        {
            float mouseX = NodeManager.game.MousePosition.X / NodeManager.game.Size.X - 0.5f;
            //Console.WriteLine(mouseX);
            mouseX *= NodeManager.game.Size.X;
            //Console.WriteLine(mouseX);
            float mouseY = NodeManager.game.MousePosition.Y / NodeManager.game.Size.Y - 0.5f;
            mouseY *= NodeManager.game.Size.Y;
            if (NodeManager.game.IsMouseButtonDown(MouseButton.Left) && projectileTimer > projectileInterval)
            {
                Vector3 direction = new Vector3(mouseY, 0, -mouseX).Normalized();
                addChild(new Projectile(new Color4(100, 210, 255, 255), 13, direction, position + direction * 1.5f, new Vector3(0,0,0), new Vector3(0.5f,0.5f,0.5f), false, new List<Node>(), null));
                projectileTimer = 0;
            }
            float rotation = (float)(Math.Atan2(mouseY, mouseX));
            //Console.WriteLine(rotation);
            return new Vector3(0,-rotation - (float)Math.PI / 2,0);
        }

        public override void resolveCollision(Collider collider, Vector3 originalDirection, bool continueMoving)
        {
            if (collider is Asteroid)
            {
                takeDamage(1, null);
            }
            if (collider is not Projectile)
            {
                base.resolveCollision(collider, originalDirection, continueMoving);
            }
            else
            {
                move(originalDirection);
            }
        }

        public void addScore(int score)
        {
            this.score += score;
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine();
            }
            Console.WriteLine(Math.Round(this.score));
        }
        public override void takeDamage(float damage, Entity dealer)
        {
            if (invulnerableTimer <= 0)
            {
                base.takeDamage(damage, dealer);
                invulnerableTimer = 2;
            }
            if (health <= 0)
            {
                Console.WriteLine($"You died! Final Score: {score}");
            }
        }
    }
}
