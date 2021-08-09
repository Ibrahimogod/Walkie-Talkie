using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Walkie_Talkie.Helpers;
using Walkie_Talkie.Models;
using Walkie_Talkie.UWP.Service;
using Windows.Devices.WiFi;
using Windows.Networking.Connectivity;
using Xamarin.Forms;

[assembly: Dependency(typeof(NetworkService))]
namespace Walkie_Talkie.UWP.Service
{
    class NetworkService : INetworkService
    {
        public async Task<Network> GetNetworkInfoAsync()
        {

            var ConnectionProfile = NetworkInformation.GetInternetConnectionProfile();

            var ssid = "";
            if (ConnectionProfile.IsWwanConnectionProfile)
            {
                ssid = ConnectionProfile.WwanConnectionProfileDetails.AccessPointName;
            }
            else if (ConnectionProfile.IsWlanConnectionProfile)
            {
                ssid = ConnectionProfile.WlanConnectionProfileDetails.GetConnectedSsid();
            }
            var access = await WiFiAdapter.RequestAccessAsync();


            var adapters = await WiFiAdapter.FindAllAdaptersAsync();

            WiFiAvailableNetwork network = adapters.Select(a => a.NetworkReport.AvailableNetworks)
                 .Where((a) =>
                 a.FirstOrDefault(n => n.Ssid == ssid) != null).FirstOrDefault().FirstOrDefault();

            return new Network()
            {
                BSSID = network.Bssid,
                SSID = network.Ssid
            };
        }
    }
}
