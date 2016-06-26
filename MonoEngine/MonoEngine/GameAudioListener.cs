using MonoEngine.Game;
using Microsoft.Xna.Framework.Audio;

namespace MonoEngine
{
    namespace Audio
    {
        public class GameAudioListener : GameObject
        {

            public AudioListener audioListener;

            private GameAudioListener(string name) : base(name)
            {
                audioListener = new AudioListener();
                audioListener.Position = parent.transform.Position;

                SoundManager.AddAudioListener(this);
            }
        }
    }
}
