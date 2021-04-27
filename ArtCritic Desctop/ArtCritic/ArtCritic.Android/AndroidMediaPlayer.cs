using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Media;
using Xamarin.Forms;

[assembly: Dependency(typeof(ArtCritic.Droid.AndroidMediaPlayer))]
namespace ArtCritic.Droid
{
    class AndroidMediaPlayer : IMediaPlayer
    {
		MediaPlayer player = new MediaPlayer();
        public AndroidMediaPlayer()
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