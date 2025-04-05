namespace Framework.Managers
{
    public interface IPersistentField<T> : IPersistentField
    {
        new T Value
        {
            get;
            set;
        }
    }

    public interface IPersistentField
    {
        void Init();

        object Value
        {
            get;
            set;
        }
    }
}
