namespace Framework.Managers
{
    public interface IManager
    {
        void Load();

        void Unload();

        void PostLayerLoad();

        void PreLayerUnload();
    }
}
