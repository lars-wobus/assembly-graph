using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Plugins.Demo.ObjectAdapter.FakeBackend
{
    public class Server
    {
        IPEndPoint ipep;
        TcpListener _server;
        List<ClientHandler> clients;
        private CancellationTokenSource cts;
        Storage storage = new();

        public void Stop()
        {
            cts.Cancel();
        }

        public async Task Run(string host, int port)
        {
            IPAddress ip = IPAddress.Parse(host);
            ipep = new(ip, port);
            clients = new();

            cts = new CancellationTokenSource();
            _server = new(ipep);
            _server.Start();

            while (!cts.Token.IsCancellationRequested)
            {
                var tcpClient = await _server.AcceptTcpClientAsync();
                var client = new ClientHandler(tcpClient, storage);
                clients.Add(client);
                client.Run().ContinueWith(t => clients.Remove(client));
            }
            _server.Stop();
        }
    }
}
