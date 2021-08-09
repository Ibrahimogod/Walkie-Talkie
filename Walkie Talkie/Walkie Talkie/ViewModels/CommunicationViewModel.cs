using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Walkie_Talkie.Connection;
using Walkie_Talkie.Helpers;
using Walkie_Talkie.Models;
using Walkie_Talkie.Service;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Walkie_Talkie.ViewModels
{
    public class CommunicationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        ClientInfo _client;
        public ClientInfo Client { 
            get=>_client; 
            set
            { 
                _client = value;
                _service = new MessagingService(new Client(_client.IPAddress, _client.Port));
                NotifyPropertyChanged(nameof(ClientAddress));
                NotifyPropertyChanged(nameof(ClientName));

            }
        }

        public ICommand ConnectCommand { get; }

        public ICommand RecordCommand { get; }

        public ICommand StopCommand { get; }

        public ICommand DeleteCommand { get; }

        public string ClientAddress { get => (_client!=null) ? _client.IPEndPoint : "N/A" ; }

        public string ClientName { get => (_client != null) ? _client.Name : "N/A"; }

        MessagingService _service;
        DataBaseService _dbService;
        public CommunicationViewModel()
        {
            _dbService = new DataBaseService();

            ConnectCommand = new Command(async () => {
                try
                {
                    await _service.ConnectAsync();
                }
                catch
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Can't Connect To This Member", "Ok");
                }
            });

            RecordCommand = new Command(async () => {
                await _service.StartRecording();
            });

            StopCommand = new Command(async () => { 
                await  _service.StopRecording();
            });

            DeleteCommand = new Command(async () => {
                var delete = await App.Current.MainPage.DisplayAlert("Confirmation", "Are you sure you wan't to Delete This Member?", "Yes","Cancel",FlowDirection.LeftToRight);
                
                if (delete)
                {
                    await _dbService.DeleteClientAsync(Client);
                    await App.Current.MainPage.Navigation.PopAsync();
                }
            });
        }
    }
}


