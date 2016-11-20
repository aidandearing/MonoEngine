using MonoEngine;
using MonoEngine.Game;
using MonoEngine.Render;
using MonoEngine.Physics.Physics2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace TestbedMonogame
{
    public class CameraController : GameObject
    {
        public static float MoveSpeed = 300;

        public float MouseDelta;

        public Camera camera;

        PlayerIndex index;

        public CameraController(string name, PlayerIndex index) : base(name)
        {
            this.index = index;
        }

        public override void Update()
        {
            // Get input, and use it to control this player
            bool keyboardControlled = false;
            Vector3 desiredVelocity = Vector3.Zero;

            if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                // Player wants to go up
                keyboardControlled = true;
                desiredVelocity += new Vector3(0, -1, 0) * MoveSpeed;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                // Player wants to go down
                keyboardControlled = true;
                desiredVelocity += new Vector3(0, 1, 0) * MoveSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                // Player wants to go left
                keyboardControlled = true;
                desiredVelocity += new Vector3(1, 0, 0) * MoveSpeed;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                // Player wants to go right
                keyboardControlled = true;
                desiredVelocity += new Vector3(-1, 0, 0) * MoveSpeed;
            }

            float delta = Mouse.GetState().ScrollWheelValue - MouseDelta;
            float zoom = MathHelper.Clamp((Mouse.GetState().ScrollWheelValue + 2000.0f) / 1000.0f, 1.0f, 5.0f);
            camera.Projection = Matrix.CreateOrthographic(GraphicsHelper.screen.Width * zoom , GraphicsHelper.screen.Height * zoom, -10000, 10000);

            if (!keyboardControlled)
            {
                // Gamepad logic
            }

            if (desiredVelocity.LengthSquared() > 0)
            {
                //body.transform.parent.Translate(Vector3.Normalize(desiredVelocity) * MoveSpeed * Time.DeltaTime);
                camera.view.Translation += Vector3.Normalize(desiredVelocity) * MoveSpeed * Time.DeltaTime;
            }

            MouseDelta = Mouse.GetState().ScrollWheelValue;
        }
    }
}

