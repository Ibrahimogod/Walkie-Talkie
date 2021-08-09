using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Timers;

namespace Walkie_Talkie.Connection.Connector
{

    public class ChannelManager
    {
        readonly Func<IChannel> _channelFactory;
        readonly ConcurrentDictionary<Guid, IChannel> _channels;

        const int GROOMIN_INTERVAL_MINUTES = 1;
        readonly Timer _groomer;

        public ChannelManager(Func<IChannel> channelFactory)
        {
            _channelFactory = channelFactory;
            _channels = new ConcurrentDictionary<Guid, IChannel>();
            _groomer = new Timer(GROOMIN_INTERVAL_MINUTES * 60 * 1000);
            _groomer.Elapsed += OnElapsedEventHandler;
            _groomer.Start();
        }

        private void OnElapsedEventHandler(object sender, ElapsedEventArgs e)
        {
            _groomer.Stop();
            Console.WriteLine("BEGIN SOCKET GROOMING");
            int socketGroomed = 0;
            try
            {

                var deidChannels = new List<Guid>();

                var delta = DateTime.Now.Subtract(new TimeSpan(0, GROOMIN_INTERVAL_MINUTES, 0));
                foreach (var id in _channels.Keys)
                {
                    var c = _channels[id];
                    var mostRecent = DateTime.Compare(c.LastReceived, c.LastSent) > 0 ? c.LastReceived : c.LastSent;
                    if (DateTime.Compare(delta, mostRecent) < 0)
                    {
                        deidChannels.Add(id);
                    }

                }

                foreach (var key in deidChannels)
                {
                    Console.WriteLine($"Channel with id {key}'s been Groomed");
                    var c = _channels[key];
                    c.Close();
                    socketGroomed++;
                }

            }
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}
            finally
            {
                _groomer.Start();
            }
        }

        public void Accept(Socket socket)
        {
            var channel = _channelFactory();
            _channels.TryAdd(channel.Id, channel);
            channel.Closed += (o, e) =>
            {
                _channels.TryRemove(channel.Id, out var _);
            };
            channel.Attach(socket);
        }

    }
}