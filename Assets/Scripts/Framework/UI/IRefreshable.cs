namespace Game.UI
{
    public interface IRefreshable<T>
    {
        void Refresh(T entity);
    }
}
