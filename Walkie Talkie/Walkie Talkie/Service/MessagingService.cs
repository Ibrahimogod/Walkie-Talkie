using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Walkie_Talkie.Connection;
using Walkie_Talkie.Connection.Connector.Audio;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using Walkie_Talkie.Helpers;

namespace Walkie_Talkie.Service
{
    public class MessagingService
    {
        Client _client;
        IRecorderService _recorder;
        public MessagingService(Client client)
        {
            _client = client;
            _recorder = DependencyService.Get<IRecorderService>();
        }


        public string ServerAddress { get => _client.ClientAddress.ToString(); }

        public async Task ConnectAsync()
        {
            await _client.ConnectAsync();
        }

        public async Task StartRecording()
        {
            AudioMessage messageBeginng = AudioMessage.Create(true, false, new byte[0]);
            await _client.SendAsync(messageBeginng);
            _recorder.startRecording(OnStreamBroadcast);
        }



        public async Task StopRecording()
        {
            _recorder.stopRecording();
            AudioMessage messageEnd = AudioMessage.Create(false, true, new byte[0]);
            await _client.SendAsync(messageEnd);
        }

        async Task OnStreamBroadcast(byte[] audio)
        {
            AudioMessage message = AudioMessage.Create(false, false, audio);
            await _client.SendAsync(message);
        }



    }
}
