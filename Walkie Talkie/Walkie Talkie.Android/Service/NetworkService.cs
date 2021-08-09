using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Walkie_Talkie.Helpers;
using Walkie_Talkie.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(Walkie_Talkie.Droid.Service.NetworkService))]
namespace Walkie_Talkie.Droid.Service
{
    public class NetworkService : INetworkService
    {
        public async Task<Network> GetNetworkInfoAsync()
        {
            WifiManager wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
            WifiInfo info = wifiManager.ConnectionInfo;

            return await Task.Run(() => new Network()
            {
                BSSID = info.BSSID,
                SSID = info.SSID
            });
        }
    }
}