using System;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using GoTagGo.Common;

namespace GoTagGoClient
{
    public static class ClientService
    {
        //exposed to hook into server status changes
        public static event EventHandler<ConnectionStatusChangedEventArgs> ConnectStatusChanged;
        private static ConnectionStatus _status = ConnectionStatus.Idle;

        public static ConnectionStatus Status
        {
            get { return _status; }
            set
            {
                if (value != _status)
                {
                    _status = value;
                    var args = new ConnectionStatusChangedEventArgs { Status = _status };
                    var handler = ConnectStatusChanged;
                    handler?.Invoke(Status, args);
                }
            }
        }

        private static HostName _hostName;
        private static StreamSocket _streamSocket;
        private static DataWriter _writer;

        public static async void Connect(string serverIP, string serverPort)
        {
            try
            {
                Status = ConnectionStatus.Connecting;
                _hostName = new HostName(serverIP);
                _streamSocket = new StreamSocket();
                await _streamSocket.ConnectAsync(_hostName, serverPort);
                Status = ConnectionStatus.Connected;
                _writer = new DataWriter(_streamSocket.OutputStream);
            }
            catch (Exception e)
            {
                Status = ConnectionStatus.Failed;
                //todo:log errors
            }
        }

        public static async void SendMessage(string message)
        {
            _writer.WriteUInt32(_writer.MeasureString(message));
            _writer.WriteString(message);
            await _writer.StoreAsync();
        }
    }

    public class ConnectionStatusChangedEventArgs : EventArgs
    {
        public ConnectionStatus Status;
    }
}
