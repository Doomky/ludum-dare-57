namespace Framework.UI
{
    public interface IWindow : IUIEntity
    {
        bool IsVisible { get; }

        void Load();
        
        void Unload();

        void UpdateVisibility();
    }
}