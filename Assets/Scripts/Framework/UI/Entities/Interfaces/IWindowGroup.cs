using System.Collections.Generic;

namespace Framework.UI
{
    public interface IWindowGroup : IUIEntity
    {
        List<Window> Windows { get; }

        bool IsVisible { get; }

        void Load();

        void Unload();
        
        void UpdateVisibility();

        public void Add(Window window);

        public void Remove(Window window);
    }
}