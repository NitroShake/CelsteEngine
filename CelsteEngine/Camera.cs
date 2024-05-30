using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelsteEngine
{
    public class Camera : Node3D
    {
        public bool debugControl = false;
        public Camera(Vector3 position, Vector3 rotation, Vector3 scale, bool inheritTransform, List<Node> children, Node? parent, float aspectRatio, bool startActive)
            : base(position, rotation, scale, inheritTransform, children, parent)
        {
            this.aspectRatio = aspectRatio;
            this.isActive = startActive;
            if (startActive)
            {
                NodeManager.activeCamera = this;
            }
        }

        public float aspectRatio;
        bool isActive;
        Vector3 forward = -Vector3.UnitZ;
        Vector3 up = Vector3.UnitY;
        Vector3 right = Vector3.UnitX;
        
        //this is how much i hate radians
        private float _pitch;
        private float _yaw;
        private float _fov = 1;
        public float fov
        {
            get => MathHelper.RadiansToDegrees(_fov);
            set
            {
                var angle = MathHelper.Clamp(value, 1f, 90f);
                _fov = MathHelper.DegreesToRadians(angle);
            }
        }
        public float pitch
        {
            get => MathHelper.RadiansToDegrees(_pitch);
            set
            {
                var angle = MathHelper.Clamp(value, -90f, 90f);
                _pitch = MathHelper.DegreesToRadians(angle);
                updateVectors();
            }
        }


        public float yaw
        {
            get => MathHelper.RadiansToDegrees(_yaw);
            set
            {
                _yaw = MathHelper.DegreesToRadians(value);
                updateVectors();
            }
        }

        private void updateVectors()
        {
            forward.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
            forward.Y = MathF.Sin(_pitch);
            forward.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);

            forward = Vector3.Normalize(forward);
            right = Vector3.Normalize(Vector3.Cross(forward, Vector3.UnitY));
            up = Vector3.Normalize(Vector3.Cross(right, forward));
        }

        public Matrix4 getViewMatrix()
        {
            return Matrix4.LookAt(position, position + forward, up);
        }

        public Matrix4 getProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(_fov, aspectRatio, 0.01f, 100f);
        }

        Vector3 rotationInRadians(Vector3 rotation)
        {
            Vector3 vector;
            vector.X = MathHelper.DegreesToRadians(rotation.X);
            vector.Y = MathHelper.DegreesToRadians(rotation.Y);
            vector.Z = MathHelper.DegreesToRadians(rotation.Z);
            return vector;
        }




        private Vector2 lastMousePos;
        private bool firstLoop = true;
        private float controlSensitivity = 0.5f;
        public override void onUpdate(double deltaTime)
        {
            if (debugControl)
            {
                float speed = 1;
                var input = NodeManager.game.KeyboardState;

                if (input.IsKeyDown(Keys.W))
                {
                    position += forward * speed * (float)deltaTime; //Forward 
                }

                if (input.IsKeyDown(Keys.S))
                {
                    position -= forward * speed * (float)deltaTime;
                }

                if (input.IsKeyDown(Keys.A))
                {
                    position -= Vector3.Normalize(Vector3.Cross(forward, up)) * speed * (float)deltaTime;
                }

                if (input.IsKeyDown(Keys.D))
                {
                    position += Vector3.Normalize(Vector3.Cross(forward, up)) * speed * (float)deltaTime;
                }

                if (input.IsKeyDown(Keys.Space))
                {
                    position += up * speed * (float)deltaTime;
                }

                if (input.IsKeyDown(Keys.LeftShift))
                {
                    position -= up * speed * (float)deltaTime;
                }

                var mouse = NodeManager.game.MouseState;

                if (firstLoop)
                {
                    lastMousePos = new Vector2(mouse.X, mouse.Y);
                    firstLoop = false;
                }
                else
                {
                    var deltaX = mouse.X - lastMousePos.X;
                    var deltaY = mouse.Y - lastMousePos.Y;
                    lastMousePos = new Vector2(mouse.X, mouse.Y);
                    yaw += deltaX * controlSensitivity;
                    pitch -= deltaY * controlSensitivity;
                }
            }
        }
    }
}
