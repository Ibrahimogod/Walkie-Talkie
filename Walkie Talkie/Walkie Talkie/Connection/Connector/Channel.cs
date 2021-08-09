using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Walkie_Talkie.Connection.Connector
{

    public abstract class Channel<TProtocol, TMessageType> : IDisposable, IChannel where TProtocol : Protocol<TMessageType>, new()
    {

        protected bool _isDisposed = false;
        protected bool _isClosed = false;


        readonly TProtocol _protocol = new TProtocol();
        readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        Func<TMessageType, Task> _messageCallback;
        NetworkStream _networkStream;
        Task _receiveLoopTask;
        public event EventHandler Closed;

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.



        public Guid Id { get; } = Guid.NewGuid();

        public DateTime LastSent { get; protected set; }

        public DateTime LastReceived { get; protected set; }

        public bool IsConnected { get => _networkStream.CanWrite; }

        public void Attach(Socket socket)
        {
            _networkStream = new NetworkStream(socket, true);
            _receiveLoopTask = Task.Run(ReceiveLoop, _cancellationTokenSource.Token);
        }

        public void OnMessage(Func<TMessageType, Task> callbackHandler)
            => _messageCallback = callbackHandler;

        public void Close()
        {
            if (!_isClosed)
            {
                _isClosed = true;
                _cancellationTokenSource.Cancel();
                _networkStream?.Close();
                _receiveLoopTask?.Wait();
                Closed?.Invoke(this, EventArgs.Empty);
            }
        }

        public async Task SendAsync<T>(T message)
        {
            await _protocol.SendAsync(_networkStream, message).ConfigureAwait(false);
            LastSent = DateTime.Now;
        }

        protected virtual async Task ReceiveLoop()
        {
            try
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    //TODO: Pass Cancellation Token to Protocol methods
                    var msg = await _protocol.ReceiveAsync(_networkStream).ConfigureAwait(false);
                    LastReceived = DateTime.Now;
                    await _messageCallback(msg).ConfigureAwait(false);
                }
            }
            catch (IOException)
            {
                Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ReceiveLoop Exception" + ex);
                Close();
            }
        }

        ~Channel() => Dispose(false);
        public void Dispose() => Dispose(true);
        protected void Dispose(bool isDisposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                Close();
                //TODO: Clean up socket, stream, etc.
                _networkStream?.Dispose();

                if (isDisposing)
                    GC.SuppressFinalize(this);
            }
        }
    }
}

