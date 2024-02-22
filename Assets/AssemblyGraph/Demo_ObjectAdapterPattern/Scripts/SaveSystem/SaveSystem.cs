using System;
using System.Threading.Tasks;

namespace Plugins.Demo.ObjectAdapter.SaveSystem
{
    public class SaveSystem
    {
        private IRequirements Requirements { get; }

        public SaveSystem(IRequirements requirements)
        {
            Requirements = requirements;
        }

        public async Task<GameState> LoadLastSaveState(DateTime timestamp)
        {
            string text = await Requirements.Load(timestamp);
            return new GameState(text);
        }

        public async Task SaveNewSaveState(DateTime timestamp, GameState gameState)
        {
            await Requirements.Save(timestamp, gameState.ToString());
        }
    }
}