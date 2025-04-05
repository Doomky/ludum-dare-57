using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

namespace Game.Core
{
    public partial class Game
    {
        [TitleGroup("Debug", Order = 3)]
        [Button("Restart", ButtonSizes.Large)]
        public void RestartGame()
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        [TitleGroup("Debug", Order = 3)]
        [Button("Go To Ingame", ButtonSizes.Large)]
        public void GoToIngame()
        {
            this._stateMachine.InjectAction(GameAction.GoToIngame);
        }

        [TitleGroup("Debug", Order = 3)]
        [Button("Go To Outgame", ButtonSizes.Large)]
        public void GoToOutgame()
        {
            this._stateMachine.InjectAction(GameAction.GoToOutgame);
        }
    }
}