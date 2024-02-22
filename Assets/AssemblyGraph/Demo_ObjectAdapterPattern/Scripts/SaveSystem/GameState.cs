namespace Plugins.Demo.ObjectAdapter.SaveSystem
{
    public class GameState
    {
        private string Content { get; }

        public GameState(string content)
        {
            Content = content;
        }

        public override string ToString()
        {
            return Content;
        }
    }
}
