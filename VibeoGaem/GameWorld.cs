using CelsteEngine;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeoGaem
{
    internal class GameWorld : Node3D
    {
        Player player;

        Random random;
        double entityTimer = 0;
        double entityMaxTimer = 10;

        double asteroidTimer = 0;
        double asteroidMaxTimer = 3;


        public GameWorld() : base(new(), new(), new(), false)
        {
            AssetManager.loadMesh("assets/asteroid.obj");
            AssetManager.loadTexture("assets/asteroiddiffuse.png");
            random = new Random();
            player = new Player(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1f, 1f, 1f), false, new List<Node>(), null);
            addChild(new DirectionalLight(true, 0.3f, 0.75f, 0.2f, new Vector3(90, 0, 0), null, null));
            addChild(new MeshInstance("assets/background.obj", "assets/background.png", new Vector3(0, -10, 0), new Vector3(0, 0, 0),new Vector3(1,1,1), false, null, null, new Color4(255,255,255,255)));
            addChild(player);
        }

        public override void onUpdate(double deltaTime)
        {
            base.onUpdate(deltaTime);
            entityMaxTimer = Math.Max(6 - Math.Log(player.score + 10), 0);
            asteroidMaxTimer = Math.Max(5 - Math.Log(player.score + 10), 0);

            entityTimer += deltaTime;
            if (entityTimer > entityMaxTimer)
            {
                Vector3 offset = new Vector3((float)(random.NextDouble() - 0.5), 0, (float)(random.NextDouble() - 0.5)).Normalized();
                offset *= random.Next(10, 20);
                NodeManager.masterNode.addChild(new Enemy(player, player.position + offset, new(), new Vector3(1,1,1), false, new(), null));
                entityTimer = 0;
            }

            asteroidTimer += deltaTime;
            if (asteroidTimer > asteroidMaxTimer)
            {
                Vector3 offset = new Vector3((float)(random.NextDouble() - 0.5), 0, (float)(random.NextDouble() - 0.5)).Normalized();
                offset *= random.Next(10, 20);
                addChild(new Asteroid(player.position + offset, new(), new(), false, new(), null));
                asteroidTimer = 0;
            }
        }
    }
}
