using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelsteEngine
{
    public class Camera : Node3D
    {
        public Camera(Vector3 position, Vector3 rotation, Vector3 scale, bool inheritTransform, List<Node> children, Node? parent, float aspectRatio, bool startActive)
            : base(position, rotation, scale, inheritTransform, children, parent)
        {
            this.aspectRatio = aspectRatio;
            this.isActive = startActive;
        }

        float aspectRatio;
        bool isActive;
        private Vector3 _front = -Vector3.UnitZ;

        private Vector3 _up = Vector3.UnitY;

        private Vector3 _right = Vector3.UnitX;


        public float fov
        {
            get => MathHelper.RadiansToDegrees(fov);
            set
            {
                var angle = MathHelper.Clamp(value, 1f, 90f);
                fov = MathHelper.DegreesToRadians(angle);
            }
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(position, position + _front, _up);
        }

        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(fov, aspectRatio, 0.01f, 100f);
        }

        Vector3 rotationInRadians(Vector3 rotation)
        {
            Vector3 vector;
            vector.X = MathHelper.DegreesToRadians(rotation.X);
            vector.Y = MathHelper.DegreesToRadians(rotation.Y);
            vector.Z = MathHelper.DegreesToRadians(rotation.Z);
            return vector;
        }


        public override void onUpdate(double deltaTime)
        {
            
        }
    }
}
