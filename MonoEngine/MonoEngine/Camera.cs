using Microsoft.Xna.Framework;
using System;

namespace Capstone
{
    class Camera : GameObjectComponent, IGameObjectUpdatable
    {
        // TODO A variety of safer static methods to handle main camera and other camera based stuff
        public static Camera MainCamera;

        public Camera(GameObject parent) : base(parent)
        {
            // Isometric stuff
            // Making the camera view matrix rotated properly for the 45 degree arctan(1/sqrt(2)) stuffs that is necessary to create the appearance of 120 degrees between any 2 axis that is necessary for isometric
            Vector3 position = new Vector3((float)Math.Cos(Math.PI / 4.0f), (float)Math.Atan(1.0f / Math.Sqrt(2.0f)), (float)Math.Sin(Math.PI / 4.0f));
            Vector3 lookAt = new Vector3(0, 0, 0);
            view = Matrix.CreateLookAt(position, lookAt, Vector3.Up);

            // Standard orthographic projection
            projection = Matrix.CreateOrthographic(GraphicsHelper.screen.Width, GraphicsHelper.screen.Height, -1000, 1000);

            if (MainCamera == null)
                MainCamera = this;
        }

        private Matrix view;
        public Matrix View
        {
            get
            {
                return view;
            }
        }
        private Matrix projection;
        public Matrix Projection
        {
            get
            {
                return projection;
            }
        }

        void IGameObjectUpdatable.Update()
        {

        }

        // TODO probably want to build raycast stuff for cameras at some point in the eventual future
    }
}
