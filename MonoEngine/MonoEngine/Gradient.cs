using Microsoft.Xna.Framework;

namespace MonoEngine
{
    public class Gradient
    {
        public Color[] colours;
        public float[] times;

        public Gradient(Color start, Color end)
        {
            colours = new Color[2] { start, end };
            times = new float[2] { 0.0f, 1.0f };
        }

        public Gradient(Color[] colours, float[] times)
        {
            this.colours = colours;
            this.times = times;
        }

        public Color Evaluate(float amount)
        {
            // Go across the entire list of points and times, find the nearest lower, and nearest higher

            Color colour_lower = colours[0];
            float time_lower = times[0];
            Color colour_higher = colours[colours.Length - 1];
            float time_higher = times[times.Length - 1];

            for (int i = 0; i < times.Length; i++)
            {
                if (times[i] < amount && times[i] > time_lower)
                {
                    colour_lower = colours[i];
                    time_lower = times[i];
                }
                else if (times[i] > amount && times[i] <= time_higher)
                {
                    colour_higher = colours[i];
                    time_higher = times[i];
                }
            }

            return Color.LerpPrecise(colour_lower, colour_higher, (amount - time_lower) / (time_higher - time_lower));
        }
    }
}
