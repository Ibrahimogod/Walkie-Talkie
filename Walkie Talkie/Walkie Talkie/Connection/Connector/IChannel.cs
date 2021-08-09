using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Walkie_Talkie.Connection.Connector
{
    public interface IChannel
    {
        Guid Id { get; }

        DateTime LastSent { get; }
        DateTime LastReceived { get; }

        event EventHandler Closed;

        void Attach(Socket socket);
        void Close();
        void Dispose();
        Task SendAsync<T>(T message);
    }
}
