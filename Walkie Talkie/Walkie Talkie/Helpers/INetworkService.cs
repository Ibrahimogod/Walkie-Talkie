using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Walkie_Talkie.Models;

namespace Walkie_Talkie.Helpers
{
    public interface INetworkService
    {
        Task<Network> GetNetworkInfoAsync();
    }
}
