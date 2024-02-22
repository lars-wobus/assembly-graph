namespace Plugins.Demo.ObjectAdapter.FakeBackend
{
    public interface IStorage
    {
        void SaveGameStateFromUser(long userId, string timestamp, string data);
        string LoadPlayerState(long userId, string timestamp);
    }

    public class Storage : IStorage
    {
        private string LastReceivedMessage { get; set; }

        public void SaveGameStateFromUser(long userId, string timestamp, string data)
        {
            // Let's assume that the backend is store the data
            LastReceivedMessage = data;
        }

        public string LoadPlayerState(long userId, string timestamp)
        {
            return LastReceivedMessage;
        }
    }
}