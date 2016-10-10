using MonoEngine.Game;
using Microsoft.Xna.Framework.Audio;

namespace MonoEngine.Audio
{
    public class AudioListener : GameObject
    {
        public Microsoft.Xna.Framework.Audio.AudioListener audioListener;

        private AudioListener(string name) : base(name)
        {
            audioListener = new Microsoft.Xna.Framework.Audio.AudioListener();

            SoundManager.AddAudioListener(this);
        }
    }
}
