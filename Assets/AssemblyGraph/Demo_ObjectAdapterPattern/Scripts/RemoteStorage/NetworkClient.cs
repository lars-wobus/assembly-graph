using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Demo.ObjectAdapter.RemoteStorage
{
    public class NetworkClient
    {
        public const string LoadState = nameof(LoadState);
        public const string SaveState = nameof(SaveState);

        TcpClient _tcpClient;
        NetworkStream _stream;

        public NetworkClient(string hostname, int port)
        {
            _tcpClient = new TcpClient(hostname, port);
            _stream = _tcpClient.GetStream();
        }

        public async Task<string> GetData(DateTime timestamp)
        {
            return await Task.Run(() =>
            {
                return SendCommand(LoadState, timestamp);
            });
        }

        public async Task<string> PostData(DateTime timestamp, string message)
        {
            return await Task.Run(() =>
            {
                return SendCommand(SaveState, timestamp, message);
            });
        }

        private string SendCommand(string command, DateTime timestamp, string content = null)
        {
            string message = $"{command}|{timestamp}|{content}";
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            _stream.Write(bytes, 0, bytes.Length);
            bytes = new byte[256];
            _stream.Read(bytes, 0, bytes.Length);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}