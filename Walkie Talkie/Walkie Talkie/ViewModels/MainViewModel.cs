using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Walkie_Talkie.Connection;
using Walkie_Talkie.Helpers;
using Walkie_Talkie.Models;
using Walkie_Talkie.Service;
using Walkie_Talkie.Views;
using Xamarin.Forms;

namespace Walkie_Talkie.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        DataBaseService _dbService;

        ObservableCollection<ClientInfo> _clients;

        public ObservableCollection<ClientInfo> Clients
        {
            get => _clients;
            set
            {
                _clients = value;
                NotifyPropertyChanged(nameof(Clients));
            }
        }


        public ICommand TappedCommand { get; private set; }

        public ICommand AddCommand { get; private set; }

        public ICommand InfoCommand { get; private set; }

        public ICommand RefreshCommand { get; private set; }

        bool _isRefreshing;
        public bool IsRefreshing 
        {  
            get =>_isRefreshing;
            set 
            {
                _isRefreshing = value;
                NotifyPropertyChanged(nameof(IsRefreshing));
            }
        }

        public MainViewModel()
        {
            Clients = new ObservableCollection<ClientInfo>();
            _dbService = new DataBaseService();

            _dbService.Initializing += (sender, e) => Init();

            TappedCommand = new Command(async (context) =>
            {
                ClientInfo client = context as ClientInfo;
                await App.Current.MainPage.Navigation.PushAsync(new CommunicationPage(client));
            });

            AddCommand = new Command(async () =>
            {
                await App.Current.MainPage.Navigation.PushAsync(new AddUserPage());
            });

            InfoCommand = new Command(async () =>
            {
                await App.Current.MainPage.Navigation.PushAsync(new InfoPage());
            });

            RefreshCommand = new Command(async () =>
            {
                await ReloadData();
            });
        }

        async void Init()
        {
            Network network = await DependencyService.Get<INetworkService>().GetNetworkInfoAsync();

            await _dbService.AddNetworkAsync(network);

            await ReloadData();
        }

        public async Task ReloadData()
        {
            if (_dbService.Initialized && _isRefreshing)
            {
                Clients = new ObservableCollection<ClientInfo>(await _dbService.GetClientsAsync());
                IsRefreshing = false;
            }

        }
    }
}
