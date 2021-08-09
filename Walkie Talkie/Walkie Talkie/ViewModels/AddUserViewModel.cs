using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Walkie_Talkie.Helpers;
using Walkie_Talkie.Models;
using Walkie_Talkie.Service;
using Xamarin.Forms;

namespace Walkie_Talkie.ViewModels
{
    class AddUserViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        DataBaseService _dbService;

        public AddUserViewModel()
        {
            _dbService= new DataBaseService();

            AddClientCommand = new Command(async () =>
            {
                string ipEndpoint = $"{IPAddress}:{Port}";
                string bssid = (await DependencyService.Get<INetworkService>().GetNetworkInfoAsync()).BSSID;
                ClientInfo client = new ClientInfo()
                {
                    IPAddress = this.IPAddress,
                    Port = int.Parse(this.Port),
                    IPEndPoint = ipEndpoint,
                    Name = this.Name,
                    BSSID = bssid
                };
                await _dbService.AddClientAsync(client);
                await App.Current.MainPage.Navigation.PopAsync();
            });
        }

        public ICommand AddClientCommand { get; }

        public int LayoutHeight { get => App.Height - 80; }

        public int LayoutWidth { get => App.Width - 20; }

        public int FormInputWidth { get => LayoutWidth - 50; }

        string _ipAddress;
        public string IPAddress
        {
            get => _ipAddress;
            set
            {
                _ipAddress = value;
                NotifyPropertyChanged(nameof(IPAddress));
            }
        }

        string _port;
        public string Port
        {
            get => _port;
            set
            {
                _port = value;
                NotifyPropertyChanged(nameof(Port));
            }
        }

        string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyPropertyChanged(nameof(Name));
            }
        }
    }
}
