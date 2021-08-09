using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Walkie_Talkie.Connection.Connector;
using Walkie_Talkie.Connection.Connector.Audio;
using Xamarin.Essentials;
using Xamarin.Forms;
using Walkie_Talkie.Helpers;

#nullable enable

namespace Walkie_Talkie.Connection
{
    public class Server : IDisposable
    {
        ChannelManager _channelManager;
        Random _portPool;
        Func<AudioMessage, Task> _handleReceivedMessage;

        int _port;
        IPEndPoint? _serverAddress;
        public IPEndPoint? ServerAddress { get => _serverAddress; }

        bool _isRunning;
        public bool IsRunning { get => _isRunning; }

        Server(Func<AudioMessage, Task> receivedMessageHandler)
        {
            _portPool = new Random();
            _channelManager = new ChannelManager(Factory);
            _handleReceivedMessage = receivedMessageHandler;
        }

        static Server? _instance;
        public static Server GetInstance(Func<AudioMessage, Task> receivedMessageHandler)
        {
            if (_instance == null)
            {
                _instance = new Server(receivedMessageHandler);
            }
            return _instance;
        }

        AudioChannel Factory()
        {
            //not finished yet
            AudioChannel channel = new AudioChannel();
            channel.OnMessage(_handleReceivedMessage);
            return channel;
        }

        public async void StartAsync()
        {
            int tried = 0;
        retry:
            if (!_isRunning)
                try
                {
                    _port = await GetPortAsync(tried > 0);
                    var endPoint = new IPEndPoint(IPAddress.Any, _port);
                    var socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    socket.Bind(endPoint);
                    socket.Listen(128);
                    SetPort(_port);
                    _serverAddress = new IPEndPoint(GetLoacalIP(), _port);
                    App.ServerAddress = _serverAddress.ToString();
                    _ = Task.Run(() => RunAsync(socket));
                    _isRunning = true;
                }
                catch (SocketException)
                {
                    if (tried < 5)
                    {
                        tried++;
                        goto retry;
                    }
                    else
                    {
                        throw;
                    }
                }
        }

        private async Task RunAsync(Socket socket)
        {

            do
            {
                var clientSocket = await Task.Factory.FromAsync(
                    new Func<AsyncCallback, object?, IAsyncResult>(socket.BeginAccept),
                    new Func<IAsyncResult, Socket>(socket.EndAccept),
                    null).ConfigureAwait(false);

                var ip = clientSocket.RemoteEndPoint;
                Console.WriteLine("SERVER :: CLIENT CONNECTION REQUEST");

                _channelManager.Accept(clientSocket);

                Console.WriteLine("SERVER :: CLIENT CONNECTED");


            } while (true);
        }

        IPAddress GetLoacalIP()
        {
            var localIP = IPAddress.Parse("0.0.0.0");
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        localIP = ip;
                    }
                }
                return localIP;
            }
            throw new NetworkInformationException();
        }

        async Task<int> GetPortAsync(bool isPortInUse)
        {
            if (!isPortInUse)
            {
                var network = await DependencyService.Get<INetworkService>().GetNetworkInfoAsync();
                var bssid = network.BSSID;
                var strport = await SecureStorage.GetAsync($"{bssid}:{GetLoacalIP()}");
                if (!string.IsNullOrWhiteSpace(strport))
                {
                    return int.Parse(strport);
                }
            }
            return GetRandomPort();

        }

        async void SetPort(int port)
        {
            var network = await DependencyService.Get<INetworkService>().GetNetworkInfoAsync();
            var bssid = network.BSSID;
            var key = $"{bssid}:{GetLoacalIP().ToString()}";
            var strport = await SecureStorage.GetAsync(key);
            if (string.IsNullOrWhiteSpace(strport))
            {
                await SecureStorage.SetAsync(key, port.ToString());
            }
        }

        int  GetRandomPort()
        {

            return _portPool.Next(1000, 9999);
        }



        public void Dispose()
        {
            if (_instance != null)
            {
                _isRunning = false;
                GC.SuppressFinalize(this);
            }
            GC.Collect();
        }

        ~Server()
        {
            Dispose();
        }
    }
}
