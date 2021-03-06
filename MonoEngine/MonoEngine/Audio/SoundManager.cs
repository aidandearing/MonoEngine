﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace MonoEngine.Audio
{
    public class SoundManager : GameComponent
    {
        private Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

        private static List<AudioListener> audioListeners;

        private static SoundManager instance;

        public static SoundManager Instance(Microsoft.Xna.Framework.Game game)
        {
            instance = (instance == null) ? new SoundManager(game) : instance;

            return instance;
        }

        private SoundManager(Microsoft.Xna.Framework.Game game) : base(game)
        {

        }

        //Load a soundeffect from content and add it to the list
        public static void LoadSound(string soundName)
        {
            instance.sounds.Add(soundName, ContentHelper.Content.Load<SoundEffect>("Assets/SoundEffects/" + soundName));
        }

        public static void PlaySound(string soundName)
        {
            instance.sounds[soundName].CreateInstance().Play();
        }

        //play a sound and set the volume, panning and pitch 
        public static void PlaySound(string soundName, float volume, float panAmount, float pitch)
        {
            SoundEffectInstance sfi = instance.sounds[soundName].CreateInstance();

            MathHelper.Clamp(volume, 0f, 1f);

            MathHelper.Clamp(panAmount, -1f, 1f);

            MathHelper.Clamp(pitch, -1f, 1f);

            sfi.Volume = volume;
            sfi.Pan = panAmount;
            sfi.Pitch = pitch;
            sfi.Play();
        }

        //play a sound in 3d space
        public static void PlaySound(string soundName, Vector3 position)
        {
            AudioEmitter audioEmitter = new AudioEmitter();

            audioEmitter.Position = position;

            foreach (AudioListener gameAudioListener in audioListeners)
            {
                instance.sounds[soundName].CreateInstance().Apply3D(gameAudioListener.audioListener, audioEmitter);
            }
        }

        public static void AddAudioListener(AudioListener audioListener)
        {
            audioListeners.Add(audioListener);
        }
    }
}
