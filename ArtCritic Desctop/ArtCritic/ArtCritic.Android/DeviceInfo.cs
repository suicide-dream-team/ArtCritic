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
using Xamarin.Forms;

[assembly: Dependency(typeof(ArtCritic.Droid.DeviceInfo))]
namespace ArtCritic.Droid
{
    public class DeviceInfo : IDeviceInfo
    {
        public string GetInfo()
        {
            return $"Android {Build.VERSION.Release}";
        }
    }
}