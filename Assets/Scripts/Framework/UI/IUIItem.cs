namespace Framework.UI
{
    public interface IUIItem<T>
    {
        bool IsBound();

        bool IsBoundTo(T entity);

        void Bind(T entity);

        void Unbind();
    }
}
