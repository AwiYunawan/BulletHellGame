using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace BulletHellGame.Managers
{
    public class AudioManager
    {
        private static AudioManager _instance;
        public static AudioManager Instance
        {
            get
            {
                _instance ??= new AudioManager();
                return _instance;
            }
        }

        private Dictionary<string, SoundEffect> _soundEffects;
        private Dictionary<string, Song> _songs;
        private ContentManager _content;

        private AudioManager()
        {
            _soundEffects = new Dictionary<string, SoundEffect>();
            _songs = new Dictionary<string, Song>();
        }

        public void Initialize(ContentManager content)
        {
            _content = content;
            LoadAudio();
        }

        private void LoadAudio()
        {
            // Load sound effects
            // _soundEffects["shoot"] = _content.Load<SoundEffect>("Sounds/shoot");
            // _soundEffects["explosion"] = _content.Load<SoundEffect>("Sounds/explosion");
            
            // Load songs
            // _songs["background"] = _content.Load<Song>("Sounds/background");
        }

        public void PlaySound(string soundName)
        {
            if (_soundEffects.ContainsKey(soundName))
            {
                _soundEffects[soundName].Play();
            }
        }

        public void PlaySong(string songName, bool loop = true)
        {
            if (_songs.ContainsKey(songName))
            {
                MediaPlayer.Play(_songs[songName]);
                MediaPlayer.IsRepeating = loop;
            }
        }

        public void StopSong()
        {
            MediaPlayer.Stop();
        }
    }
}
