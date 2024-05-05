using CelsteEngine;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeoGaem
{
    internal class PlayerCamera : Camera
    {
        Node3D targetPlayer;
        public PlayerCamera(Vector3 position, Vector3 rotation, Vector3 scale, bool inheritTransform, List<Node> children, Node? parent, float aspectRatio, bool startActive, Node3D targetPlayer) : base(position, rotation, scale, inheritTransform, children, parent, aspectRatio, startActive)
        {
            this.targetPlayer = targetPlayer;
        }

        void trackPlayer()
        {
            position = targetPlayer.position;
            position.X -= 1f;
            position.Y += 20;
            Pitch = -90f;

        }

        public override void onUpdate(double deltaTime)
        {
            debugControl = false;
            trackPlayer();
            base.onUpdate(deltaTime);
        }
    }
}
