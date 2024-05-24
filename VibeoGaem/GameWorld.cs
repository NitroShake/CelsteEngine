﻿using CelsteEngine;
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
            addChild(player);
        }

        public override void onUpdate(double deltaTime)
        {
            base.onUpdate(deltaTime);
            entityMaxTimer = Math.Log(player.score + 10);
            asteroidMaxTimer = Math.Log(player.score + 10);

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