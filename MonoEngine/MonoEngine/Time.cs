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
        }

        private float deltaTime_last;

        public static float LastDeltaTime
        {
            get
            {
                return instance.deltaTime_last;
            }
        }

        private float elapsedTime;

        public static float ElapsedTime
        {
            get
            {
                return instance.elapsedTime;
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

            deltaTime_last = deltaTime;
            deltaTime = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            elapsedTime += deltaTime;
        }
    }
}
