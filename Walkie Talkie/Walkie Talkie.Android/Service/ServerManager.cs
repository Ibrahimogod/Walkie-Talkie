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
using Walkie_Talkie.Droid.Service;
using Walkie_Talkie.Helpers;
using Xamarin.Forms;

[assembly:Dependency(typeof(ServerManager))]
namespace Walkie_Talkie.Droid.Service
{
    class ServerManager : IServerManager
    {
        public void Start()
        {
            InitBackgroundService();
        }

        void InitBackgroundService()
        {
            Intent serverintent = new Intent(Android.App.Application.Context, typeof(ServerBackgroundService));
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                Android.App.Application.Context.StartForegroundService(serverintent);
            else
                Android.App.Application.Context.StartService(serverintent);
        }
    }
}