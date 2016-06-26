using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Capstone
{
    class GameAudioListener : GameObjectComponent
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
