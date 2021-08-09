using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;

namespace Walkie_Talkie.Models
{
    public static class Constants
    {
        public static string CONNECTION_STRING { get => Path.Combine(FileSystem.AppDataDirectory,"Connections.db3"); }
    }
}
