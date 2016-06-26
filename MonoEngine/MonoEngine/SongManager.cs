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
    class SongManager : GameComponent
    {
        private Dictionary<string, Song> songList = new Dictionary<string, Song>();

        public static float maxVolume;

        private static SongManager instance;

        public static SongManager Instance(Game game)
        {
            instance = (instance == null) ? new SongManager(game) : instance;

            return instance;
        }

        private SongManager(Game game) : base(game)
        {
        }
        //Load a song from Content and add it to the list
        public static void LoadSong(string songName)
        {
            instance.songList.Add(songName, ContentHelper.Content.Load<Song>("Assets/Music/" + songName));
        }
        //Play the song by the name provided
        public static void PlaySong(string songName, bool shouldLoop)
        {
            MediaPlayer.Play(instance.songList[songName]);
            //loop the song when its over
            if (shouldLoop)
            {
                MediaPlayer.IsRepeating = true;
            }
            else
            {
                MediaPlayer.IsRepeating = false;
            }
        }
        //stop the current song playing
        public static void StopSong()
        {
            MediaPlayer.Stop();
        }
        //pause the current song playing 
        public static void PauseSong()
        {
            MediaPlayer.Pause();
        }
        //resume the current song from the position it was paused at
        public static void ResumeSong()
        {
            MediaPlayer.Resume();
        }
        public static void SetSongVolume(float volumeAmount)
        {
            //assure its a value between 0 and 1
            MathHelper.Clamp(volumeAmount, 0f, 1f);

            MediaPlayer.Volume = volumeAmount;
        }
        //set the max volume you wish for the current
        //used to fade between the current song volume and the max volume desired
        //if the song wont be doing fading just use SetSongVolume method instead
        public static float SetMaxVolume(float maxAmount)
        {
            //assure its a value between 0 and 1
            MathHelper.Clamp(maxAmount, 0f, 1f);

            maxVolume = maxAmount;

            return maxAmount;
        }
        //fade out the current song playing 
        public static void FadeOutSong()
        {
            MediaPlayer.Volume -= 0.005f;

            MathHelper.Clamp(MediaPlayer.Volume, 0f, 1f);
        }
        //fade in the current song playing
        public static void FadeInSong()
        {
            MediaPlayer.Volume += 0.005f;

            //assure that the volume of the song will not be any louder than the amount specifed in the SetMaxVolume method 
            MathHelper.Clamp(MediaPlayer.Volume, 0f, SetMaxVolume(maxVolume));
        }
    }
}
