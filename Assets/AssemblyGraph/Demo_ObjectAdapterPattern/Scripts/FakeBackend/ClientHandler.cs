using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Plugins.Demo.ObjectAdapter.FakeBackend
{
    class ClientHandler
    {
        IStorage _storage;
        NetworkStream _stream;

        public ClientHandler(TcpClient client, IStorage storage)
        {
            _stream = client.GetStream();
            _storage = storage;
        }

        public async Task Run()
        {
            byte[] bytes = new byte[256];
            string data = null;
            int i;

            await Task.Run(() =>
            {
                while ((i = _stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = Encoding.ASCII.GetString(bytes, 0, i);
                    Debug.Log($"Raw data received: {data}");

                    ProcessMessage(data);
                }
            });
        }

        private void ProcessMessage(string content)
        {
            var message = new ClientRequest(content);
            if (message.Command == ClientRequest.LoadState)
            {
                var data = _storage.LoadPlayerState(message.UserId, message.Timestamp);
                SendResponseMessage(data);
            }
            else if (message.Command == ClientRequest.SaveState)
            {
                _storage.SaveGameStateFromUser(message.UserId, message.Timestamp, message.Data);
                SendResponseMessage("Status:Complete");
            }
        }

        private void SendResponseMessage(string response)
        {
            var bytes = Encoding.UTF8.GetBytes(response);
            _stream.Write(bytes, 0, bytes.Length);
        }

        public class ClientRequest
        {
            public const string LoadState = nameof(LoadState);
            public const string SaveState = nameof(SaveState);

            public long UserId { get; protected set; }
            public string Timestamp { get; protected set; }
            public string Command { get; protected set; }
            public string Data { get; set; }

            public ClientRequest(string content)
            {
                var data = content.Split("|");
                if (data.Length > 0) Command   = data[0];
                if (data.Length > 1) Timestamp = data[1];
                if (data.Length > 2) Data      = data[2];
            }

            public byte[] GetBytes() => Encoding.ASCII.GetBytes(ToString());

            public override string ToString() => $"{Command}|{Timestamp}|{Data}";
        }
    }
}