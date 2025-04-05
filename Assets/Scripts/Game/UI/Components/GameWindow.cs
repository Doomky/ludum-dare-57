using Framework.UI;

namespace Game.UI
{
    public abstract class GameWindow : Window, IGameWindow
    {
        public abstract void OnInGameEntered();
        
        public abstract void OnInGameExited();
    }
}
