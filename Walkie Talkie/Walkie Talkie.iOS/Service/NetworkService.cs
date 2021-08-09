using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using SystemConfiguration;
using UIKit;
using Walkie_Talkie.Helpers;
using Xamarin.Forms;

[assembly:Dependency(typeof(Walkie_Talkie.iOS.Service.NetworkService))]
namespace Walkie_Talkie.iOS.Service
{
    class NetworkService : INetworkService
    {
        public async Task<Models.Network> GetNetworkInfoAsync()
        {
            return await Task.Run(()=> new Models.Network()
            {
                BSSID = CaptiveNetwork.NetworkInfoKeyBSSID,
                SSID = CaptiveNetwork.NetworkInfoKeySSID
            });
        }
    }
}