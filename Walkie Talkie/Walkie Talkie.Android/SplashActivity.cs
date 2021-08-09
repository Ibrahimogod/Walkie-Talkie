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
using System.Threading.Tasks;

namespace Walkie_Talkie.Droid
{
    [Activity(Label = "Walkie Talkie", Theme = "@style/MainTheme.Splash", Icon = "@mipmap/icon", MainLauncher = true,NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            await Task.Delay(TimeSpan.FromSeconds(5));
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            
        }
    }
}