﻿using System;
using Microsoft.Xna.Framework.Media;

namespace SolStandard.Utility.Monogame
{
    public static class MusicBox
    {
        private const float MaxVolume = 1f;
        private const float MinVolume = 0f;
        public static bool Muted { get; private set; }
        private static Song _currentSong;
        private static float _currentVolume;
        public static float CurrentVolume => Convert.ToInt32(_currentVolume * 100);

        public static void ToggleMute()
        {
            Muted = !Muted;

            if (Muted)
            {
                MediaPlayer.Pause();
            }
            else
            {
                Play(_currentSong, _currentVolume);
            }
        }

        public static void Play(Song song, float volume = 1f)
        {
            _currentSong = song;
            _currentVolume = volume;
            if (Muted) return;

            MediaPlayer.Play(_currentSong);
            MediaPlayer.Volume = _currentVolume;
            MediaPlayer.IsRepeating = false;
        }

        public static void PlayLoop(Song song, float volume = 1f)
        {
            _currentSong = song;
            _currentVolume = volume;
            if (Muted) return;

            MediaPlayer.Play(_currentSong);
            MediaPlayer.Volume = _currentVolume;
            MediaPlayer.IsRepeating = true;
        }

        public static void IncreaseVolume(float increasedBy)
        {
            if (_currentVolume + increasedBy > MaxVolume)
            {
                _currentVolume = MaxVolume;
            }
            else
            {
                _currentVolume += increasedBy;
            }

            MediaPlayer.Volume = _currentVolume;
        }

        public static void ReduceVolume(float reducedBy)
        {
            if (_currentVolume - reducedBy < MinVolume)
            {
                _currentVolume = MinVolume;
            }
            else
            {
                _currentVolume -= reducedBy;
            }

            MediaPlayer.Volume = _currentVolume;
        }

        public static void Stop()
        {
            MediaPlayer.Stop();
        }

        public static void Pause()
        {
            MediaPlayer.Pause();
        }
    }
}