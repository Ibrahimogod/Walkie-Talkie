using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Walkie_Talkie.Connection.Connector.Audio;

namespace Walkie_Talkie.Connection
{
    public class Client : IDisposable
    {
        bool _connected;
        IPEndPoint _clientEndpoint;
        AudioClientChannel _clientChannel;
        

        public IPEndPoint ClientAddress { get => _clientEndpoint; }

        public bool IsConnected { get => _clientChannel.IsConnected && _connected; }


        public Client(string ipAddress, int port)
        {
            _clientEndpoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            _clientChannel = new AudioClientChannel();
        }

        public async Task ConnectAsync()
        {
            await _clientChannel.ConnectAsync(_clientEndpoint);
            _connected = true;
        }

        public async Task SendAsync(AudioMessage message)
        {
            await _clientChannel.SendAsync(message);
        }

        public void Dispose()
        {
            if (_clientChannel != null)
            {
                _clientChannel.Dispose();
                _clientChannel.Close();
                GC.SuppressFinalize(this);
            }
            _connected = false;
            GC.Collect();
        }

        ~Client()
        {
            Dispose();
        }
    }
}
