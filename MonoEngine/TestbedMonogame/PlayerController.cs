using MonoEngine;
using MonoEngine.Game;
using MonoEngine.Physics.Physics2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestbedMonogame
{
    public class PlayerController : GameObject
    {
        public static float MoveSpeed = 3;

        public PhysicsBody2D body;

        PlayerIndex index;

        public PlayerController(string name, PlayerIndex index) : base(name)
        {
            this.index = index;
        }

        public override void Update()
        {
            if (body == null)
            {
                body = GetComponent(new PhysicsBody2D().GetType()) as PhysicsBody2D;
            }

            // Get input, and use it to control this player
            bool keyboardControlled = false;
            Vector3 desiredVelocity = Vector3.Zero;

            if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                // Player wants to go up
                keyboardControlled = true;
                desiredVelocity += new Vector3(0, 0, -1) * MoveSpeed;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                // Player wants to go down
                keyboardControlled = true;
                desiredVelocity += new Vector3(0, 0, 1) * MoveSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                // Player wants to go left
                keyboardControlled = true;
                desiredVelocity += new Vector3(-1, 0, 0) * MoveSpeed;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                // Player wants to go right
                keyboardControlled = true;
                desiredVelocity += new Vector3(1, 0, 0) * MoveSpeed;
            }

            if (!keyboardControlled)
            {
                // Gamepad logic
            }

            if (desiredVelocity.LengthSquared() > 0)
            {    
                //body.transform.parent.Translate(Vector3.Normalize(desiredVelocity) * MoveSpeed * Time.DeltaTime);
                body.Velocity = Vector3.Normalize(desiredVelocity) * MoveSpeed * Time.DeltaTime;
            }
            else
            {
                body.Velocity = Vector3.Zero;
            }
        }
    }
}
