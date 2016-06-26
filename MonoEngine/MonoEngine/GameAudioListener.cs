using MonoEngine.Game;
using Microsoft.Xna.Framework.Audio;

namespace MonoEngine
{
    namespace Audio
    {
        public class GameAudioListener : GameObjectComponent
        {

            public AudioListener audioListener;

            private GameAudioListener(GameObject parent) : base(parent)
            {
                audioListener = new AudioListener();
                audioListener.Position = parent.transform.Position;

                SoundManager.AddAudioListener(this);
            }
        }
    }
}
