﻿using Android.Media;
using Xamarin.Forms;

[assembly: Dependency(typeof(ArtCritic.Droid.AndroidMusicPlayer))]
namespace ArtCritic.Droid
{
    class AndroidMusicPlayer : IMusicPlayer
    {
        MediaPlayer player = new MediaPlayer();
        public AndroidMusicPlayer()
        {
        }

        public void Open(string assetFileName)
        {
            player.Reset();
            var musicFile = global::Android.App.Application.Context.Assets.OpenFd(assetFileName);
            player.SetDataSource(musicFile);
            player.Prepare();
        }

        public void Play()
        {
            player.Start();
        }

        public void Pause()
        {
            player.Pause();
        }

        public void Stop()
        {
            player.Stop();
        }
    }
}