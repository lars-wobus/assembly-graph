#define CLOUD_SAVE

using System;
using UnityEngine;

namespace Plugins.Demo.ObjectAdapter.AppLayer
{
    using SaveSystem;

    public class MainBehaviour : MonoBehaviour
    {
        public async void Start()
        {
            // Sample use case of adapters.
            // Usually you wouldn't need preprocessor directives here.
            // But let's imagine, you would have only included one of two assemblies. 
#if CLOUD_SAVE
            string hostname = "127.0.0.1";
            int port = 8000;
            var server = new FakeBackend.Server();
            server.Run(hostname, port);

            IRequirements adapter = new RemoteStorageAdapter(hostname, port);
            var saveSystem = new SaveSystem(adapter);
#else
            IRequirements adapter = new LocalStorageAdapter();
            var saveSystem = new SaveSystem(adapter);
#endif
            // Some time passes and the player now wants to save his current game state.
            var gameState = new GameState("Player Health: 77%");
            Debug.Log($"Current state: {gameState}");

            // Let's assume that the player has already selected an existing savegame to overwrite and we know its timestamp.
            var dateTime = DateTime.Now;
            await saveSystem.SaveNewSaveState(dateTime, gameState);

            // The player continues his journey, ...
            gameState = new GameState("Player Health: 5%");
            Debug.Log($"New state: {gameState}");

            // ..., but then he decides to load his last savegame.
            gameState = await saveSystem.LoadLastSaveState(dateTime);
            Debug.Log($"Restored state: {gameState}");
        }
    }
}
