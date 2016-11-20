using Microsoft.Xna.Framework;

namespace MonoEngine
{
    public class Random : GameComponent
    {
        public static int Seed = -1;

        private System.Random random;

        private static Random instance;

        private Random(Microsoft.Xna.Framework.Game game) : base(game)
        {
            random = new System.Random(Seed);
        }

        public static Random Instance(Microsoft.Xna.Framework.Game game)
        {
            instance = (instance == null) ? new Random(game) : instance;
            return instance;
        }

        public static float Range()
        {
            return (float)instance.random.NextDouble();
        }

        public static float Range(float max)
        {
            return (float)instance.random.NextDouble() * max;
        }

        public static float Range(float min, float max)
        {
            return (float)(min + instance.random.NextDouble() * (max - min));
        }
    }
}
