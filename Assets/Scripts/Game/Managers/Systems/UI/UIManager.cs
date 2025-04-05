using Framework.UI;
using Game.Core;
using Game.UI;
using System.Collections.Generic;

namespace Game.Managers
{
    public class UIManager : Framework.Managers.UIManager
    {
        public override void PostLayerLoad()
        {
            base.PostLayerLoad();

            Core.Game.Singleton.GameStateEntered += this.OnGameStateEntered;
            Core.Game.Singleton.GameStateExited += this.OnGameStateExited;
        }

        public override void PreLayerUnload()
        {
            Core.Game.Singleton.GameStateExited -= this.OnGameStateExited;
            Core.Game.Singleton.GameStateEntered -= this.OnGameStateEntered;

            base.PreLayerUnload();
        }

        private void OnGameStateEntered(Framework.GameState gameState)
        {
            if (gameState is not GameState_InGame)
            {
                return;
            }

            this.OnInGameEntered();
        }

        private void OnGameStateExited(Framework.GameState gameState)
        {
            if (gameState is not GameState_InGame)
            {
                return;
            }

            this.OnExitedInGame();
        }

        private void OnInGameEntered()
        {
            int canvasCount = this.Canvases.Count;
            for (int i = 0; i < canvasCount; i++)
            {
                Canvas canvas = this.Canvases[i];
                IReadOnlyList<WindowGroup> windowGroups = canvas.WindowGroups;
                int count = windowGroups.Count;
                for (int j = 0; j < count; j++)
                {
                    WindowGroup windowGroup = windowGroups[j];

                    List<Window> windows = windowGroup.Windows;
                    int windowCount = windows.Count;
                    for (int k = 0; k < windowCount; k++)
                    {
                        Window window = windows[k];
                        if (window is IGameWindow gameWindow)
                        {
                            gameWindow.OnInGameEntered();
                        }
                    }
                }
            }
        }

        private void OnExitedInGame()
        {
            int canvasCount = this.Canvases.Count;
            for (int i = 0; i < canvasCount; i++)
            {
                Canvas canvas = this.Canvases[i];
                IReadOnlyList<WindowGroup> windowGroups = canvas.WindowGroups;
                int count = windowGroups.Count;
                for (int j = 0; j < count; j++)
                {
                    WindowGroup windowGroup = windowGroups[j];

                    List<Window> windows = windowGroup.Windows;
                    int windowCount = windows.Count;
                    for (int k = 0; k < windowCount; k++)
                    {
                        Window window = windows[k];
                        if (window is IGameWindow gameWindow)
                        {
                            gameWindow.OnInGameExited();
                        }
                    }
                }
            }
        }
    }
}
