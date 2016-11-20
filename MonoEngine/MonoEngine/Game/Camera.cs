using Microsoft.Xna.Framework;
using System;
using MonoEngine.Render;

namespace MonoEngine.Game
{
    public class Camera : GameObject
    {
        // TODO A variety of safer static methods to handle main camera and other camera based stuff
        public static Camera MainCamera;

        public Camera() { }

        public Camera(string name, Matrix view, Matrix projection) : base(name)
        {
            // Isometric stuff
            // Making the camera view matrix rotated properly for the 45 degree arctan(1/sqrt(2)) stuffs that is necessary to create the appearance of 120 degrees between any 2 axis that is necessary for isometric
            //Vector3 position = new Vector3((float)Math.Cos(Math.PI / 4.0f), (float)Math.Atan(1.0f / Math.Sqrt(2.0f)), (float)Math.Sin(Math.PI / 4.0f));
            //Vector3 lookAt = new Vector3(0, 0, 0);
            this.view = view;//Matrix.CreateLookAt(position, lookAt, Vector3.Up);

            // Standard orthographic projection
            this.projection = projection;//Matrix.CreateOrthographic(GraphicsHelper.screen.Width, GraphicsHelper.screen.Height, -1000, 1000);

            if (MainCamera == null)
                MainCamera = this;
        }

        public Matrix view;
        public Matrix View
        {
            get
            {
                return view;
            }
            set
            {
                view = value;
            }
        }
        private Matrix projection;
        public Matrix Projection
        {
            get
            {
                return projection;
            }
            set
            {
                projection =  value;
            }
        }

        public void Translate(Vector3 translation)
        {
            view.Translation += translation;
        }

        // TODO Some static methods for instantiating different kinds of cameras
        public static Camera Orthographic(string name, Vector3 position, Vector3 lookat)
        {
            return new Camera(name, Matrix.CreateLookAt(position, lookat, Vector3.Up), Matrix.CreateOrthographic(GraphicsHelper.screen.Width, GraphicsHelper.screen.Height, 0.01f, 1000f));
        }

        public static Camera Orthographic(string name, Vector3 position, Vector3 lookat, int width, int height)
        {
            return new Camera(name, Matrix.CreateLookAt(position, lookat, Vector3.Up), Matrix.CreateOrthographic(width, height, 0.01f, 1000f));
        }

        public static Camera Orthographic(string name, Vector3 position, Vector3 lookat, float near, float far)
        {
            return new Camera(name, Matrix.CreateLookAt(position, lookat, Vector3.Up), Matrix.CreateOrthographic(GraphicsHelper.screen.Width, GraphicsHelper.screen.Height, near, far));
        }

        public static Camera Orthographic(string name, Vector3 position, Vector3 lookat, int width, int height, float near, float far)
        {
            return new Camera(name, Matrix.CreateLookAt(position, lookat, Vector3.Up), Matrix.CreateOrthographic(width, height, near, far));
        }

        public static Camera Isometric(string name, Vector3 lookat, float distance)
        {
            return new Camera(name, Matrix.CreateLookAt(lookat + new Vector3((float)Math.Cos(Math.PI / 4.0f), (float)Math.Atan(1.0f / Math.Sqrt(2.0f)), (float)Math.Sin(Math.PI / 4.0f)) * distance, lookat, Vector3.Up), Matrix.CreateOrthographic(GraphicsHelper.screen.Width * distance, GraphicsHelper.screen.Height * distance, 0.01f, 1000f));
        }

        public static Camera Isometric(string name, Vector3 lookat, float distance, int width, int height)
        {
            return new Camera(name, Matrix.CreateLookAt(lookat + new Vector3((float)Math.Cos(Math.PI / 4.0f), (float)Math.Atan(1.0f / Math.Sqrt(2.0f)), (float)Math.Sin(Math.PI / 4.0f)) * distance, lookat, Vector3.Up), Matrix.CreateOrthographic(width * distance, height * distance, 0.01f, 1000f));
        }

        public static Camera Isometric(string name, Vector3 lookat, float distance, float near, float far)
        {
            return new Camera(name, Matrix.CreateLookAt(lookat + new Vector3((float)Math.Cos(Math.PI / 4.0f), (float)Math.Atan(1.0f / Math.Sqrt(2.0f)), (float)Math.Sin(Math.PI / 4.0f)) * distance, lookat, Vector3.Up), Matrix.CreateOrthographic(GraphicsHelper.screen.Width * distance, GraphicsHelper.screen.Height * distance, near, far));
        }

        public static Camera Isometric(string name, Vector3 lookat, float distance, int width, int height, float near, float far)
        {
            return new Camera(name, Matrix.CreateLookAt(lookat + new Vector3((float)Math.Cos(Math.PI / 4.0f), (float)Math.Atan(1.0f / Math.Sqrt(2.0f)), (float)Math.Sin(Math.PI / 4.0f)) * distance, lookat, Vector3.Up), Matrix.CreateOrthographic(width * distance, height * distance, near, far));
        }

        public static Camera Perspective(string name, Vector3 position, Vector3 lookat)
        {
            return new Camera(name, Matrix.CreateLookAt(position, lookat, Vector3.Up), Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60f), GraphicsHelper.screen.Width / GraphicsHelper.screen.Height, 0.01f, 1000f));
        }

        public static Camera Perspective(string name, Vector3 position, Vector3 lookat, float fieldofview)
        {
            return new Camera(name, Matrix.CreateLookAt(position, lookat, Vector3.Up), Matrix.CreatePerspectiveFieldOfView(fieldofview, GraphicsHelper.screen.Width / GraphicsHelper.screen.Height, 0.01f, 1000f));
        }

        public static Camera Perspective(string name, Vector3 position, Vector3 lookat, float near, float far)
        {
            return new Camera(name, Matrix.CreateLookAt(position, lookat, Vector3.Up), Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60f), GraphicsHelper.screen.Width / GraphicsHelper.screen.Height, near, far));
        }

        public static Camera Perspective(string name, Vector3 position, Vector3 lookat, float fieldofview, float near, float far)
        {
            return new Camera(name, Matrix.CreateLookAt(position, lookat, Vector3.Up), Matrix.CreatePerspectiveFieldOfView(fieldofview, GraphicsHelper.screen.Width / GraphicsHelper.screen.Height, near, far));
        }

        // TODO probably want to build raycast stuff for cameras at some point in the eventual future
    }
}
