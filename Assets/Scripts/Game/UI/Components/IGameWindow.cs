using Framework.UI;

namespace Game.UI
{
    public interface IGameWindow : IWindow
    {
        void OnInGameEntered();
        
        void OnInGameExited();
    }
}
