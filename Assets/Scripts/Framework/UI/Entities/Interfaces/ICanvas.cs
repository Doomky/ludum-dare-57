namespace Framework.UI
{
    public interface ICanvas
    {
        void Load();

        void Unload();

        public void LoadWindowGroups();

        public void UnloadWindowGroups();

        void UpdateVisibility();
    }
}