using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Buffer = Windows.Storage.Streams.Buffer;

namespace WindowsIoTSocketClient
{
    public static class SocketConnection
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
                    var args = new ConnectionStatusChangedEventArgs {Status = _status};
                    var handler = ConnectStatusChanged;
                    handler?.Invoke(Status, args);
                }
            }
        }
        
        private static HostName _hostName;
        private static StreamSocket _streamSocket;
        private static DataWriter _writer;
        private static DataReader _reader;
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
                //todo:report errors via event to be consumed by UI thread
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

    public enum ConnectionStatus
    {
        Idle = 0,
        Connecting,
        Listening,
        Connected,
        Failed = 99
    }
}
