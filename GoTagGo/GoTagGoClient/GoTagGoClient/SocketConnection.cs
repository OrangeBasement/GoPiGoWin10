using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Buffer = Windows.Storage.Streams.Buffer;

namespace WindowsIoTSocketClient
{
    public static class SocketConnection
    {
        private static HostName _hostName;
        private static StreamSocket _streamSocket;
        private static DataWriter _writer;
        public static async void Connect(string serverIP, string serverPort)
        {
            try
            {
                _hostName = new HostName(serverIP);
                _streamSocket = new StreamSocket();
                await _streamSocket.ConnectAsync(_hostName, serverPort);
                _writer = new DataWriter(_streamSocket.OutputStream);
            }
            catch (Exception e)
            {
                
            }
        }
        public static async void SendMessage(string message)
        {
            _writer.WriteUInt32(_writer.MeasureString(message));
            _writer.WriteString(message);
            await _writer.StoreAsync();
        }
    }
}
