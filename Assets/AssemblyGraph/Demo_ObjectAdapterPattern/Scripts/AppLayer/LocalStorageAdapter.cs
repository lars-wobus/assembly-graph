using System;
using System.Threading.Tasks;

namespace Plugins.Demo.ObjectAdapter.AppLayer
{
    using LocalStorage;
    using SaveSystem;

    public class LocalStorageAdapter : IRequirements
    {
        private LocalFileStorage _localStorage;

        public LocalStorageAdapter()
        {
            _localStorage = new LocalFileStorage();
        }

        public Task<string> Load(DateTime timestamp)
        {
            return _localStorage.ReadFile(timestamp);
        }

        public Task Save(DateTime timestamp, string text)
        {
            return _localStorage.WriteFile(timestamp, text);
        }
    }
}