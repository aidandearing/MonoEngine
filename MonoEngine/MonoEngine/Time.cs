using Microsoft.Xna.Framework;

namespace MonoEngine
{
    public class Time : GameComponent
    {
        private float deltaTime;

        public static float DeltaTime
        {
            get
            {
                return instance.deltaTime;
            }

            internal set
            {
                instance.deltaTime = value;
            }
        }

        private float elapsedTime;

        public static float ElapsedTime
        {
            get
            {
                return instance.elapsedTime;
            }

            internal set
            {
                instance.elapsedTime = value;
            }
        }

        private static Time instance;

        private Time(Microsoft.Xna.Framework.Game game) : base(game)
        {

        }

        public static Time Instance(Microsoft.Xna.Framework.Game game)
        {
            instance = (instance == null) ? new Time(game) : instance;
            return instance;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            deltaTime = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            elapsedTime = gameTime.TotalGameTime.Milliseconds / 1000.0f;
        }
    }
}
