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

        float aspectRatio;
        bool isActive;
        Vector3 _front = -Vector3.UnitZ;

        Vector3 _up = Vector3.UnitY;

        Vector3 _right = Vector3.UnitX;
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
        }        // We convert from degrees to radians as soon as the property is set to improve performance.
        public float Pitch
        {
            get => MathHelper.RadiansToDegrees(_pitch);
            set
            {
                // We clamp the pitch value between -89 and 89 to prevent the camera from going upside down, and a bunch
                // of weird "bugs" when you are using euler angles for rotation.
                // If you want to read more about this you can try researching a topic called gimbal lock
                var angle = MathHelper.Clamp(value, -89f, 89f);
                _pitch = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }

        // We convert from degrees to radians as soon as the property is set to improve performance.
        public float Yaw
        {
            get => MathHelper.RadiansToDegrees(_yaw);
            set
            {
                _yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }

        private void UpdateVectors()
        {
            // First, the front matrix is calculated using some basic trigonometry.
            _front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
            _front.Y = MathF.Sin(_pitch);
            _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);

            // We need to make sure the vectors are all normalized, as otherwise we would get some funky results.
            _front = Vector3.Normalize(_front);

            // Calculate both the right and the up vector using cross product.
            // Note that we are calculating the right from the global up; this behaviour might
            // not be what you need for all cameras so keep this in mind if you do not want a FPS camera.
            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(position, position + _front, _up);
        }

        public Matrix4 GetProjectionMatrix()
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




        private Vector2 _lastPos;
        private bool _firstLoop = true;
        private float _sensitivity = 1;
        public override void onUpdate(double deltaTime)
        {
            float speed = 1;
            var input = NodeManager.game.KeyboardState;

            if (input.IsKeyDown(Keys.W))
            {
                position += _front * speed * (float)deltaTime; //Forward 
            }

            if (input.IsKeyDown(Keys.S))
            {
                position -= _front * speed * (float)deltaTime; //Backwards
            }

            if (input.IsKeyDown(Keys.A))
            {
                position -= Vector3.Normalize(Vector3.Cross(_front, _up)) * speed * (float)deltaTime; //Left
            }

            if (input.IsKeyDown(Keys.D))
            {
                position += Vector3.Normalize(Vector3.Cross(_front, _up)) * speed * (float)deltaTime; //Right
            }

            if (input.IsKeyDown(Keys.Space))
            {
                position += _up * speed * (float)deltaTime; //Up 
            }

            if (input.IsKeyDown(Keys.LeftShift))
            {
                position -= _up * speed * (float)deltaTime; //Down
            }

            // Get the mouse state
            var mouse = NodeManager.game.MouseState;

            if (_firstLoop) // This bool variable is initially set to true.
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstLoop = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);
                Yaw += deltaX * _sensitivity;
                Pitch -= deltaY * _sensitivity;
            }
        }
    }
}
