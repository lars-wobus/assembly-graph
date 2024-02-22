using System;
using System.Threading.Tasks;

namespace Plugins.Demo.ObjectAdapter.AppLayer
{
    using RemoteStorage;
    using SaveSystem;

    public class RemoteStorageAdapter : IRequirements
    {
        private NetworkClient _client;

        public RemoteStorageAdapter(string hostname, int port)
        {
            _client = new NetworkClient(hostname, port);
        }

        public Task<string> Load(DateTime timestamp)
        {
            return _client.GetData(timestamp);
        }

        public Task Save(DateTime timestamp, string text)
        {
            return _client.PostData(timestamp, text);
        } 
    }
}

