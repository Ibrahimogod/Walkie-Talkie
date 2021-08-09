using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Essentials;

namespace Walkie_Talkie.ViewModels
{
    class InfoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged([CallerMemberName]string propertyName ="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        string _serverAddress;
        public string ServerAddress{
            get => _serverAddress;
            set
            {
                _serverAddress = value;
                NotifyPropertyChanged(nameof(ServerAddress));
            }
        }

        public InfoViewModel()
        {
            ServerAddress = App.ServerAddress;
            Connectivity.ConnectivityChanged += (s, e) =>
            {
                ServerAddress = App.ServerAddress;
            };
        }



    }
}
